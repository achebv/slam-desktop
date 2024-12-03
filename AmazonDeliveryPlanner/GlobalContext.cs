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
        public static DriverList LastDriverList { get => lastDriverList; set => lastDriverList = value; }
        public static SerializedConfiguration SerializedConfiguration { get => serializedConfiguration; set => serializedConfiguration = value; }
        public static string ConfigurationFileName { get => "conf.json"; }
        public static PlannerEntity LoggedInPlanner { get => loggedInPlanner; set => loggedInPlanner = value; }
        public static Dictionary<string, string> Scripts { get => scripts; set => scripts = value; }

        public static ApiConfig ApiConfig { get; set; }

        public static void Log(string text, params object[] args)
        {
            // string msg = string.Format(text, args);

            gLogger.Log(0, string.Format(text, args));
        }

        public static void LogWithLevel(int level, string text, params object[] args)
        {
            // string msg = string.Format(text, args);

            gLogger.Log(level, string.Format(text, args));
        }

        // public static GLogger GLogger { get => gLogger; set => gLogger = value; }

        //static string optionsFile = "settings.xml";

        //public static string OptionsFile
        //{
        //    get { return GlobalContext.optionsFile; }
        //    set { GlobalContext.optionsFile = value; }
        //}

        static GLogger gLogger;

        static GlobalContext()
        {
            gLogger = new GLogger();

            gLogger.InitLogger();
        }

        static CefSettings globalCefSettings;

        // static string userAgent = "Mozilla/5.0 (Windows NT 6.1; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/80.0.3987.163 Safari/537.36";


        // static string _fillRelayAuthenticationScript = "";
        // static Dictionary<string, string> scripts;

        // static bool showDevTools;

        // static List<string> urls;

        static DriverList lastDriverList;

        static SerializedConfiguration serializedConfiguration;

        static PlannerEntity loggedInPlanner;

        static Dictionary<string, string> scripts;
    }
}
