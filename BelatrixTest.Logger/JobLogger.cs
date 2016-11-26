using System;
using System.Linq;
using BelatrixTest.Logger.Interfaces;

namespace BelatrixTest.Logger
{
    public class JobLogger
    {
        private static JobLogger _instance;
        private LoggerConfiguration _configuration;

        private JobLogger()
        {
        }

        public static JobLogger GetInstance()
        {
            if (_instance == null)
            {
                _instance = new JobLogger();
            }

            return _instance;
        }

        public void Configure(LoggerConfiguration configuration)
        {
            _configuration = configuration;
        }

        public void Log(ILogMessage message)
        {
            if (!_configuration.IsValid())
            {
                throw new InvalidOperationException();
            }

            if (_configuration.LogLevels.All(l => l != message.LogLevel))
            {
                return;
            }

            foreach (var logger in _configuration.Loggers)
            {
                logger.Log(message);
            }
        }
    }
}