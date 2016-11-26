using System;
using BelatrixTest.Logger.Messages;

namespace BelatrixTest.Logger.Interfaces
{
    public interface ILogMessage
    {
        Guid Id { get; }
        DateTime Date { get; }
        LogLevel LogLevel { get; }
        string LogMessage { get; }
    }
}