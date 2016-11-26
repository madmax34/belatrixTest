using System;
using BelatrixTest.Logger.Interfaces;

namespace BelatrixTest.Logger.Messages
{
    public class Warning : ILogMessage
    {
        public Guid Id { get; }
        public DateTime Date { get; }
        public LogLevel LogLevel { get; }
        public string LogMessage { get; }

        public Warning(string message)
        {
            Id = Guid.NewGuid();
            Date = DateTime.Now;
            LogMessage = message;
            LogLevel = LogLevel.Warning;
        }
    }
}