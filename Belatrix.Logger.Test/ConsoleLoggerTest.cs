using System;
using System.Globalization;
using System.Linq;
using Belatrix.Logger.Test.Mocks;
using BelatrixTest.Logger;
using BelatrixTest.Logger.Messages;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Belatrix.Logger.Test
{
    [TestClass]
    public class ConsoleLoggerTest
    {
        [TestMethod]
        public void Log_WithValidMessage_ShouldLogMessage()
        {
            var mockedConsoleWriter = new MockedConsoleWriter();
            var logger = new ConsoleLogger(mockedConsoleWriter);

            var message = new Message("This is the message");
            logger.Log(message);

            var loggedContent = mockedConsoleWriter.MockedMessages.ToList();

            Assert.AreEqual(1, loggedContent.Count);

            foreach (var content in loggedContent)
            {
                Assert.AreEqual(message.Id, content.Id);
                Assert.AreEqual(message.Date.ToString(CultureInfo.InvariantCulture), content.Date.ToString(CultureInfo.InvariantCulture));
                Assert.AreEqual(message.LogLevel, content.LogLevel);
                Assert.AreEqual(message.LogMessage, content.LogMessage);
                Assert.AreEqual(ConsoleColor.White, content.ForegroundColor);
            }
        }

        [TestMethod]
        public void Log_WithMultipleMessages_ShouldLogMessage()
        {
            var mockedConsoleWriter = new MockedConsoleWriter();
            var logger = new ConsoleLogger(mockedConsoleWriter);

            var message = new Message("This is the message");
            logger.Log(message);

            var message2 = new Message("This is the message");
            logger.Log(message2);

            var loggedContent = mockedConsoleWriter.MockedMessages.ToList();

            Assert.AreEqual(2, loggedContent.Count);

            Assert.AreEqual(message.Id, loggedContent[0].Id);
            Assert.AreEqual(message.Date.ToString(CultureInfo.InvariantCulture), loggedContent[0].Date.ToString(CultureInfo.InvariantCulture));
            Assert.AreEqual(message.LogLevel, loggedContent[0].LogLevel);
            Assert.AreEqual(message.LogMessage, loggedContent[0].LogMessage);
            Assert.AreEqual(ConsoleColor.White, loggedContent[0].ForegroundColor);

            Assert.AreEqual(message2.Id, loggedContent[1].Id);
            Assert.AreEqual(message2.Date.ToString(CultureInfo.InvariantCulture), loggedContent[1].Date.ToString(CultureInfo.InvariantCulture));
            Assert.AreEqual(message2.LogLevel, loggedContent[1].LogLevel);
            Assert.AreEqual(message2.LogMessage, loggedContent[1].LogMessage);
            Assert.AreEqual(ConsoleColor.White, loggedContent[1].ForegroundColor);
        }

        [TestMethod]
        public void Log_WithValidWarning_ShouldLogMessage()
        {
            var mockedConsoleWriter = new MockedConsoleWriter();
            var logger = new ConsoleLogger(mockedConsoleWriter);

            var warning = new Warning("This is the message");
            logger.Log(warning);

            var loggedContent = mockedConsoleWriter.MockedMessages.ToList();

            Assert.AreEqual(1, loggedContent.Count);

            foreach (var content in loggedContent)
            {
                Assert.AreEqual(warning.Id, content.Id);
                Assert.AreEqual(warning.Date.ToString(CultureInfo.InvariantCulture), content.Date.ToString(CultureInfo.InvariantCulture));
                Assert.AreEqual(warning.LogLevel, content.LogLevel);
                Assert.AreEqual(warning.LogMessage, content.LogMessage);
                Assert.AreEqual(ConsoleColor.Yellow, content.ForegroundColor);
            }
        }

        [TestMethod]
        public void Log_WithMultipleWarnings_ShouldLogMessage()
        {
            var mockedConsoleWriter = new MockedConsoleWriter();
            var logger = new ConsoleLogger(mockedConsoleWriter);

            var warning = new Warning("This is the message");
            logger.Log(warning);

            var warning2 = new Warning("This is the message");
            logger.Log(warning2);

            var loggedContent = mockedConsoleWriter.MockedMessages.ToList();

            Assert.AreEqual(2, loggedContent.Count);

            Assert.AreEqual(warning.Id, loggedContent[0].Id);
            Assert.AreEqual(warning.Date.ToString(CultureInfo.InvariantCulture), loggedContent[0].Date.ToString(CultureInfo.InvariantCulture));
            Assert.AreEqual(warning.LogLevel, loggedContent[0].LogLevel);
            Assert.AreEqual(warning.LogMessage, loggedContent[0].LogMessage);
            Assert.AreEqual(ConsoleColor.Yellow, loggedContent[0].ForegroundColor);

            Assert.AreEqual(warning2.Id, loggedContent[1].Id);
            Assert.AreEqual(warning2.Date.ToString(CultureInfo.InvariantCulture), loggedContent[1].Date.ToString(CultureInfo.InvariantCulture));
            Assert.AreEqual(warning2.LogLevel, loggedContent[1].LogLevel);
            Assert.AreEqual(warning2.LogMessage, loggedContent[1].LogMessage);
            Assert.AreEqual(ConsoleColor.Yellow, loggedContent[1].ForegroundColor);
        }

        [TestMethod]
        public void Log_WithValidError_ShouldLogMessage()
        {
            var mockedConsoleWriter = new MockedConsoleWriter();
            var logger = new ConsoleLogger(mockedConsoleWriter);

            var error = new Error("This is the message");
            logger.Log(error);

            var loggedContent = mockedConsoleWriter.MockedMessages.ToList();

            Assert.AreEqual(1, loggedContent.Count);

            foreach (var content in loggedContent)
            {
                Assert.AreEqual(error.Id, content.Id);
                Assert.AreEqual(error.Date.ToString(CultureInfo.InvariantCulture), content.Date.ToString(CultureInfo.InvariantCulture));
                Assert.AreEqual(error.LogLevel, content.LogLevel);
                Assert.AreEqual(error.LogMessage, content.LogMessage);
                Assert.AreEqual(ConsoleColor.Red, content.ForegroundColor);
            }
        }

        [TestMethod]
        public void Log_WithMultipleError_ShouldLogMessage()
        {
            var mockedConsoleWriter = new MockedConsoleWriter();
            var logger = new ConsoleLogger(mockedConsoleWriter);

            var error = new Error("This is the message");
            logger.Log(error);

            var error2 = new Error("This is the message");
            logger.Log(error2);

            var loggedContent = mockedConsoleWriter.MockedMessages.ToList();

            Assert.AreEqual(2, loggedContent.Count);

            Assert.AreEqual(error.Id, loggedContent[0].Id);
            Assert.AreEqual(error.Date.ToString(CultureInfo.InvariantCulture), loggedContent[0].Date.ToString(CultureInfo.InvariantCulture));
            Assert.AreEqual(error.LogLevel, loggedContent[0].LogLevel);
            Assert.AreEqual(error.LogMessage, loggedContent[0].LogMessage);
            Assert.AreEqual(ConsoleColor.Red, loggedContent[0].ForegroundColor);

            Assert.AreEqual(error2.Id, loggedContent[1].Id);
            Assert.AreEqual(error2.Date.ToString(CultureInfo.InvariantCulture), loggedContent[1].Date.ToString(CultureInfo.InvariantCulture));
            Assert.AreEqual(error2.LogLevel, loggedContent[1].LogLevel);
            Assert.AreEqual(error2.LogMessage, loggedContent[1].LogMessage);
            Assert.AreEqual(ConsoleColor.Red, loggedContent[1].ForegroundColor);
        }
    }
}
