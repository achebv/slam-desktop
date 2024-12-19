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
using static AmazonDeliveryPlanner.SerializedConfiguration;
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
            try
            {
                InitConfig();
                InitializeComponent();
                LoadAmazonTabs();
                GlobalContext.MainWindow = this;
                GlobalContext.ApplicationTitle = "Slam AMZ Import - 2024.12.11 - v1";
                this.Text = GlobalContext.ApplicationTitle;
                plannerLabel.Text = "Welcome"; 
                exportFileAutoDownloadEnabledCheckBox.Checked = true;
                exportFileAutoDownloadEnabledCheckBox.Show();
                InitMainFormDataControls();
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

        void InitConfig()
        {
            try
            {
                // Ensure File Storage Path exists
                EnsureDirectoryExists(GetFileStoragePath());

                // Initialize configuration file path
                configurationFilePath = Path.Combine(GetFileStoragePath(), GlobalContext.ConfigurationFileName);

                // Load existing configuration or initialize a new one
                if (File.Exists(configurationFilePath))
                {
                    LoadConfiguration();
                }
                else
                {
                    InitializeNewConfiguration();
                }

                // Ensure Download Directory exists
                EnsureDirectoryExists(GlobalContext.SerializedConfiguration.DownloadDirectoryPath);

                // Configure network security protocols
                ConfigureNetworkSecurity();
            }
            catch (Exception ex)
            {
                // Log error for debugging purposes
                LogException("Initialization failed", ex);
                throw; // Rethrow to allow caller to handle appropriately
            }
        }

        private void EnsureDirectoryExists(string path)
        {
            if (!string.IsNullOrWhiteSpace(path) && !Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
        }

        private void InitializeNewConfiguration()
        {
            GlobalContext.SerializedConfiguration = new SerializedConfiguration();
            SaveConfiguration();
        }

        private void ConfigureNetworkSecurity()
        {
            ServicePointManager.Expect100Continue = true;
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
        }

        private void LogException(string message, Exception ex)
        {
            // Placeholder for actual logging implementation
            Console.WriteLine($"{message}: {ex.Message}");
        }


        void InitMainFormDataControls()
        {
            LoadScripts();
            InitializeCEF();
            if (!GlobalContext.SerializedConfiguration.Debug)
                mainTabControl.TabPages.Remove(loggingTabPage);
        }

        async void LoadAmazonTabs(bool reloadConfiguration = false)
        {
            await Task.Delay(4000);

            AmzTab[] tpcs = GlobalContext.SerializedConfiguration.AmzTabs;

            AmzTab upcomingTPC = tpcs[0];
            AmzTab intransitTPC = tpcs[1];
            AmzTab historyTPC = tpcs[2];

            if (upcomingTPC != null)
            {
                upcomingTabBrowserTimerExportUserControl.MinRandomIntervalMinutes = upcomingTPC.MinMin;
                upcomingTabBrowserTimerExportUserControl.MaxRandomIntervalMinutes = upcomingTPC.MaxMin;
                upcomingTabBrowserTimerExportUserControl.ExportFileAutoDownloadEnabled = exportFileAutoDownloadEnabledCheckBox.Checked;
                upcomingTabBrowserTimerExportUserControl.DloadDone += DloadDoneHandler;
                upcomingTabBrowserTimerExportUserControl.ResetTimers();
                upcomingTabBrowserTimerExportUserControl.GoToURL(upcomingTPC.URL);
            }

            if (intransitTPC != null)
            {
                intransitTabBrowserTimerExportUserControl.MinRandomIntervalMinutes = intransitTPC.MinMin;
                intransitTabBrowserTimerExportUserControl.MaxRandomIntervalMinutes = intransitTPC.MaxMin;
                intransitTabBrowserTimerExportUserControl.ExportFileAutoDownloadEnabled = exportFileAutoDownloadEnabledCheckBox.Checked;
                intransitTabBrowserTimerExportUserControl.DloadDone += DloadDoneHandler;
                intransitTabBrowserTimerExportUserControl.ResetTimers();
                intransitTabBrowserTimerExportUserControl.GoToURL(intransitTPC.URL);
            }

            if (historyTPC != null)
            {
                historyTabBrowserTimerExportUserControl.MinRandomIntervalMinutes = historyTPC.MinMin;
                historyTabBrowserTimerExportUserControl.MaxRandomIntervalMinutes = historyTPC.MaxMin;
                historyTabBrowserTimerExportUserControl.ExportFileAutoDownloadEnabled = exportFileAutoDownloadEnabledCheckBox.Checked;
                historyTabBrowserTimerExportUserControl.DloadDone += DloadDoneHandler;
                historyTabBrowserTimerExportUserControl.ResetTimers();
                historyTabBrowserTimerExportUserControl.GoToURL(historyTPC.URL);
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

        private void openSettingsButton_Click(object sender, EventArgs e)
        {
            SettingsForm sForm = new SettingsForm();

            if (sForm.ShowDialog() == DialogResult.OK)
            {
                SaveConfiguration();
                // ...
            }
        }

        private void MainForm_Load(object sender, EventArgs e)
        {

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

        void SaveConfiguration()
        {
            string confJsonString = Newtonsoft.Json.JsonConvert.SerializeObject(GlobalContext.SerializedConfiguration, Formatting.Indented);

            File.WriteAllText(configurationFilePath, confJsonString);
        }

        void LoadConfiguration()
        {
            try
            {
                // Read the file content
                string confJsonString = File.ReadAllText(configurationFilePath);

                // Log or print the file content for debugging purposes
                Console.WriteLine("Configuration File Content:");
                Console.WriteLine(confJsonString);

                // Deserialize the JSON string into the configuration object
                GlobalContext.SerializedConfiguration =
                    Newtonsoft.Json.JsonConvert.DeserializeObject<SerializedConfiguration>(confJsonString);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading configuration: {ex.Message}");
                throw;
            }
        }


        // This is the directory in which  the settings, output files and measurements directory are placed
        // Normally it would be the %APPDATA% - User Application Path directory but for portability reasons (to be easily moved between computers keeping settings), the application path can be used
        public static string GetFileStoragePath()
        {
            return Utilities.GetApplicationPath();
        }

        bool displayOnlyDriverGroupNameInDriversForm = false;

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