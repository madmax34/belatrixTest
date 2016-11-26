using System;
using System.Globalization;
using System.IO;
using BelatrixTest.Logger.Interfaces;

namespace BelatrixTest.Logger
{
    public class FileLogger : ILogger
    {
        private static FileLogger _instance;
        private readonly string _path;
        private static readonly object Locker = new Object();

        private FileLogger()
        {
            _path = Path.Combine(ConfigurationHelper.LogFileFolder, ConfigurationHelper.LogFileName);
        }

        public static FileLogger GetInstance()
        {
            return _instance ?? (_instance = new FileLogger());
        }

        public void Log(ILogMessage message)
        {
            lock (Locker)
            {
                var logMessage = $"{message.Id}|{message.Date.ToString(CultureInfo.InvariantCulture)}|{message.LogLevel}|{message.LogMessage}";
                using (var sw = File.AppendText(_path))
                {
                    sw.WriteLine(logMessage);
                }
            }
        }
    }
}
