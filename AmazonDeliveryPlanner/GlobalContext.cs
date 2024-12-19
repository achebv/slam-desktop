using AmazonDeliveryPlanner.API;
using AmazonDeliveryPlanner.API.data;
using CefSharp.WinForms;
using System.Collections.Generic;

namespace AmazonDeliveryPlanner
{
    public static class GlobalContext
    {
        static MainForm mainForm;

        public static MainForm MainWindow
        {
            get { return GlobalContext.mainForm; }
            set { GlobalContext.mainForm = value; }
        }

        static string applicationTitle;

        public static string ApplicationTitle
        {
            get { return GlobalContext.applicationTitle; }
            set { GlobalContext.applicationTitle = value; }
        }

        public static CefSettings GlobalCefSettings { get => globalCefSettings; set => globalCefSettings = value; }

        // public static string UserAgent { get => userAgent; set => userAgent = value; }
        // public static List<string> Urls { get => urls; set => urls = value; }
        public static SerializedConfiguration SerializedConfiguration { get => serializedConfiguration; set => serializedConfiguration = value; }
        public static string ConfigurationFileName { get => "conf.json"; }
        public static PlannerEntity LoggedInPlanner { get => loggedInPlanner; set => loggedInPlanner = value; }
        public static Dictionary<string, string> Scripts { get => scripts; set => scripts = value; }

        public static ApiConfig ApiConfig { get; set; }

        public static void Log(string text, params object[] args)
        {
            gLogger.Log(0, string.Format(text, args));
        }

        static GLogger gLogger;

        static GlobalContext()
        {
            gLogger = new GLogger();

            gLogger.InitLogger();
        }

        static CefSettings globalCefSettings;

        static SerializedConfiguration serializedConfiguration;

        static PlannerEntity loggedInPlanner;

        static Dictionary<string, string> scripts;
    }
}
