using System;
using System.Globalization;
using BelatrixTest.Logger.Interfaces;
using BelatrixTest.Logger.Messages;

namespace BelatrixTest.Logger
{
    public class ConsoleLogger : ILogger
    {
        private IConsoleWriter _consoleWriter;

        public ConsoleLogger(IConsoleWriter consoleWriter)
        {
            _consoleWriter = consoleWriter;
            consoleWriter.SetForegroundColor(ConsoleColor.White);
        }

        public void Log(ILogMessage message)
        {
            ConsoleColor color = ConsoleColor.White;

            switch (message.LogLevel)
            {
                case LogLevel.Warning:
                    color = ConsoleColor.Yellow;
                    break;
                case LogLevel.Error:
                    color = ConsoleColor.Red;
                    break;
            }

            _consoleWriter.SetForegroundColor(color);

            var logMessage = $"{message.Id}|{message.Date.ToString(CultureInfo.InvariantCulture)}|{message.LogLevel}|{message.LogMessage}";
            _consoleWriter.WriteLine(logMessage);
        }
    }
}