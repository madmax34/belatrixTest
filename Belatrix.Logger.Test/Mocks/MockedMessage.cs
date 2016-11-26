using System;
using BelatrixTest.Logger.Interfaces;
using BelatrixTest.Logger.Messages;

namespace Belatrix.Logger.Test.Mocks
{
    public class MockedMessage : ILogMessage
    {
        public Guid Id { get; set; }
        public DateTime Date { get; set; }
        public LogLevel LogLevel { get; set; }
        public string LogMessage { get; set; }
        public ConsoleColor ForegroundColor { get; set; }
    }
}