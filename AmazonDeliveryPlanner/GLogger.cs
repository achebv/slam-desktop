using System;
using System.IO;
using System.Text;
using System.Threading;

namespace AmazonDeliveryPlanner
{
    public class GLogger
    {
        string logFilePath;
        private static readonly Mutex mutex = new Mutex();

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

            string logEntry = string.Format("{0} : {1}", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff"), text);
            mutex.WaitOne();
            try
            {
                logSw.WriteLine(logEntry);
                logSw.Flush(); // Ensure the log entry is written to the file immediately
            }
            finally
            {
                mutex.ReleaseMutex();
            }
        }



    }
}
