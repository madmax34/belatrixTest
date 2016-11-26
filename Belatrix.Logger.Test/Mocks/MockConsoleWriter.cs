using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using BelatrixTest.Logger.Interfaces;
using BelatrixTest.Logger.Messages;

namespace Belatrix.Logger.Test.Mocks
{
    public class MockedConsoleWriter : IConsoleWriter
    {
        public IList<MockedMessage> MockedMessages { get; set; }

        private ConsoleColor _currentConsoleColor;

        public MockedConsoleWriter()
        {
            MockedMessages = new List<MockedMessage>();
        }


        public void Write(string format, params object[] args)
        {
            if (!format.Contains("|"))
            {
                Console.Write(format, args);
            }

            var message = ParseMessage(format);
            message.ForegroundColor = _currentConsoleColor;

            MockedMessages.Add(message);
        }

        public void WriteLine(string format, params object[] args)
        {
            if (!format.Contains("|"))
            {
                Console.Write(format, args);
            }

            var message = ParseMessage(format);
            message.ForegroundColor = _currentConsoleColor;

            MockedMessages.Add(message);
        }

        public void SetForegroundColor(ConsoleColor color)
        {
            _currentConsoleColor = color;
        }

        private MockedMessage ParseMessage(string format)
        {
            var contentArray = format.Split('|');

            LogLevel logLevel;
            Enum.TryParse(contentArray[2], out logLevel);

            return new MockedMessage
            {
                Id = Guid.Parse(contentArray[0]),
                Date = DateTime.Parse(contentArray[1], CultureInfo.InvariantCulture),
                LogLevel = logLevel,
                LogMessage = contentArray[3]
            };
        }
    }
}
