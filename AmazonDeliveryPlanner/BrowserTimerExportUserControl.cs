// using CamioaneAmazon.CEF;
using CefSharp;
using CefSharp.WinForms;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Net.Http;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;

namespace AmazonDeliveryPlanner
{
    public partial class BrowserTimerExportUserControl : UserControl, IDisposable
    {
        ChromiumWebBrowser browser;

        string url;

        RequestContextSettings requestContextSettings;

        public event EventHandler Close;
        public event EventHandler<FileUploadFinishedEventArgs> FileUploadFinished;

        public event EventHandler<UpdateAutoDownloadIntervalStatusEventArgs> UpdateAutoDownloadStatus;



        //long driverId;

        int minRandomIntervalMinutes;
        int maxRandomIntervalMinutes;
        private System.Windows.Forms.Timer timer;

        int nextDownload = -1;
        int nextRefresh = -1;

        bool exportFileAutoDownloadEnabled;
        string pageType = "unknown";


        public EventHandler<string> DloadDone;

        public BrowserTimerExportUserControl() : this("", null)
        {
        }

        public BrowserTimerExportUserControl(string url, RequestContextSettings requestContextSettings)
        {
            // this.driverId = driverId;
            this.url = url;
            this.requestContextSettings = requestContextSettings;
            InitializeComponent();
            InitBrowser();
        }

        public void GoToURL(string url)
        {
            browser.Load(url);

            if (!string.IsNullOrWhiteSpace(url))
                SetPageType(url);
        }

        public void ResetTimers()
        {
            GlobalContext.Log("Restart timers for '{0}'", browser.Address);
            nextDownload = 0;
            nextRefresh = 0;
        }

        void InitBrowser()
        {


            browser = new ChromiumWebBrowser();

            if (requestContextSettings != null)
                browser.RequestContext = new RequestContext(requestContextSettings);


            browser.DownloadHandler = new DownloadHandler();

            ((DownloadHandler)browser.DownloadHandler).OnBeforeDownloadFired += BrowserTimerExportUserControl_OnBeforeDownloadFired;
            ((DownloadHandler)browser.DownloadHandler).OnDownloadUpdatedFired += BrowserUserControl_OnDownloadUpdatedFired;


            panel1.Controls.Add(browser);

            browser.Dock = DockStyle.Fill;

            browser.FrameLoadEnd += Browser_FrameLoadEnd;


            this.PerformLayout();
            this.Invalidate();
            this.Refresh();

            nextDownload = getRandomBetween(minRandomIntervalMinutes, maxRandomIntervalMinutes);
            nextRefresh = getRandomBetween(2, 5);

            browser.Load(url);

            browser.Dock = DockStyle.Fill;

            timer = new System.Windows.Forms.Timer();
            timer.Interval = 1000;
            timer.Tick += new EventHandler(Timer_Tick);

            if (!string.IsNullOrWhiteSpace(url))
                SetPageType(url);
        }


        private int getRandomBetween(int min, int max)
        {
            Random random = new Random();
            return random.Next(min * 60, max * 60);
        }




        private void Timer_Tick(object sender, EventArgs e)
        {


            Int32 now = (int)DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1)).TotalSeconds;

            if (now > nextDownload)
            {
                nextDownload = now + getRandomBetween(minRandomIntervalMinutes, maxRandomIntervalMinutes);
                this.Invoke((MethodInvoker)delegate
                {
                    ClickExportTripsFile();
                });
            }

             if (now > nextRefresh)
             {
                 nextRefresh = now + getRandomBetween(5, 7);
                 GlobalContext.Log("Reloaded page '{0}'", browser.Address);
                 this.Invoke((MethodInvoker)delegate
                {
                    browser.Reload();
                });
             }
        }

        void SetPageType(string loadedUrl)
        {
            if (loadedUrl.IndexOf("relay.amazon", StringComparison.OrdinalIgnoreCase) >= 0)
            {
                pageType = (loadedUrl.IndexOf("in-transit", StringComparison.OrdinalIgnoreCase) >= 0) ? "transit" : pageType;
                pageType = (loadedUrl.IndexOf("history", StringComparison.OrdinalIgnoreCase) >= 0) ? "history" : pageType;
                pageType = (loadedUrl.IndexOf("upcoming", StringComparison.OrdinalIgnoreCase) >= 0) ? "upcoming" : pageType;
            }
            else
            {
                pageType = "unknown";
                GlobalContext.Log("");
                GlobalContext.Log("    Warning - unknown page type");
            }
        }

        private void BrowserTimerExportUserControl_OnBeforeDownloadFired(object sender, DownloadItem e)
        {
            string fileSuffix = pageType + ".csv";
            e.FullPath = e.FullPath.Replace(".csv", fileSuffix);
            e.SuggestedFileName = fileSuffix;
        }


        private bool isDownloadInProgress = false;

        private async void BrowserUserControl_OnDownloadUpdatedFired(object sender, DownloadItem e)
        {
            GlobalContext.Log("Downloaded isDownloadInProgress: one '{0}'", isDownloadInProgress);

            if (isDownloadInProgress)
            {
                return; // Exit if a download is already in progress
            }

            isDownloadInProgress = true; // Set the flag

            try
            {
                if (!e.IsComplete) return;

                GlobalContext.Log("Downloaded isDownloadInProgress: two '{0}'", isDownloadInProgress);

                string uploadURL = "";

                try
                {
                    uploadURL = GlobalContext.SerializedConfiguration.AdminURL;
                    uploadURL += GlobalContext.SerializedConfiguration.APIBaseURL;
                    uploadURL += GlobalContext.SerializedConfiguration.FileUploadURL;
                    uploadURL += "/" + pageType;

                    GlobalContext.Log("Upload URL=\"{0}\"", uploadURL);

                    string fileName = e.SuggestedFileName;
                    if (string.IsNullOrEmpty(fileName))
                        fileName = Path.GetFileName(e.FullPath);

                    string csvFileContents = File.ReadAllText(e.FullPath);
                    csvFileContents = csvFileContents.Replace(",Operator ID,Spot Work", ",Operator ID,Spot Work,ColBC,COlBD");

                    byte[] bytes = Encoding.UTF8.GetBytes(csvFileContents);
                    HttpContent bytesContent = new ByteArrayContent(bytes);

                    using (var client = new HttpClient())
                    using (var formData = new MultipartFormDataContent())
                    {
                        formData.Add(bytesContent, "files", fileName);

                        var response = await client.PostAsync(uploadURL, formData);

                        if (!response.IsSuccessStatusCode)
                        {
                            // Handle non-successful response
                        }

                        using (var stream = await response.Content.ReadAsStreamAsync())
                        using (var sr = new StreamReader(stream))
                        {
                            string responseText = sr.ReadToEnd();
                            LogResponse(responseText);
                        }

                        response.EnsureSuccessStatusCode();
                        FileUploadFinished?.Invoke(this, new FileUploadFinishedEventArgs(fileName));
                    }
                }
                catch (Exception ex)
                {
                    GlobalContext.Log("Aici era nenorocotul de alert!");
                    GlobalContext.Log($"Exception uploading the file to {uploadURL}: {ex.Message}");
                    GlobalContext.Log($"Exception uploading the file to {uploadURL}: {ex.StackTrace}");
                    // MessageBox.Show($"Exception uploading the file to {uploadURL}: {ex.Message}", GlobalContext.ApplicationTitle);
                } finally
                {
                    isDownloadInProgress = false;
                }
                onUploadDone(browser);
            }
            finally
            {
                isDownloadInProgress = false; // Reset the flag
            }
        }

        private void onUploadDone(ChromiumWebBrowser browser)
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new Action(() =>
                {
                    DloadDone?.Invoke(this, browser.Address);
                }));
            }
            else
            {
                DloadDone?.Invoke(this, browser.Address);
            }
            //DloadDone(this, browser.Address);
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

        private async void Browser_FrameLoadEnd(object sender, FrameLoadEndEventArgs e)
        {
            try
            {
                GlobalContext.Log("Browser_FrameLoadEnd url={0} frame={1}", e.Url, e.Frame.Name);

                if (!e.Frame.IsMain) return;

                if (exportFileAutoDownloadEnabled)
                {
                    timer.Start();
                }

                // https://www.amazon.com/ap/signin?openid.return_to=https://relay.amazon.com/&openid.identity=http://specs.openid.net/auth/2.0/identifier_select&openid.assoc_handle=amzn_relay_desktop_us&openid.mode=checkid_setup&openid.claimed_id=http://specs.openid.net/auth/2.0/identifier_select&openid.ns=http://specs.openid.net/auth/2.0&pageId=amzn_relay_desktop_us
                if ((e.Url.IndexOf("amazon.com/ap/signin") >= 0) ||
                    (e.Url.IndexOf("amazon.co.uk/ap/signin") >= 0))
                {
                    string email = GlobalContext.SerializedConfiguration.RelayAuth.Username;
                    string pass = GlobalContext.SerializedConfiguration.RelayAuth.Password;

                    string jsSource1 = string.Format(
                        "(function () {{ document.getElementById('ap_email').value = '{0}'; document.getElementById('ap_password').value = '{1}'; }} )(); ",
                        email,
                        pass
                    );

                    JavascriptResponse response = await browser.GetMainFrame().EvaluateScriptAsync(jsSource1);
                }



            }
            catch (Exception ex)
            {
                GlobalContext.Log("Exception = " + ex.Message); //--
            }
        }


        [DllImport("user32.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall)]
        public static extern void mouse_event(uint dwFlags, uint dx, uint dy, uint cButtons, uint dwExtraInfo);

        public string Url { get => url; /*set => url = value;*/ }
        public int MinRandomIntervalMinutes { get => minRandomIntervalMinutes; set => minRandomIntervalMinutes = value; }
        public int MaxRandomIntervalMinutes { get => maxRandomIntervalMinutes; set => maxRandomIntervalMinutes = value; }
        public bool ExportFileAutoDownloadEnabled
        {
            get => exportFileAutoDownloadEnabled;
            set => SetExportFileAutoDownloadEnabled(value);
        }

        public void SetExportFileAutoDownloadEnabled(bool value)
        {
            exportFileAutoDownloadEnabled = value;
            if (value && browser.IsBrowserInitialized)
            {
                browser.Reload();
            }

            if (!value)
            {
                timer.Stop();
            }

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



        private void goBackButton_Click(object sender, EventArgs e)
        {
            browser.Back();
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



        public async void ClickExportTripsFile()
        {
            try
            {
                GlobalContext.Log("Clicking on the export button [BrowserTimmer]... {0}", browser.Address);

                JavascriptResponse response = await browser.GetMainFrame().EvaluateScriptAsync(GlobalContext.Scripts["clickExportTripsButton"]);

                if (response == null)
                    throw new NullReferenceException("response == null");

                if (response.Result == null)
                    throw new NullReferenceException("response.Result == null");
                // return;

                bool jsScriptResult = (bool)response.Result;

                //   if (!jsScriptResult)
                //       GlobalContext.Log($"Failed to click on the export button! 1");
            }
            catch (Exception ex)
            {
                GlobalContext.Log("Failed to click on the export button: {0}", ex.Message);
                onUploadDone(browser);
            }
        }

        private void downloadTripsButton_Click(object sender, EventArgs e)
        {
            ClickExportTripsFile();
        }

        private void BrowserTimerExportUserControl_Layout(object sender, LayoutEventArgs e)
        {

        }
    }
}
