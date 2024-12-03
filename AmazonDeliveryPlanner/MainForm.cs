using AmazonDeliveryPlanner.API;
using AmazonDeliveryPlanner.API.data;
using CefSharp;
using CefSharp.WinForms;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.Caching;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using System.Windows.Forms;
using File = System.IO.File;
using ListBox = System.Windows.Forms.ListBox;
// using System.Runtime.InteropServices;

namespace AmazonDeliveryPlanner
{
    public partial class MainForm : Form
    {
        ChromiumWebBrowser adminBrowser;
        ChromiumWebBrowser driversPanelBrowser;
        static string configurationFilePath;

        InitAppApi _initApp;

        public MainForm()
        {
            InitializeComponent();

            GlobalContext.ApplicationTitle = "AMZ Relay Import - v.1-28.10.2024";
            GlobalContext.MainWindow = this;
            this.Text = GlobalContext.ApplicationTitle;

            try
            {
                Init();
                if (!OpenPlannerSelectorForm())
                {
                    MessageBox.Show("Could not get planner list - application will now exit!", GlobalContext.ApplicationTitle);
                    System.Diagnostics.Process.GetCurrentProcess().Kill();
                    Application.Exit();
                    return;
                }
                else
                {
                    // set planner name
                    plannerLabel.Text = "\uA19C" + " " + GlobalContext.LoggedInPlanner.ToString(); // U+1F464 ??  U+A19C ?
                    string[] roles = GlobalContext.LoggedInPlanner.roles;
                    if (roles.Contains("pm") || roles.Contains("admin"))
                    {
                        exportFileAutoDownloadEnabledCheckBox.Checked = true;
                        exportFileAutoDownloadEnabledCheckBox.Show();
                    }
                    else
                    {
                        exportFileAutoDownloadEnabledCheckBox.Checked = false;
                        exportFileAutoDownloadEnabledCheckBox.Hide();
                    }
                }
                InitMainFormDataControls();
                InitDriversPanelBrowser();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Exception on init: " + System.Environment.NewLine +
                    ex.Message + System.Environment.NewLine +
                    ex.StackTrace + System.Environment.NewLine +
                    ex.TargetSite + System.Environment.NewLine +
                    ex.Source + System.Environment.NewLine
                    );
            }

            driversPanel.Visible = false;
        }

        public static void InitializeCEF()
        {
            GlobalContext.Log("Director aplicatie: {0}", Utilities.GetApplicationPath());

            if (Cef.IsInitialized)
            {
                GlobalContext.Log("CEF a fost deja initializat");
                return;
            }

            if (GlobalContext.GlobalCefSettings != null)
            {
                GlobalContext.Log("exista deja o instanta CEFSettings");
                return;
            }

            string cachePath = Path.Combine(Utilities.GetApplicationPath(), "cachedirs\\c1");

            cachePath = "";


            CefSettings cfsettings = new CefSettings();

            cfsettings.LogSeverity = LogSeverity.Disable;
            cfsettings.Locale = "en-US";


            Cef.Initialize(cfsettings, performDependencyCheck: true, browserProcessHandler: null);


            GlobalContext.Log("Proces CEF initializat", cachePath);

        }

        void Init()
        {
            try
            {
                if (!Directory.Exists(GetFileStoragePath()))
                    Directory.CreateDirectory(GetFileStoragePath());
            }
            catch (Exception)
            {
            }

            //settingsFilePath = Utilities.GetApplicationPath() + Path.DirectorySeparatorChar + GlobalContext.OptionsFile;
            //settingsFilePath = Utilities.GetUserApplicationPath() + Path.DirectorySeparatorChar + GlobalContext.OptionsFile;
            configurationFilePath = GetFileStoragePath() + Path.DirectorySeparatorChar + GlobalContext.ConfigurationFileName;

            if (File.Exists(configurationFilePath))
                LoadConfiguration();
            // GlobalContext.SerializedConfiguration = (SerializedConfiguration)Utilities.LoadXML(configurationFilePath, typeof(SerializedConfiguration));
            else
            {
                // SerializedConfiguration conf = SerializedConfiguration.GetDefaultOptions();
                SerializedConfiguration conf = new SerializedConfiguration();
                GlobalContext.SerializedConfiguration = conf;
                SaveConfiguration();
                // Utilities.SaveXML(configurationFilePath, GlobalContext.SerializedConfiguration);
            }

            try
            {
                if (!Directory.Exists(GlobalContext.SerializedConfiguration.DownloadDirectoryPath))
                    Directory.CreateDirectory(GlobalContext.SerializedConfiguration.DownloadDirectoryPath);
            }
            catch (Exception)
            {
            }

            // using System.Net;
            ServicePointManager.Expect100Continue = true;
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
            // Use SecurityProtocolType.Ssl3 if needed for compatibility reasons
            // otherwise we get {"The request was aborted: Could not create SSL/TLS secure channel."}

            LoadAmazonTabs();
        }

        void InitMainFormDataControls()
        {
            UpdateDriverList();
            LoadScripts();
            InitializeCEF();
            if (!GlobalContext.SerializedConfiguration.Debug)
                mainTabControl.TabPages.Remove(loggingTabPage);
        }

        async void LoadAmazonTabs(bool reloadConfiguration = false)
        {
            await Task.Delay(4000);

            List<TripPageConfiguration> tpcs = GlobalContext.ApiConfig.tripPages;

            TripPageConfiguration upcomingTPC = null;
            TripPageConfiguration intransitTPC = null;
            TripPageConfiguration historyTPC = null;

            if (tpcs != null && tpcs.Count > 0)
            {
                upcomingTPC = tpcs[0];
                if (tpcs.Count > 1)
                    intransitTPC = tpcs[1];

                if (tpcs.Count > 2)
                    historyTPC = tpcs[2];
            }

            if (upcomingTPC != null)
            {
                upcomingTabBrowserTimerExportUserControl.MinRandomIntervalMinutes = upcomingTPC.MinRandomIntervalMinutes;
                upcomingTabBrowserTimerExportUserControl.MaxRandomIntervalMinutes = upcomingTPC.MaxRandomIntervalMinutes;
                upcomingTabBrowserTimerExportUserControl.ExportFileAutoDownloadEnabled = exportFileAutoDownloadEnabledCheckBox.Checked;
                upcomingTabBrowserTimerExportUserControl.DloadDone += DloadDoneHandler;
                upcomingTabBrowserTimerExportUserControl.ResetTimers();
                upcomingTabBrowserTimerExportUserControl.GoToURL(upcomingTPC.Url);
            }

            if (intransitTPC != null)
            {
                intransitTabBrowserTimerExportUserControl.MinRandomIntervalMinutes = intransitTPC.MinRandomIntervalMinutes;
                intransitTabBrowserTimerExportUserControl.MaxRandomIntervalMinutes = intransitTPC.MaxRandomIntervalMinutes;
                intransitTabBrowserTimerExportUserControl.ExportFileAutoDownloadEnabled = exportFileAutoDownloadEnabledCheckBox.Checked;
                intransitTabBrowserTimerExportUserControl.DloadDone += DloadDoneHandler;
                intransitTabBrowserTimerExportUserControl.ResetTimers();
                intransitTabBrowserTimerExportUserControl.GoToURL(intransitTPC.Url);
            }

            if (historyTPC != null)
            {
                historyTabBrowserTimerExportUserControl.MinRandomIntervalMinutes = historyTPC.MinRandomIntervalMinutes;
                historyTabBrowserTimerExportUserControl.MaxRandomIntervalMinutes = historyTPC.MaxRandomIntervalMinutes;
                historyTabBrowserTimerExportUserControl.ExportFileAutoDownloadEnabled = exportFileAutoDownloadEnabledCheckBox.Checked;
                historyTabBrowserTimerExportUserControl.DloadDone += DloadDoneHandler;
                historyTabBrowserTimerExportUserControl.ResetTimers();
                historyTabBrowserTimerExportUserControl.GoToURL(historyTPC.Url);
            }
        }

        #region logging

        int logCounter = 0;

        // Constants for extern calls to various scrollbar functions
        private const int SB_HORZ = 0x0;
        private const int SB_VERT = 0x1;
        private const int WM_HSCROLL = 0x114;
        private const int WM_VSCROLL = 0x115;
        private const int SB_THUMBPOSITION = 4;
        private const int SB_BOTTOM = 7;
        private const int SB_OFFSET = 13;

        [DllImport("user32.dll")]
        static extern int SetScrollPos(IntPtr hWnd, int nBar, int nPos, bool bRedraw);
        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        private static extern int GetScrollPos(IntPtr hWnd, int nBar);
        [DllImport("user32.dll")]
        private static extern bool PostMessageA(IntPtr hWnd, int nBar, int wParam, int lParam);
        [DllImport("user32.dll")]
        static extern bool GetScrollRange(IntPtr hWnd, int nBar, out int lpMinPos, out int lpMaxPos);


        private delegate void SetTextCallback(string text);

        public void Output(string text, params object[] args)
        {
            string msg = string.Format(text, args);
            Output(msg);
        }

        public void Output(string text)
        {
            if (this.logTextBox == null)
                return;

            if (this.logTextBox.InvokeRequired)
            {
                SetTextCallback stc = new SetTextCallback(Output);

                try
                {
                    if (!this.IsDisposed && this.IsHandleCreated)
                        this.Invoke(stc, new object[] { text });
                }
                catch (ObjectDisposedException)
                {
                }
            }
            else
            {
                if (GlobalContext.SerializedConfiguration.Debug)
                {
                    try
                    {
                        //if (text == "x")
                        //{
                        //    UpdateProgress();
                        //    return;
                        //}

                        if (autoScrollCheckBox.Checked)
                        {
                            bool bottomFlag = false;
                            int VSmin;
                            int VSmax;
                            int sbOffset;
                            int savedVpos;

                            // Win32 magic to keep the textbox scrolling to the newest append to the textbox unless
                            // the user has moved the scrollbox up
                            sbOffset = (int)((this.logTextBox.ClientSize.Height - SystemInformation.HorizontalScrollBarHeight) / (this.logTextBox.Font.Height));
                            savedVpos = GetScrollPos(this.logTextBox.Handle, SB_VERT);
                            GetScrollRange(this.logTextBox.Handle, SB_VERT, out VSmin, out VSmax);
                            if (savedVpos >= (VSmax - sbOffset - 1))
                                bottomFlag = true;

                            this.logTextBox.AppendText("[" + DateTime.Now.ToString("dd HH:mm:ss.fff") + "] " + text + "\r\n");

                            if (bottomFlag)
                            {
                                GetScrollRange(this.logTextBox.Handle, SB_VERT, out VSmin, out VSmax);
                                savedVpos = VSmax - sbOffset;
                                bottomFlag = false;
                            }

                            SetScrollPos(this.logTextBox.Handle, SB_VERT, savedVpos, true);
                            PostMessageA(this.logTextBox.Handle, WM_VSCROLL, SB_THUMBPOSITION + 0x10000 * savedVpos, 0);
                        }
                        else
                            logTextBox.AppendText("[" + DateTime.Now.ToString("dd HH:mm:ss.fff") + "] " + text + "\r\n");


                        //if (autoScrollCheckBox.Checked)
                        //{
                        //    logTextBox.Select()
                        //    logTextBox.SelectionStart = logTextBox.Text.Length;
                        //    logTextBox.ScrollToCaret();
                        //}

                        if (logCounter++ > 9000)
                        {
                            logTextBox.Clear();
                            logCounter = 0;
                        }
                    }
                    catch (ObjectDisposedException //ex
                )
                    {

                    }
                }
            }

        }

        #endregion

        int sessionCount = 0;

        Dictionary<long, bool> openTabDrivers = new Dictionary<long, bool>();

        private MemoryCache cache = MemoryCache.Default;

        public void AddIdToList(int idToAdd)
        {
            DbLite db = new DbLite();
            db.AddIdToList(idToAdd);
        }

        public List<int> GetListOfIds()
        {
            DbLite db = new DbLite();
            return db.GetListOfIds();
        }

        void AddSessionTab()
        {
            if (selectedDriver == null)
            {
                MessageBox.Show("No driver selected", GlobalContext.ApplicationTitle, MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            AddIdToList((int)selectedDriver.driver_id);


            foreach (TabPage page in tabControl.TabPages)
            {
                if (((DriverSessionObject)page.Tag).DriverId == selectedDriver.driver_id)
                {
                    tabControl.SelectedTab = page;
                    return;
                }
            }

            sessionCount++;

            TabPage stp = new TabPage();

            tabControl.SuspendLayout();

            stp.SuspendLayout();

            tabControl.TabPages.Add(stp);


            stp.Name = "PageSesiune" + sessionCount;
            stp.Text = selectedDriver.ToString();

            DriverSessionObject driverSessionObject = new DriverSessionObject()
            {
                DriverId = selectedDriver.driver_id
            };

            stp.Tag = driverSessionObject; // the object changes on resfreshing data from server as new objects are created for the same entity


            DriverUserControl driverUC = new DriverUserControl(selectedDriver);

            driverUC.SuspendLayout();


            driverUC.Anchor = AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Top;
            driverUC.Location = new System.Drawing.Point(3, 0);
            // driverUC.Width = stp.Width - 10; //?
            driverUC.Dock = DockStyle.Fill;
            driverUC.Name = "TCSesiune_DriverUC_" + sessionCount;

            // driverUC.Tag = selectedDriver;
            driverUC.SessionClosed += DriverUC_SessionClosed;
            driverUC.OpenURL += DriverUC_OpenURL;

            driverUC.ResumeLayout();

            stp.Controls.Add(driverUC);

            driverSessionObject.DriverUC = driverUC;


            TabControl urlsTabControl = null;

            if (uniqueSharedUrlsTabControl == null)
            {
                urlsTabControl = new System.Windows.Forms.TabControl();



                urlsTabControl.Anchor = AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Bottom | AnchorStyles.Top;
                urlsTabControl.Location = new System.Drawing.Point(3, driverUC.Height + 0 + 5);

                urlsTabControl.Name = "TCSesiune" + sessionCount;

                urlsTabControl.Dock = DockStyle.Fill;



                RequestContextSettings requestContextSettings = new RequestContextSettings();

                requestContextSettings.PersistSessionCookies = !false;
                requestContextSettings.PersistUserPreferences = !false;

                string cachePath = Path.Combine(Utilities.GetApplicationPath(), "cachedirs", "TCSesiune_" + selectedDriver.driver_id); // "TCSesiune" + sessionCount

                if (!Directory.Exists(cachePath))
                    Directory.CreateDirectory(cachePath);

                requestContextSettings.CachePath = cachePath;

                // if (false)
                foreach (string url in GlobalContext.SerializedConfiguration.DefaultTabs /*GlobalContext.Urls*/)
                {
                    TabPage urlTabPage = new System.Windows.Forms.TabPage();


                    urlsTabControl.Controls.Add(urlTabPage);

                    urlTabPage.Location = new System.Drawing.Point(4, 24 /*+ driverUC.Height*/);
                    // urlTabPage.Name = "tabPage1";
                    urlTabPage.Padding = new System.Windows.Forms.Padding(3);
                    urlTabPage.Size = new System.Drawing.Size(1071, 659 /*- driverUC.Height*/);
                    urlTabPage.TabIndex = 0;
                    urlTabPage.Text = GetUrlTabPageName(url);
                    urlTabPage.UseVisualStyleBackColor = true;

                    // urlTabPage.BackColor = Color.Green;

                    driverSessionObject.ReqContextSettings = requestContextSettings;

                    BrowserUserControl bUC = new BrowserUserControl(url, requestContextSettings, selectedDriver.driver_id);

                    {
                        // mfbUC.Cif = cif;

                        bUC.SuspendLayout();

                        urlTabPage.Controls.Add(bUC);

                        urlTabPage.Tag = bUC;

                        bUC.Dock = System.Windows.Forms.DockStyle.Fill;
                        bUC.Location = new System.Drawing.Point(3, 0);
                        bUC.TabIndex = 1;
                        bUC.Close += BUC_Close;

                        bUC.ResumeLayout(!false);

                        bUC.PerformLayout();

                        bUC.Tag = driverUC;
                        bUC.FileUploadFinished += BUC_FileUploadFinished;
                        bUC.UpdateAutoDownloadStatus += BUC_UpdateAutoDownloadStatus;
                    }

                    urlTabPage.ResumeLayout();
                }

                urlsTabControl.SelectedIndex = 0;

                urlsTabControl.SuspendLayout();

                urlsTabControl.ResumeLayout(false);

                uniqueSharedUrlsTabControl = urlsTabControl;
            }
            else
            {
                driverUC.SplitContainer.Panel1.Controls.Add(urlsTabControl);
            }

            // stp.Controls.Add(urlsTabControl);
            driverUC.SuspendLayout();
            driverUC.SplitContainer.SuspendLayout();
            driverUC.SplitContainer.Panel1.Controls.Add(urlsTabControl);
            driverUC.UrlsTabControl = urlsTabControl;
            driverUC.SplitContainer.ResumeLayout();
            driverUC.ResumeLayout();

            stp.Controls.Add(driverUC);


            stp.ResumeLayout();


            tabControl.ResumeLayout();

            tabControl.SelectTab(stp);

            tabControl.Refresh();
            tabControl.Invalidate();

            tabControl.PerformLayout();


            openTabDrivers[selectedDriver.driver_id] = true;
        }

        private void BUC_UpdateAutoDownloadStatus(object sender, BrowserUserControl.UpdateAutoDownloadIntervalStatusEventArgs e)
        {
            System.Action sa = (System.Action)(() =>
            {
                ((sender as BrowserUserControl).Tag as DriverUserControl).UpdateAutoDownloadLabel(e.Text);
            });

            if (this.InvokeRequired)
                this.Invoke(sa);
            else
                sa();
        }

        private void BUC_FileUploadFinished(object sender, BrowserUserControl.FileUploadFinishedEventArgs e)
        {
            System.Action sa = (System.Action)(() =>
            {
                ((sender as BrowserUserControl).Tag as DriverUserControl).UpdateUploadLabel("uploaded file " + e.FileName);
            });

            if (this.InvokeRequired)
                this.Invoke(sa);
            else
                sa();
        }

        private void DriverUC_OpenURL(object sender, OpenURLEventArgs e)
        {


            {

                TabControl urlsTabControl = uniqueSharedUrlsTabControl;
                TabPage page = (sender as Control).Parent as TabPage;

                #region GMaps open test
                // bool isGMapsOpen = false;

                foreach (TabPage tp in urlsTabControl.TabPages) // 
                    if ((tp.Tag as BrowserUserControl).Url.Contains("google.com/maps"))
                    {
                        // isGMapsOpen = true;

                        if (e.URL.Contains("google.com/maps"))
                        {
                            urlsTabControl.SelectedTab = tp;
                            return; // if there's one tab page already open with the google maps location, don't open a second one
                        }
                    }

                //if (isGMapsOpen && e.URL.Contains("google.com/maps"))
                //    return; // if there's one tab page already open with the google maps location, don't open a second one
                #endregion

                TabPage urlTabPage = new System.Windows.Forms.TabPage();

                // 
                // tabControl
                // 
                urlsTabControl.Controls.Add(urlTabPage);

                // tabPage1
                // 
                urlTabPage.Location = new System.Drawing.Point(4, 24 /*+ driverUC.Height*/);
                // urlTabPage.Name = "tabPage1";
                urlTabPage.Padding = new System.Windows.Forms.Padding(3);
                urlTabPage.Size = new System.Drawing.Size(1071, 659 /*- driverUC.Height*/);
                urlTabPage.TabIndex = 0;
                urlTabPage.Text = GetUrlTabPageName(e.URL);
                urlTabPage.UseVisualStyleBackColor = true;

                // urlTabPage.BackColor = Color.Green;

                BrowserUserControl bUC = new BrowserUserControl(e.URL, ((DriverSessionObject)page.Tag).ReqContextSettings, ((DriverSessionObject)page.Tag).DriverId);

                {
                    // mfbUC.Cif = cif;

                    bUC.SuspendLayout();

                    urlTabPage.Controls.Add(bUC);

                    urlTabPage.Tag = bUC;

                    // bUC.OnFinishedQuery += MFbUC_OnFinishedQuery;

                    bUC.Dock = System.Windows.Forms.DockStyle.Fill;
                    bUC.Location = new System.Drawing.Point(3, 0);
                    // bpipbUC.Name = "x";
                    // this.tabControl1.Size = new System.Drawing.Size(852, 586);
                    bUC.TabIndex = 1;
                    bUC.Close += BUC_Close;

                    bUC.ResumeLayout(!false);

                    bUC.PerformLayout();
                }

                urlTabPage.ResumeLayout();

                urlsTabControl.SelectedTab = urlTabPage;
            }

        }

        private void BUC_Close(object sender, EventArgs e)
        {

            TabControl tcp = (sender as BrowserUserControl).Parent.Parent as TabControl;

            foreach (TabPage page in tabControl.TabPages)
            {
                TabControl tc = ((DriverSessionObject)page.Tag).DriverUC.UrlsTabControl;

                {
                    TabControl urlsTabControl = tc;

                    if (urlsTabControl != null)
                        foreach (TabPage tp in urlsTabControl.TabPages) // 
                            if ((tp.Tag as BrowserUserControl) == sender)
                                urlsTabControl.TabPages.Remove(tp);
                }


            }
        }
        private void DriverUC_SessionClosed(object sender, EventArgs e)
        {
            DbLite db = new DbLite();

            if (tabControl.TabPages.IndexOf((sender as DriverUserControl).Parent as TabPage) == 0)
                if (tabControl.TabPages.Count > 1)
                {
                    ((DriverSessionObject)(tabControl.TabPages[1]).Tag).DriverUC.SplitContainer.Panel1.Controls.Add(uniqueSharedUrlsTabControl);
                }


            foreach (TabPage page in tabControl.TabPages)
            {
                if (page.Controls.Contains((Control)sender))
                {
                    tabControl.TabPages.Remove(page);


                    Driver drv = GlobalContext.LastDriverList.drivers.Where(dr => dr.driver_id == ((DriverSessionObject)page.Tag).DriverId).SingleOrDefault();

                    openTabDrivers[drv.driver_id] = false;

                    db.DeleteIdFromList((int)drv.driver_id);

                    driverListBox.Refresh();
                }
            }

            if (tabControl.TabPages.Count == 0)
                uniqueSharedUrlsTabControl = null;
        }

        private void openSettingsButton_Click(object sender, EventArgs e)
        {
            SettingsForm sForm = new SettingsForm();

            if (sForm.ShowDialog() == DialogResult.OK)
            {
                SaveConfiguration();
                // ...
            }
        }

        private void addSessionButton_Click(object sender, EventArgs e)
        {
            AddSessionTab();
        }

        private void MainForm_Load(object sender, EventArgs e)
        {

        }

        DriverList _driverList;

        void UpdateDriverList()
        {
            int tryCount = 0;
            Exception lastException = null;

            do
            {
                try
                {
                    tryCount++;

                    DriverList driverList = DriversAPI.GetDrivers();

                    GlobalContext.LastDriverList = driverList;

                    _driverList = driverList;

                    UpdateDriverListControl();

                    return;
                }
                catch (Exception ex)
                {
                    GlobalContext.Log("Exception getting drivers from web service: '{0}'", ex.Message);
                    lastException = ex;
                }
            } while (tryCount < 4);

            if (lastException != null)
                MessageBox.Show("Exception loading drivers: " + lastException.Message);
        }

        void UpdateDriverListControl()
        {
            driverListBox.Items.Clear();

            driverListBox.Items.AddRange(
                GlobalContext.LastDriverList.drivers.Where(dr =>
                {
                    if (activeDriversRadioButton.Checked)
                        return dr.is_user_active;
                    else
                        return true;
                }).ToArray()
            );

            // openTabDrivers.Clear();

            foreach (Driver dr in GlobalContext.LastDriverList.drivers)
                if (!openTabDrivers.ContainsKey(dr.driver_id))
                    openTabDrivers.Add(dr.driver_id, false);
        }

        private void refreshDriversButton_Click(object sender, EventArgs e)
        {
            UpdateDriverList();
        }

        Driver selectedDriver;

        private void driverListBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            selectedDriver = GlobalContext.LastDriverList.drivers.Where(dr => dr.ToString() == driverListBox.SelectedItem.ToString()).SingleOrDefault();
        }

        private void driverListBox_DoubleClick(object sender, EventArgs e)
        {
            AddSessionTab();
        }

        private void driversRadioButton_CheckedChanged(object sender, EventArgs e)
        {
            UpdateDriverList();
        }

        private void driverListBox_DrawItem(object sender, DrawItemEventArgs e)
        {
            if (e.Index < 0)
                return;

            e.DrawBackground();

            Graphics g = e.Graphics;

            if (openTabDrivers.ElementAt(e.Index).Value)
                g.FillRectangle(new SolidBrush(Color.LightBlue), e.Bounds);
            else
                g.FillRectangle(new SolidBrush(Color.White), e.Bounds);

            ListBox lb = (ListBox)sender;
            g.DrawString(lb.Items[e.Index].ToString(), e.Font, new SolidBrush(Color.Black), new PointF(e.Bounds.X, e.Bounds.Y));

            e.DrawFocusRectangle();
        }

        private void MainForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.Exit();
            System.Diagnostics.Process.GetCurrentProcess().Kill();
        }

        public static string GetUrlTabPageName(string url)
        {
            if (url.Contains("google.com/maps"))
                return "Google Maps";
            else
            if (url.Contains("relay.amazon.com"))
                return "Amazon Relay";
            else
                return string.IsNullOrEmpty(url) ? "_____________" : (url.Length >= 20) ? url.Substring(0, 20) : url;
        }

        private void Browser_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            if ((e.Control || e.KeyCode == Keys.ControlKey) && (e.KeyCode == Keys.F))
            {
                OpenSearchDriverForm();
            }
        }

        private void MainForm_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Control && (e.KeyCode == Keys.NumPad1 || e.KeyCode == Keys.D1))
                mainTabControl.SelectedTab = sessionsTabPage;

            if ((e.Control || e.KeyCode == Keys.ControlKey) && (e.KeyCode == Keys.F))
                OpenSearchDriverForm();
        }

        void SaveConfiguration()
        {
            string confJsonString = Newtonsoft.Json.JsonConvert.SerializeObject(GlobalContext.SerializedConfiguration, Formatting.Indented);

            File.WriteAllText(configurationFilePath, confJsonString);
        }

        void LoadConfiguration()
        {
            string confJsonString = File.ReadAllText(configurationFilePath);

            GlobalContext.SerializedConfiguration = Newtonsoft.Json.JsonConvert.DeserializeObject<SerializedConfiguration>(confJsonString);

            if (GlobalContext.SerializedConfiguration.DefaultTabs == null)
                GlobalContext.SerializedConfiguration.DefaultTabs = new string[0];
        }

        // This is the directory in which  the settings, output files and measurements directory are placed
        // Normally it would be the %APPDATA% - User Application Path directory but for portability reasons (to be easily moved between computers keeping settings), the application path can be used
        public static string GetFileStoragePath()
        {
            return Utilities.GetApplicationPath();
        }

        private void toggleLeftPanelVisibilityButton_Click(object sender, EventArgs e)
        {
            ToggleLeftPanelVisibility();
        }

        void ToggleLeftPanelVisibility()
        {
            splitContainer1.Panel1Collapsed = !splitContainer1.Panel1Collapsed;
            toggleLeftPanelVisibilityButton.Text = splitContainer1.Panel1Collapsed ? "\u220E \u220E" : "| \u220E";
        }

        void InitDriversPanelBrowser()
        {
            driversPanelBrowser = new ChromiumWebBrowser();
            RequestContextSettings requestContextSettings = new RequestContextSettings();
            requestContextSettings.PersistSessionCookies = !false;
            requestContextSettings.PersistUserPreferences = !false;
            if (requestContextSettings != null)
                driversPanelBrowser.RequestContext = new RequestContext(requestContextSettings);

            splitContainer1.Panel1.Controls.Add(driversPanelBrowser);
            driversPanelBrowser.Dock = DockStyle.Fill;

            driversPanelBrowser.FrameLoadEnd += DriversPanelBrowser_FrameLoadEnd;

            this.PerformLayout();
            this.Invalidate();
            this.Refresh();

            driversPanelBrowser.Dock = DockStyle.Fill;

            refreshDriverListBrowserButton.BringToFront();

            driversPanelBrowser.PreviewKeyDown += Browser_PreviewKeyDown;
            driversPanelBrowser.KeyboardHandler = new BrowserKeyboardHandler();

            var browserCallbackObjectForJs = new BrowserCallbackObjectForJs();

            CefSharpSettings.WcfEnabled = true;
            driversPanelBrowser.JavascriptObjectRepository.Settings.LegacyBindingEnabled = true;
            driversPanelBrowser.JavascriptObjectRepository.Register("driverCallbackObj", browserCallbackObjectForJs, isAsync: false, options: BindingOptions.DefaultBinder);


            List<int> listOfIds = GetListOfIds();

            foreach (int id in listOfIds)
            {
                GlobalContext.Log(" ---------------- ID: " + id);
                OpenDriverWindow(id.ToString(), true);
                GlobalContext.Log(" ---------------- ID: " + id);
            }

            //tabControl.SelectedTab = tabControl.TabPages[0];
        }

        private void DriversPanelBrowser_FrameLoadEnd(object sender, FrameLoadEndEventArgs e)
        {
            // throw new NotImplementedException();
        }

        private void refreshDriverListBrowserButton_Click(object sender, EventArgs e)
        {
            driversPanelBrowser.Reload(true);
        }

        bool displayOnlyDriverGroupNameInDriversForm = false;

        private void showOpenDriverFormButton_Click(object sender, EventArgs e)
        {
            OpenSearchDriverForm();
        }

        public void OpenSearchDriverForm()
        {

        }

        private void showDriversBrowserControlDevToolsButton_Click(object sender, EventArgs e)
        {
            driversPanelBrowser.ShowDevTools();
        }

        public void OpenDriverWindow(string driverHash, bool forceCall = false)
        {
            int _driverId = 0;
            string driverId = driverHash;
            string messageId = "";
            bool containsUnderscore = driverHash.Contains("_");
            if (containsUnderscore)
            {
                string[] substrings = driverHash.Split('_');
                // Split the string by underscore
                driverId = substrings[0];
                messageId = substrings[1];
            }

            try
            {

                _driverId = Convert.ToInt32(driverId);
            }
            catch (Exception ex)
            {
                Output(string.Format("The specified driverId value ('{1}') is not an integer number: '{0}'", ex.Message, driverId));
                MessageBox.Show(string.Format("The specified driverId value ('{1}') is not an integer number: '{0}'", ex.Message, driverId), GlobalContext.ApplicationTitle);
                return;
            }

            Driver clickedDriver = GlobalContext.LastDriverList.drivers.Where(dr => dr.driver_id == _driverId).FirstOrDefault();

            if (clickedDriver == null)
            {
                Output(string.Format("The specified driverId value ('{0}') could not be found.", driverId));
                MessageBox.Show(string.Format("The specified driverId value ('{0}') could not be found.", driverId), GlobalContext.ApplicationTitle);
                return;
            }

            clickedDriver.message_id = messageId;

            if (forceCall)
            {
                selectedDriver = clickedDriver;
                AddSessionTab();
            }
            else
            {

                if (this.InvokeRequired)
                    this.Invoke((MethodInvoker)delegate
                    {
                        selectedDriver = clickedDriver;
                        AddSessionTab();
                    });
            }
        }

        public bool OpenPlannerSelectorForm()
        {
            UpdateAppInit();

            if (_initApp == null || _initApp.planners == null || _initApp.planners.Length == 0)
                return false;

            PlannerSelectorForm plannerSelectorForm = new PlannerSelectorForm(_initApp.planners);

            plannerSelectorForm.DisplayOnlyPlannerGroupName = false;
            plannerSelectorForm.StartPosition = FormStartPosition.CenterScreen;

            if (plannerSelectorForm.ShowDialog() == DialogResult.OK && plannerSelectorForm.SelectedPlanner != null)
            {
                GlobalContext.LoggedInPlanner = plannerSelectorForm.SelectedPlanner;
                GlobalContext.Log($"Planner logged in: '{GlobalContext.LoggedInPlanner}'");

                return true;
            }

            return false;
        }


        void UpdateAppInit()
        {
            int tryCount = 0;
            Exception lastException = null;

            do
            {
                try
                {
                    tryCount++;
                    InitAppApi initAppConfig = InitAppApi.GetAppInit();
                    GlobalContext.ApiConfig = initAppConfig.configuration;
                    _initApp = initAppConfig;
                }
                catch (Exception ex)
                {
                    GlobalContext.Log("Exception getting planners(and configuration) from web service: '{0}'", ex.Message);
                    lastException = ex;
                }
            } while (tryCount < 4);

            if (lastException != null)
                MessageBox.Show("Exception loading planners(and configuration): " + lastException.Message);
        }

        private void changeUserButton_Click(object sender, EventArgs e)
        {
            if (!OpenPlannerSelectorForm())
            {
                MessageBox.Show("Could not get planner list!", GlobalContext.ApplicationTitle);
                return;
            }
            else
            {
                ClosePreviousPlannerTabs();
                // set planner name
                plannerLabel.Text = "\uA19C" + " " + GlobalContext.LoggedInPlanner.ToString(); // U+1F464 ??  U+A19C ?

                string[] roles = GlobalContext.LoggedInPlanner.roles;
                if (roles.Contains("pm") || roles.Contains("admin"))
                {
                    exportFileAutoDownloadEnabledCheckBox.Checked = true;
                    exportFileAutoDownloadEnabledCheckBox.Show();
                }
                else
                {
                    exportFileAutoDownloadEnabledCheckBox.Checked = false;
                    exportFileAutoDownloadEnabledCheckBox.Hide();
                }
            }
        }

        void ClosePreviousPlannerTabs()
        {
            foreach (TabPage page in tabControl.TabPages)
            {
                tabControl.TabPages.Remove(page);

                Driver drv = GlobalContext.LastDriverList.drivers.Where(dr => dr.driver_id == ((DriverSessionObject)page.Tag).DriverId).SingleOrDefault();

                openTabDrivers[drv.driver_id] = false;

            }
        }

        void LoadScripts()
        {


            GlobalContext.Scripts = new Dictionary<string, string>();

            string scriptsDirectory = Path.Combine(Utilities.GetApplicationPath(), @"cef\js\");

            string[] files = Directory.GetFiles(scriptsDirectory, "*.js");

            foreach (string filePath in files)
            {
                string fileName = Path.GetFileName(filePath);

                GlobalContext.Scripts.Add(fileName.Replace(".js", ""), File.ReadAllText(filePath));
            }
        }

        TabControl uniqueSharedUrlsTabControl;

        private void tabControl_Selecting(object sender, TabControlCancelEventArgs e)
        {

            if (e.TabPageIndex == -1)
                return;

            TabControl tc = uniqueSharedUrlsTabControl;
            ((DriverSessionObject)e.TabPage.Tag).DriverUC.SplitContainer.Panel1.Controls.Add(tc);

        }

        private void tabControl_TabIndexChanged(object sender, EventArgs e)
        {

        }

        private void exportFileAutoDownloadEnabledCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            LoadAmazonTabs();
        }

        private void reloadConfigurationButton_Click(object sender, EventArgs e)
        {
            this.Invoke((MethodInvoker)delegate
            {
                UpdateAppInit();
                LoadAmazonTabs(true);
            });
        }

        private void btn_dload_History_Click(object sender, EventArgs e)
        {
            btn_dload_History.Enabled = false;
            historyTabBrowserTimerExportUserControl.ClickExportTripsFile();
        }

        private void btn_dload_Transit_Click(object sender, EventArgs e)
        {
            btn_dload_Transit.Enabled = false;
            intransitTabBrowserTimerExportUserControl.ClickExportTripsFile();
        }

        private void btn_dload_Upcomming_Click(object sender, EventArgs e)
        {
            btn_dload_Upcomming.Enabled = false;
            upcomingTabBrowserTimerExportUserControl.ClickExportTripsFile();
        }


        private void DloadDoneHandler(object sender, string url)
        {
            if (url.Contains("/history")) btn_dload_History.Enabled = true;
            if (url.Contains("/in-transit")) btn_dload_Transit.Enabled = true;
            if (url.Contains("/upcoming")) btn_dload_Upcomming.Enabled = true;

        }
    }
}