using System.Collections.Generic;
using System.Linq;
using BelatrixTest.Logger.Interfaces;
using BelatrixTest.Logger.Messages;

namespace BelatrixTest.Logger
{
    public class LoggerConfiguration
    {
        public IList<LogLevel> LogLevels { get; set; }

        public List<ILogger> Loggers { get; set; }

        public LoggerConfiguration(List<ILogger> loggers, IList<LogLevel> logLevels)
        {
            Loggers = loggers;
            LogLevels = logLevels;
        }

        public bool IsValid()
        {
            return Loggers != null && Loggers.Any() && LogLevels != null && LogLevels.Any();
        }
    }
}
