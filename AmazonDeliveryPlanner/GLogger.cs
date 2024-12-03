using System;
using System.IO;

namespace AmazonDeliveryPlanner
{
    public class GLogger
    {
        string logFilePath;

        StreamWriter logSw;

        public string LogFilePath { get => logFilePath; set => logFilePath = value; }

        public void InitLogger()
        {
            // 2015-08-12 02:16:30,734 [10] : Application start
            LogFilePath = Path.Combine(AmazonDeliveryPlanner.Utilities.GetApplicationPath(), string.Format("output_{0}.log", DateTime.Now.ToString("yyyy-MM-dd_HHmmss")));

            logSw = new StreamWriter(LogFilePath, true);
        }

        public void Log(int level, string text)
        {
            if (level <= 1)
                GlobalContext.MainWindow.Output(text);

            //File.AppendAllText(logFilePath, string.Format("{0} : {1}", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff"), text));
            logSw.WriteLine(string.Format("{0} : {1}", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff"), text));
            logSw.Flush();
        }
    }
}
