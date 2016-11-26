using System;
using BelatrixTest.Logger.Interfaces;

namespace BelatrixTest.Logger.Messages
{
    public class Message : ILogMessage
    {
        public Guid Id { get; }
        public DateTime Date { get; }
        public LogLevel LogLevel { get; }
        public string LogMessage { get; }

        public Message(string message)
        {
            Id = Guid.NewGuid();
            Date = DateTime.Now;
            LogMessage = message;
            LogLevel = LogLevel.Message;
        }
    }
}