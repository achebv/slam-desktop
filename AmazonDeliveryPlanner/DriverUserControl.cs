using AmazonDeliveryPlanner.API;
using CefSharp;
using CefSharp.WinForms;
using System;
using System.Web;
using System.Windows.Forms;

namespace AmazonDeliveryPlanner
{
    public partial class DriverUserControl : UserControl
    {
        Driver driver;

        //. API.data.DriverRouteEntity driverRouteEntity;

        RequestContextSettings requestContextSettings = null;
        //string plan_note = null;
        //string op_note = null;

        public event EventHandler/*<SessionClosedEventArgs>*/ SessionClosed;
        public event EventHandler<OpenURLEventArgs> OpenURL;

        public SplitContainer SplitContainer { get => this.splitContainer1; }

        TabControl _urlsTabControl;
        public TabControl UrlsTabControl { get => _urlsTabControl; set => _urlsTabControl = value; }

        public DriverUserControl(Driver driver/*, RequestContextSettings requestContextSettings*/)
        {
            InitializeComponent();

            this.driver = driver;

            //. this.requestContextSettings = requestContextSettings;

            InitPanel2Browser();
        }

        public Driver Driver { get => driver; }

        private void DriverUserControl_Load(object sender, EventArgs e)
        {
            //savedIdLabel.Text = "not saved";
            // this.BackColor = Color.Red; //xtest

            locationLabel.Text = driver.more_info.address.Length > 55 ? driver.more_info.address.Substring(0, 55) + "..." : driver.more_info.address;
            // odometerLabel.Text = driver.more_info.km.ToString() + " km";
            // regPlateLabel.Text = driver.reg_plate;
            // currentJobLabel.Text = "?";
            // nextJobLabel.Text = "?";

            // testLabel.Text = string.Format("{0}  {1} {2}  {3}", driver.driver_id, driver.first_name, driver.last_name, driver.group_name);

            fileDownloadedLabel.Text = "";
            //dayRadioButton.Checked = true;
        }


        private void closeButton_Click(object sender, EventArgs e)
        {
            // Show a confirmation dialog to the user
            DialogResult result = MessageBox.Show("Are you sure you want to close the session?", "Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            // Check the user's response from the confirmation dialog
            if (result == DialogResult.Yes)
            {

                // Raise the SessionClosed event if the user confirms
                if (SessionClosed != null)
                    SessionClosed(this, EventArgs.Empty);
            }
            // If the user clicks "No," the action will be canceled and the event won't be raised.
        }

        private void openAddressGoogleMapsButton_Click(object sender, EventArgs e)
        {
            string gmURL = "https://www.google.com/maps/search/?api=1&query=" + HttpUtility.UrlEncode(driver.more_info.address);

            // System.Diagnostics.Process.Start(gmURL);
            this.OpenURL(this, new OpenURLEventArgs(gmURL, this));
        }

        private void addBrowserTabButton_Click(object sender, EventArgs e)
        {
            this.OpenURL(this, new OpenURLEventArgs("", this));
        }

        Timer t;

        public void UpdateUploadLabel(string text)
        {
            fileDownloadedLabel.Text = text;

            if (t != null)
            {
                t.Enabled = false;
                t.Dispose();
                t = null;
            }

            t = new Timer();

            t.Interval = 14000;
            t.Tick += T_Tick;
            t.Enabled = true;
        }

        private void T_Tick(object sender, EventArgs e)
        {
            (sender as Timer).Enabled = false;
            t = null;

            fileDownloadedLabel.Text = "";
        }

        public void UpdateAutoDownloadLabel(string text)
        {
            autoDownloadStatusLabel.Text = text;
        }

        private void DriverUserControl_Resize(object sender, EventArgs e)
        {
            this.PerformLayout();
        }

        void InitPanel2Browser()
        {
            // !
            // System.AccessViolationException: 'Attempted to read or write protected memory. This is often an indication that other memory is corrupt.'
            //GlobalContext.GlobalCefSettings.CachePath = @"C:\temp\cache_1";
            // string cachePath = GlobalContext.GlobalCefSettings.CachePath;
            // requestContextSettings.CachePath

            // string upworkStartUrl = "www.google.com"; // "https://www.upwork.com";
            // string upworkStartUrl = "https://www.upwork.com";

            ChromiumWebBrowser browser2 = new ChromiumWebBrowser();
            // browser = new ChromiumWebBrowser(url, requestContextSettings.);

            if (requestContextSettings != null)
                browser2.RequestContext = new RequestContext(requestContextSettings);
            // projectSearchTabPage.SuspendLayout();

            // browser2.DownloadHandler = new DownloadHandler();

            // ((DownloadHandler)browser.DownloadHandler).OnDownloadUpdatedFired += BrowserUserControl_OnDownloadUpdatedFired;

            // this.Controls.Add(browser);
            splitContainer1.Panel2.Controls.Add(browser2);

            browser2.Dock = DockStyle.Fill;

            // projectSearchTabPage.ResumeLayout();

            // projectSearchTabPage.Refresh();

            // browser.LoadingStateChanged += Browser_LoadingStateChanged;
            // browser2.FrameLoadEnd += Browser_FrameLoadEnd;

            //browser.IsBrowserInitializedChanged += Browser_IsBrowserInitializedChanged;

            // browser.RequestHandler = new CustomRequestHandler();

            //browser.Show();
            //browser.PerformLayout();
            this.PerformLayout();
            this.Invalidate();
            this.Refresh();
            //browser.Invalidate();
            //browser.Refresh();

            // LoadMFIFCPage();

            // string panel2URL = string.Format("https://dlg1.app/planning-overview/{0}/info", driverId);

            if (string.IsNullOrWhiteSpace(GlobalContext.SerializedConfiguration.PlanningOverviewURL))
            {
                GlobalContext.Log("Error: planning_overview_url value not set in configuration file.");
                MessageBox.Show("planning_overview_url value not set in configuration file.", GlobalContext.ApplicationTitle);
                return;
            }

            // ex.: http://dlg1.app/planning-overview/{user_id}/info
            string panel2URL = GlobalContext.SerializedConfiguration.AdminURL
                + GlobalContext.SerializedConfiguration.PlanningOverviewURL.Replace("{user_id}", driver.driver_id.ToString() /*driverId.ToString()*/)
                + "/" + GlobalContext.LoggedInPlanner.token + "-" + driver.message_id;

            // MessageBox.Show(panel2URL);

            browser2.Load(panel2URL);

            GlobalContext.Log("Planning Overview Url is set to:  '{0}'", panel2URL);

            browser2.Dock = DockStyle.Fill;
        }
    }

    //public class SessionClosedEventArgs : EventArgs
    //{
    //    public SessionClosedEventArgs(DriverUserControl driverUC)
    //    {
    //        DriverUC = driverUC;
    //    }

    //    public DriverUserControl DriverUC { get; set; }
    //}

    public class OpenURLEventArgs : EventArgs
    {
        public OpenURLEventArgs(string url, DriverUserControl driverUC)
        {
            URL = url;
            DriverUC = driverUC;
        }

        public DriverUserControl DriverUC { get; set; }
        public string URL { get; set; }
    }

    //// Wrap event invocations inside a protected virtual method
    //// to allow derived classes to override the event invocation behavior
    //protected virtual void OnRaiseCustomEvent(ProjectsAddedEventArgs e)
    //{
    //    // Make a temporary copy of the event to avoid possibility of
    //    // a race condition if the last subscriber unsubscribes
    //    // immediately after the null check and before the event is raised.
    //    EventHandler<ProjectsAddedEventArgs> raiseEvent = ProjectsAdded;

    //    // Event will be null if there are no subscribers
    //    if (raiseEvent != null)
    //    {
    //        // Format the string to send inside the CustomEventArgs parameter
    //        e.Message += $" at {DateTime.Now}";

    //        // Call to raise the event.
    //        raiseEvent(this, e);
    //    }
    //}
}
