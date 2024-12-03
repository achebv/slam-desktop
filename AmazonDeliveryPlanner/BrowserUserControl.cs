// using CamioaneAmazon.CEF;
using CefSharp;
using CefSharp.WinForms;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AmazonDeliveryPlanner
{
    public partial class BrowserUserControl : UserControl, IDisposable
    {
        ChromiumWebBrowser browser;

        string url;

        RequestContextSettings requestContextSettings;

        public event EventHandler Close;
        public event EventHandler<FileUploadFinishedEventArgs> FileUploadFinished;

        public event EventHandler<UpdateAutoDownloadIntervalStatusEventArgs> UpdateAutoDownloadStatus;

        long driverId;

        public BrowserUserControl(string url, RequestContextSettings requestContextSettings, long driverId)
        {
            this.driverId = driverId;
            this.url = url;
            this.requestContextSettings = requestContextSettings;

            InitializeComponent();

            InitBrowser();
            //. InitPanel2Browser();

            // new Task(() => { Thread.Sleep(800); InitBrowser(); }).Start();

            browser.PreviewKeyDown += Browser_PreviewKeyDown;
            browser.KeyUp += Browser_KeyUp;
            browser.KeyboardHandler = new BrowserKeyboardHandler();

            urlTextBox.Text = url;
        }

        private void Browser_KeyUp(object sender, KeyEventArgs e)
        {
            e.SuppressKeyPress = false;
        }

        private void Browser_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            if ((e.Control || e.KeyCode == Keys.ControlKey) && (e.KeyCode == Keys.F))
                GlobalContext.MainWindow.OpenSearchDriverForm();
        }

        //public delegate void InitBrowserHandler();
        //            if (this.InvokeRequired)
        //    {
        //    InitBrowserHandler ofc = new InitBrowserHandler(InitBrowser);

        //    if (!this.IsDisposed)
        //    {
        //        this.Invoke(ofc, new object[] { });
        //        return;
        //    }

        //    return;
        //}
        //    else
        //    {

        void InitBrowser()
        {
            browser = new ChromiumWebBrowser();

            if (requestContextSettings != null)
                browser.RequestContext = new RequestContext(requestContextSettings);

            browser.DownloadHandler = new DownloadHandler();

            ((DownloadHandler)browser.DownloadHandler).OnDownloadUpdatedFired += BrowserUserControl_OnDownloadUpdatedFired;

            panel1.Controls.Add(browser);

            browser.Dock = DockStyle.Fill;

            browser.FrameLoadEnd += Browser_FrameLoadEnd;


            this.PerformLayout();
            this.Invalidate();
            this.Refresh();

            browser.Load(url);

            browser.Dock = DockStyle.Fill;

            browser.TitleChanged += Browser_TitleChanged;

            InitAutoDownloadTimer(url);
        }

        void InitAutoDownloadTimer(string loadedUrl)
        {
            // https://relay.amazon.co.uk/tours/in-transit
            if (loadedUrl.IndexOf("in-transit", StringComparison.OrdinalIgnoreCase) >= 0 &&
                loadedUrl.IndexOf("relay.amazon", StringComparison.OrdinalIgnoreCase) >= 0)
            {
                if (GlobalContext.SerializedConfiguration.AutoDownloadExportFileRandomMinInterval > 0 &&
                    GlobalContext.SerializedConfiguration.AutoDownloadExportFileRandomMaxInterval > 0 &&
                    GlobalContext.SerializedConfiguration.AutoDownloadExportFileRandomMinInterval < GlobalContext.SerializedConfiguration.AutoDownloadExportFileRandomMaxInterval)
                {
                    if (autoDownloadIntervalTask != null)
                    {
                        ts.Cancel();
                        // aut
                    }

                    ts = new CancellationTokenSource();

                    autoDownloadIntervalTask = Task.Run(() =>
                        {
                            GlobalContext.Log("Started auto download with random interval between {0} and {1} seconds", GlobalContext.SerializedConfiguration.AutoDownloadExportFileRandomMinInterval, GlobalContext.SerializedConfiguration.AutoDownloadExportFileRandomMaxInterval);
                            StartAutoDownloadInterval(true);
                        },
                        ts.Token
                );
                }
                else
                {
                    GlobalContext.Log("Auto download with random interval not started because of the configured values 1 - interval between {0} and {1} seconds", GlobalContext.SerializedConfiguration.AutoDownloadExportFileRandomMinInterval, GlobalContext.SerializedConfiguration.AutoDownloadExportFileRandomMaxInterval);
                }
            }
        }

        Task autoDownloadIntervalTask = null;
        CancellationTokenSource ts;

        void StartAutoDownloadInterval(bool first)
        {
            if (first)
            {
                int delayBase = GlobalContext.SerializedConfiguration.AutoDownloadExportFileRandomMinInterval;
                int addedRandom = GlobalContext.SerializedConfiguration.AutoDownloadExportFileRandomMaxInterval - GlobalContext.SerializedConfiguration.AutoDownloadExportFileRandomMinInterval;

                int waitPeriodSec = (int)(delayBase + (new Random(DateTime.Now.Millisecond)).NextDouble() * addedRandom);

                TimeSpan waitPeriod = TimeSpan.FromSeconds(waitPeriodSec);

                if (UpdateAutoDownloadStatus != null)
                    UpdateAutoDownloadStatus(this, new UpdateAutoDownloadIntervalStatusEventArgs(String.Format("Waiting {0:00} seconds before downloading export file.", waitPeriod.TotalSeconds)));

                GlobalContext.Log("Auto download with random interval - waiting {0} s", waitPeriod.TotalSeconds);
                Thread.Sleep(waitPeriod);
            }

            {
                if (ts.IsCancellationRequested)
                    return;

                this.Invoke((MethodInvoker)delegate
                {
                    ClickExportTripsFile();
                });

                int delayBase = GlobalContext.SerializedConfiguration.AutoDownloadExportFileRandomMinInterval;
                int addedRandom = GlobalContext.SerializedConfiguration.AutoDownloadExportFileRandomMaxInterval - GlobalContext.SerializedConfiguration.AutoDownloadExportFileRandomMinInterval;

                int waitPeriodSec = (int)(delayBase + (new Random(DateTime.Now.Millisecond)).NextDouble() * addedRandom);

                TimeSpan waitPeriod = TimeSpan.FromSeconds(waitPeriodSec);

                if (ts.IsCancellationRequested)
                    return;

                if (UpdateAutoDownloadStatus != null)
                    UpdateAutoDownloadStatus(this, new UpdateAutoDownloadIntervalStatusEventArgs(String.Format("Last export made at {0}. Expoting again at {1},  in {2:00} s", DateTime.Now.ToString("HH:mm:ss"), DateTime.Now.Add(waitPeriod).ToString("HH:mm:ss"), waitPeriod.TotalSeconds)));

                GlobalContext.Log("Auto download with random interval - waiting {0} s", waitPeriod.TotalSeconds);
                Thread.Sleep(waitPeriod);

                StartAutoDownloadInterval(false);
            }
        }

        private async void BrowserUserControl_OnDownloadUpdatedFired(object sender, DownloadItem e)
        {
            if (!e.IsComplete) return;

            //string uploadURL = "";

            //try
            //{
            //    uploadURL = GlobalContext.SerializedConfiguration.AdminURL + GlobalContext.SerializedConfiguration.ApiBaseURL + GlobalContext.SerializedConfiguration.FileUploadURL;

            //    GlobalContext.Log("Upload URL=\"{0}\"", uploadURL);

            //    string fileName = e.SuggestedFileName;
            //    if (string.IsNullOrEmpty(fileName))
            //        fileName = Path.GetFileName(e.FullPath);

            //    string csvFileContents = File.ReadAllText(e.FullPath);
            //    csvFileContents = csvFileContents.Replace(",Operator ID,Spot Work", ",Operator ID,Spot Work,ColBC,COlBD");

            //    byte[] bytes = Encoding.UTF8.GetBytes(csvFileContents);
            //    HttpContent bytesContent = new ByteArrayContent(bytes);

            //    using (var client = new HttpClient())
            //    using (var formData = new MultipartFormDataContent())
            //    {
            //        formData.Add(bytesContent, "files", fileName);

            //        var response = await client.PostAsync(uploadURL, formData);

            //        if (!response.IsSuccessStatusCode)
            //        {
            //            // Handle non-successful response
            //        }

            //        using (var stream = await response.Content.ReadAsStreamAsync())
            //        using (var sr = new StreamReader(stream))
            //        {
            //            string responseText = sr.ReadToEnd();
            //            LogResponse(responseText);
            //        }

            //        response.EnsureSuccessStatusCode();
            //        FileUploadFinished?.Invoke(this, new FileUploadFinishedEventArgs(fileName));
            //        MessageBox.Show("Fisierul " + fileName + " a fost descarcat", GlobalContext.ApplicationTitle);
            //    }
            //}
            //catch (Exception ex)
            //{
            //    GlobalContext.Log($"Exception uploading the file to {uploadURL}: {ex.Message}");
            //    MessageBox.Show($"Exception uploading the file to {uploadURL}: {ex.Message}", GlobalContext.ApplicationTitle);
            //}
        }


        private static void LogResponse(string responseText)
        {
            if (GlobalContext.SerializedConfiguration.Debug)
            {
                try
                {
                    // var responseText = response.Content;

                    var r = Newtonsoft.Json.JsonConvert.DeserializeObject(responseText);

                    string identedResponse = Newtonsoft.Json.JsonConvert.SerializeObject(r, Formatting.Indented);

                    GlobalContext.Log("Server response: {0} {1}", System.Environment.NewLine, identedResponse);


                    string debugDirectory = Path.Combine(MainForm.GetFileStoragePath(), "debug");

                    if (!Directory.Exists(debugDirectory))
                        Directory.CreateDirectory(debugDirectory);

                    {
                        string debugFilePathTime = Path.Combine(debugDirectory, $"response_{DateTime.Now.ToString("yyyyMMdd_hhmmss")}.json");

                        File.WriteAllText(debugFilePathTime, identedResponse);
                    }

                    {
                        string debugFilePath = Path.Combine(debugDirectory, $"last_response.json");

                        File.WriteAllText(debugFilePath, identedResponse);
                    }
                }
                catch (Exception ex)
                {
                    GlobalContext.Log("Exception = " + ex.Message);
                }
            }
        }

        private void Browser_TitleChanged(object sender, TitleChangedEventArgs e)
        {
            System.Action sa = (System.Action)(() =>
            {
                ((TabPage)this.Parent).Text = e.Title.Length >= 28 ? e.Title.Substring(0, 28) + "…" : e.Title;
            });

            if (this.InvokeRequired)
                this.Invoke(sa);
            else
                sa();
        }




        private async void Browser_FrameLoadEnd(object sender, FrameLoadEndEventArgs e)
        {
            try
            {
                GlobalContext.Log("Browser_FrameLoadEnd url={0} frame={1}", e.Url, e.Frame.Name);

                if (e.Frame.IsMain)
                {

                    // https://www.amazon.com/ap/signin?openid.return_to=https://relay.amazon.com/&openid.identity=http://specs.openid.net/auth/2.0/identifier_select&openid.assoc_handle=amzn_relay_desktop_us&openid.mode=checkid_setup&openid.claimed_id=http://specs.openid.net/auth/2.0/identifier_select&openid.ns=http://specs.openid.net/auth/2.0&pageId=amzn_relay_desktop_us
                    if ((e.Url.IndexOf("amazon.com/ap/signin") >= 0) ||
                        (e.Url.IndexOf("amazon.co.uk/ap/signin") >= 0))
                    {
                        string email = GlobalContext.ApiConfig.relayAuth.username;
                        string pass = GlobalContext.ApiConfig.relayAuth.password;

                        string jsSource1 = string.Format(
                            "(function () {{ document.getElementById('ap_email').value = '{0}'; document.getElementById('ap_password').value = '{1}'; }} )(); ",
                            email,
                            pass
                        );


                        JavascriptResponse response = await browser.GetMainFrame().EvaluateScriptAsync(jsSource1);
                    }



                    this.Invoke((MethodInvoker)delegate
                    {
                        urlTextBox.Text = e.Url;
                        // urlTextBox.Text = 
                        if (((TabPage)this.Parent).Text == "_____________")
                            ((TabPage)this.Parent).Text = MainForm.GetUrlTabPageName(e.Url);
                    });
                }
            }
            catch (Exception ex)
            {
                GlobalContext.Log("Exception = " + ex.Message); //--
            }
        }


        [DllImport("user32.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall)]
        public static extern void mouse_event(uint dwFlags, uint dx, uint dy, uint cButtons, uint dwExtraInfo);
        // Mouse actions
        private const int MOUSEEVENTF_LEFTDOWN = 0x02;
        private const int MOUSEEVENTF_LEFTUP = 0x04;
        private const int MOUSEEVENTF_RIGHTDOWN = 0x08;
        private const int MOUSEEVENTF_RIGHTUP = 0x10;

        public string Url { get => url; /*set => url = value;*/ }

        public void DoMouseClick(uint X, uint Y)
        {
            //Call the imported function with the cursor's current position
            // uint X = (uint)Cursor.Position.X;
            // uint Y = (uint)Cursor.Position.Y;

            mouse_event(MOUSEEVENTF_LEFTDOWN | MOUSEEVENTF_LEFTUP, X, Y, 0, 0);
        }

        private void UserControl1_Load(object sender, EventArgs e)
        {
            //. InitBrowser();
        }

        protected override void OnLoad(EventArgs e)
        {
            // InitBrowser();

            // Call the base class OnLoad to ensure any delegate event handlers are still callled
            base.OnLoad(e);
        }

        private void showDevToolsButton_Click(object sender, EventArgs e)
        {
            browser.ShowDevTools();
        }

        private void refrehPageButton_Click(object sender, EventArgs e)
        {
            browser.Reload(true);
        }

        private void closeButton_Click(object sender, EventArgs e)
        {
            if (Close != null)
                Close(this, EventArgs.Empty);
        }

        private void increaseTextSizeButton_Click(object sender, EventArgs e)
        {
            browser.SetZoomLevel(browser.GetZoomLevelAsync().Result + 0.1);
        }

        private void decreaseTextSizeButton_Click(object sender, EventArgs e)
        {
            browser.SetZoomLevel(browser.GetZoomLevelAsync().Result - 0.1);
        }

        private void loadUrlButton_Click(object sender, EventArgs e)
        {
            browser.Load(urlTextBox.Text);
            InitAutoDownloadTimer(urlTextBox.Text);
        }

        private void goBackButton_Click(object sender, EventArgs e)
        {
            browser.Back();
        }

        private void urlTextBox_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                browser.Load(urlTextBox.Text);
                InitAutoDownloadTimer(urlTextBox.Text);
            }
        }

        public class FileUploadFinishedEventArgs : EventArgs
        {
            public FileUploadFinishedEventArgs(string fileName)
            {
                FileName = fileName;
            }

            public string FileName { get; set; }
        }

        public class UpdateAutoDownloadIntervalStatusEventArgs : EventArgs
        {
            public UpdateAutoDownloadIntervalStatusEventArgs(string text)
            {
                Text = text;
            }

            public string Text { get; set; }
        }

        private void goForwardButton_Click(object sender, EventArgs e)
        {
            browser.Forward();
        }



        async void ClickExportTripsFile()
        {
            try
            {
                GlobalContext.Log("Clicking on the export button... [BrowserUserController]");

                JavascriptResponse response = await browser.GetMainFrame().EvaluateScriptAsync(GlobalContext.Scripts["clickExportTripsButton"]);
                GlobalContext.Log($"ClickExportTripsFile response: ", response);

                if (response == null)
                    throw new NullReferenceException("response == null");

                if (response.Result == null)
                    throw new NullReferenceException("response.Result == null");
                // return;

                bool jsScriptResult = (bool)response.Result;

                if (!jsScriptResult)
                    GlobalContext.Log($"Failed to click on the export button! 2");
            }
            catch (Exception ex)
            {
                GlobalContext.Log("Failed to click on the export button: {0}", ex.Message);
            }
        }

        private void downloadTripsButton_Click(object sender, EventArgs e)
        {
            ClickExportTripsFile();
        }
    }
}
