using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading;
using Belatrix.Logger.Test.Mocks;
using BelatrixTest.Logger;
using BelatrixTest.Logger.Interfaces;
using BelatrixTest.Logger.Messages;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Belatrix.Logger.Test
{
    [TestClass]
    public class JobLoggerTest
    {
        private string _filePath;

        [ClassInitialize]
        public static void ClassInitialize(TestContext context)
        {

            if (File.Exists("LoggerDB.db"))
            {
                File.Delete("LoggerDB.db");
            }

            var connection = new SQLiteConnection(ConfigurationHelper.ConnectionString);
            connection.Open();

            var createCommand = new SQLiteCommand("CREATE TABLE Logger (Id UID, LogDate varchar(25), LogLevel varchar(10), LogMessage varchar(500))", connection);
            createCommand.ExecuteNonQuery();

            connection.Close();
        }

        [TestInitialize]
        public void Setup()
        {
            _filePath = Path.Combine(ConfigurationHelper.LogFileFolder, ConfigurationHelper.LogFileName);

            if (File.Exists(_filePath))
            {
                File.Delete(_filePath);
            }

            var connection = new SQLiteConnection(ConfigurationHelper.ConnectionString);
            connection.Open();

            var deleteCommand = new SQLiteCommand("DELETE FROM Logger", connection);
            deleteCommand.ExecuteNonQuery();

            connection.Close();

            Thread.Sleep(1000);
        }

        #region FileLogger
        [TestMethod]
        public void Log_ConfiguredForFileAndMessages_ReceivingMessage_ShouldLogToFile()
        {
            var jobLogger = JobLogger.GetInstance();

            var loggerConfiguration = new LoggerConfiguration(new List<ILogger> { FileLogger.GetInstance() }, new List<LogLevel> { LogLevel.Message });
            jobLogger.Configure(loggerConfiguration);

            var message = new Message("This is the message");
            jobLogger.Log(message);

            var loggedContent = File.ReadAllLines(_filePath);
            AssertFileContent(loggedContent, new List<ILogMessage> { message });
        }

        [TestMethod]
        public void Log_ConfiguredForFileMessagesAndWarnings_ReceivingMessageAndWarning_ShouldLogToFile()
        {
            var jobLogger = JobLogger.GetInstance();

            var loggerConfiguration = new LoggerConfiguration(new List<ILogger> { FileLogger.GetInstance() }, new List<LogLevel> { LogLevel.Message, LogLevel.Warning });
            jobLogger.Configure(loggerConfiguration);

            var message = new Message("This is the message");
            jobLogger.Log(message);

            var warning = new Warning("This is the warning");
            jobLogger.Log(warning);

            var loggedContent = File.ReadAllLines(_filePath);

            Assert.AreEqual(2, loggedContent.Count());
            AssertFileContent(loggedContent, new List<ILogMessage> { message, warning });
        }

        [TestMethod]
        public void Log_ConfiguredForFileMessagesWarningsAndErrors_ReceivingMessageWarningAndError_ShouldLogToFile()
        {
            var jobLogger = JobLogger.GetInstance();

            var loggerConfiguration = new LoggerConfiguration(new List<ILogger> { FileLogger.GetInstance() }, new List<LogLevel> { LogLevel.Message, LogLevel.Warning, LogLevel.Error });
            jobLogger.Configure(loggerConfiguration);

            var message = new Message("This is the message");
            jobLogger.Log(message);

            var warning = new Warning("This is the warning");
            jobLogger.Log(warning);

            var error = new Error("This is the error");
            jobLogger.Log(error);

            var loggedContent = File.ReadAllLines(_filePath);

            Assert.AreEqual(3, loggedContent.Count());
            AssertFileContent(loggedContent, new List<ILogMessage> { message, warning, error });
        }

        [TestMethod]
        public void Log_ConfiguredForFileAndMessages_ReceivingWarning_ShouldNotLogToFile()
        {
            var jobLogger = JobLogger.GetInstance();

            var loggerConfiguration = new LoggerConfiguration(new List<ILogger> { FileLogger.GetInstance() }, new List<LogLevel> { LogLevel.Message });
            jobLogger.Configure(loggerConfiguration);

            var warning = new Warning("This is the warning");
            jobLogger.Log(warning);

            Assert.IsFalse(File.Exists(_filePath));
        }

        [TestMethod]
        public void Log_ConfiguredForFileAndMessages_ReceivingError_ShouldNotLogToFile()
        {
            var jobLogger = JobLogger.GetInstance();

            var loggerConfiguration = new LoggerConfiguration(new List<ILogger> { FileLogger.GetInstance() }, new List<LogLevel> { LogLevel.Message });
            jobLogger.Configure(loggerConfiguration);

            var error = new Error("This is the error");
            jobLogger.Log(error);

            Assert.IsFalse(File.Exists(_filePath));
        }

        [TestMethod]
        public void Log_ConfiguredForFileAndWarnings_ReceivingMessage_ShouldNotLogToFile()
        {
            var jobLogger = JobLogger.GetInstance();

            var loggerConfiguration = new LoggerConfiguration(new List<ILogger> { FileLogger.GetInstance() }, new List<LogLevel> { LogLevel.Message });
            jobLogger.Configure(loggerConfiguration);

            var message = new Error("This is the message");
            jobLogger.Log(message);

            Assert.IsFalse(File.Exists(_filePath));
        }

        [TestMethod]
        public void Log_ConfiguredForFileWarningsAndErrors_ReceivingMessageWarningAndError_ShouldLogToFileOnlyWarningAndError()
        {
            var jobLogger = JobLogger.GetInstance();

            var loggerConfiguration = new LoggerConfiguration(new List<ILogger> { FileLogger.GetInstance() }, new List<LogLevel> { LogLevel.Warning, LogLevel.Error });
            jobLogger.Configure(loggerConfiguration);

            var message = new Message("This is the message");
            jobLogger.Log(message);

            var warning = new Warning("This is the warning");
            jobLogger.Log(warning);

            var error = new Error("This is the error");
            jobLogger.Log(error);

            var loggedContent = File.ReadAllLines(_filePath);

            Assert.AreEqual(2, loggedContent.Count());
            AssertFileContent(loggedContent, new List<ILogMessage> { warning, error });
        }

        [TestMethod]
        public void Log_ConfiguredForFileMessagesAndErrors_ReceivingMessageWarningAndError_ShouldLogToFileOnlyMessageAndError()
        {
            var jobLogger = JobLogger.GetInstance();

            var loggerConfiguration = new LoggerConfiguration(new List<ILogger> { FileLogger.GetInstance() }, new List<LogLevel> { LogLevel.Message, LogLevel.Error });
            jobLogger.Configure(loggerConfiguration);

            var message = new Message("This is the message");
            jobLogger.Log(message);

            var warning = new Warning("This is the warning");
            jobLogger.Log(warning);

            var error = new Error("This is the error");
            jobLogger.Log(error);

            var loggedContent = File.ReadAllLines(_filePath);

            Assert.AreEqual(2, loggedContent.Count());
            AssertFileContent(loggedContent, new List<ILogMessage> { message, error });
        }

        [TestMethod]
        public void Log_ConfiguredForFileMessagesAndWarnings_ReceivingMessageWarningAndError_ShouldLogToFileOnlyMessageAndWarning()
        {
            var jobLogger = JobLogger.GetInstance();

            var loggerConfiguration = new LoggerConfiguration(new List<ILogger> { FileLogger.GetInstance() }, new List<LogLevel> { LogLevel.Message, LogLevel.Warning });
            jobLogger.Configure(loggerConfiguration);

            var message = new Message("This is the message");
            jobLogger.Log(message);

            var warning = new Warning("This is the warning");
            jobLogger.Log(warning);

            var error = new Error("This is the error");
            jobLogger.Log(error);

            var loggedContent = File.ReadAllLines(_filePath);

            Assert.AreEqual(2, loggedContent.Count());
            AssertFileContent(loggedContent, new List<ILogMessage> { message, warning });
        }

        #endregion

        #region DatabaseLogger

        [TestMethod]
        public void Log_ConfiguredForDatabaseAndMessages_ReceivingMessage_ShouldLogToDatabase()
        {
            var jobLogger = JobLogger.GetInstance();

            var loggerConfiguration = new LoggerConfiguration(new List<ILogger> { new DatabaseLogger() }, new List<LogLevel> { LogLevel.Message });
            jobLogger.Configure(loggerConfiguration);

            var message = new Message("This is the message");
            jobLogger.Log(message);

            var count = AssertDatabaseContent(new List<ILogMessage> { message });
            Assert.AreEqual(1, count);
        }

        [TestMethod]
        public void Log_ConfiguredForDatabaseMessagesAndWarnings_ReceivingMessageAndWarning_ShouldLogToDatabase()
        {
            var jobLogger = JobLogger.GetInstance();

            var loggerConfiguration = new LoggerConfiguration(new List<ILogger> { new DatabaseLogger() }, new List<LogLevel> { LogLevel.Message, LogLevel.Warning });
            jobLogger.Configure(loggerConfiguration);

            var message = new Message("This is the message");
            jobLogger.Log(message);

            var warning = new Warning("This is the warning");
            jobLogger.Log(warning);

            var count = AssertDatabaseContent(new List<ILogMessage> { message, warning });

            Assert.AreEqual(2, count);
        }

        [TestMethod]
        public void Log_ConfiguredForDatabaseMessagesWarningsAndErrors_ReceivingMessageWarningAndError_ShouldLogToDatabase()
        {
            var jobLogger = JobLogger.GetInstance();

            var loggerConfiguration = new LoggerConfiguration(new List<ILogger> { new DatabaseLogger() }, new List<LogLevel> { LogLevel.Message, LogLevel.Warning, LogLevel.Error });
            jobLogger.Configure(loggerConfiguration);

            var message = new Message("This is the message");
            jobLogger.Log(message);

            var warning = new Warning("This is the warning");
            jobLogger.Log(warning);

            var error = new Error("This is the error");
            jobLogger.Log(error);

            var count = AssertDatabaseContent(new List<ILogMessage> { message, warning, error });

            Assert.AreEqual(3, count);
        }

        [TestMethod]
        public void Log_ConfiguredForDatabaseAndMessages_ReceivingWarning_ShouldNotLogToDatabase()
        {
            var jobLogger = JobLogger.GetInstance();

            var loggerConfiguration = new LoggerConfiguration(new List<ILogger> { new DatabaseLogger() }, new List<LogLevel> { LogLevel.Message });
            jobLogger.Configure(loggerConfiguration);

            var warning = new Warning("This is the warning");
            jobLogger.Log(warning);

            var connection = new SQLiteConnection(ConfigurationHelper.ConnectionString);
            connection.Open();

            var command = new SQLiteCommand("SELECT Count(*) FROM Logger", connection);
            int count = int.Parse(command.ExecuteScalar().ToString());

            Assert.AreEqual(0, count);

            connection.Close();
        }

        [TestMethod]
        public void Log_ConfiguredForDatabaseAndMessages_ReceivingError_ShouldNotLogToDatabase()
        {
            var jobLogger = JobLogger.GetInstance();

            var loggerConfiguration = new LoggerConfiguration(new List<ILogger> { new DatabaseLogger() }, new List<LogLevel> { LogLevel.Message });
            jobLogger.Configure(loggerConfiguration);

            var error = new Error("This is the error");
            jobLogger.Log(error);

            var connection = new SQLiteConnection(ConfigurationHelper.ConnectionString);
            connection.Open();

            var command = new SQLiteCommand("SELECT Count(*) FROM Logger", connection);
            int count = int.Parse(command.ExecuteScalar().ToString());

            Assert.AreEqual(0, count);

            connection.Close();
        }

        [TestMethod]
        public void Log_ConfiguredForDatabaseAndWarnings_ReceivingMessage_ShouldNotLogToDatabase()
        {
            var jobLogger = JobLogger.GetInstance();

            var loggerConfiguration = new LoggerConfiguration(new List<ILogger> { new DatabaseLogger() }, new List<LogLevel> { LogLevel.Message });
            jobLogger.Configure(loggerConfiguration);

            var message = new Error("This is the message");
            jobLogger.Log(message);

            var connection = new SQLiteConnection(ConfigurationHelper.ConnectionString);
            connection.Open();

            var command = new SQLiteCommand("SELECT Count(*) FROM Logger", connection);
            int count = int.Parse(command.ExecuteScalar().ToString());

            Assert.AreEqual(0, count);

            connection.Close();
        }

        [TestMethod]
        public void Log_ConfiguredForDatabaseWarningsAndErrors_ReceivingMessageWarningAndError_ShouldLogToDatabaseOnlyWarningAndError()
        {
            var jobLogger = JobLogger.GetInstance();

            var loggerConfiguration = new LoggerConfiguration(new List<ILogger> { new DatabaseLogger() }, new List<LogLevel> { LogLevel.Warning, LogLevel.Error });
            jobLogger.Configure(loggerConfiguration);

            var message = new Message("This is the message");
            jobLogger.Log(message);

            var warning = new Warning("This is the warning");
            jobLogger.Log(warning);

            var error = new Error("This is the error");
            jobLogger.Log(error);

            var count = AssertDatabaseContent(new List<ILogMessage> { warning, error });

            Assert.AreEqual(2, count);
        }

        [TestMethod]
        public void Log_ConfiguredForDatabaseMessagesAndErrors_ReceivingMessageWarningAndError_ShouldLogToDatabaseOnlyMessageAndError()
        {
            var jobLogger = JobLogger.GetInstance();

            var loggerConfiguration = new LoggerConfiguration(new List<ILogger> { new DatabaseLogger() }, new List<LogLevel> { LogLevel.Message, LogLevel.Error });
            jobLogger.Configure(loggerConfiguration);

            var message = new Message("This is the message");
            jobLogger.Log(message);

            var warning = new Warning("This is the warning");
            jobLogger.Log(warning);

            var error = new Error("This is the error");
            jobLogger.Log(error);

            var count = AssertDatabaseContent(new List<ILogMessage> { message, error });

            Assert.AreEqual(2, count);
        }

        [TestMethod]
        public void Log_ConfiguredForDatabaseMessagesAndWarnings_ReceivingMessageWarningAndError_ShouldLogToDatabaseOnlyMessageAndWarning()
        {
            var jobLogger = JobLogger.GetInstance();

            var loggerConfiguration = new LoggerConfiguration(new List<ILogger> { new DatabaseLogger() }, new List<LogLevel> { LogLevel.Message, LogLevel.Warning });
            jobLogger.Configure(loggerConfiguration);

            var message = new Message("This is the message");
            jobLogger.Log(message);

            var warning = new Warning("This is the warning");
            jobLogger.Log(warning);

            var error = new Error("This is the error");
            jobLogger.Log(error);

            var count = AssertDatabaseContent(new List<ILogMessage> { message, warning });

            Assert.AreEqual(2, count);
        }

        #endregion

        #region ConsoleLogger

        [TestMethod]
        public void Log_ConfiguredForConsoleAndMessages_ReceivingMessage_ShouldLogToConsole()
        {
            var jobLogger = JobLogger.GetInstance();
            var mockedConsoleWriter = new MockedConsoleWriter();

            var loggerConfiguration = new LoggerConfiguration(new List<ILogger> { new ConsoleLogger(mockedConsoleWriter) }, new List<LogLevel> { LogLevel.Message });
            jobLogger.Configure(loggerConfiguration);

            var message = new Message("This is the message");
            jobLogger.Log(message);

            AssertConsoleContent(mockedConsoleWriter.MockedMessages, new List<ILogMessage> { message });

            Assert.AreEqual(1, mockedConsoleWriter.MockedMessages.Count);
        }

        [TestMethod]
        public void Log_ConfiguredForConsoleMessagesAndWarnings_ReceivingMessageAndWarning_ShouldLogToConsole()
        {
            var jobLogger = JobLogger.GetInstance();
            var mockedConsoleWriter = new MockedConsoleWriter();

            var loggerConfiguration = new LoggerConfiguration(new List<ILogger> { new ConsoleLogger(mockedConsoleWriter) }, new List<LogLevel> { LogLevel.Message, LogLevel.Warning });
            jobLogger.Configure(loggerConfiguration);

            var message = new Message("This is the message");
            jobLogger.Log(message);

            var warning = new Warning("This is the warning");
            jobLogger.Log(warning);

            AssertConsoleContent(mockedConsoleWriter.MockedMessages, new List<ILogMessage> { message, warning });

            Assert.AreEqual(2, mockedConsoleWriter.MockedMessages.Count);
        }

        [TestMethod]
        public void Log_ConfiguredForConsoleMessagesWarningsAndErrors_ReceivingMessageWarningAndError_ShouldLogToConsole()
        {
            var jobLogger = JobLogger.GetInstance();
            var mockedConsoleWriter = new MockedConsoleWriter();

            var loggerConfiguration = new LoggerConfiguration(new List<ILogger> { new ConsoleLogger(mockedConsoleWriter) }, new List<LogLevel> { LogLevel.Message, LogLevel.Warning, LogLevel.Error });
            jobLogger.Configure(loggerConfiguration);

            var message = new Message("This is the message");
            jobLogger.Log(message);

            var warning = new Warning("This is the warning");
            jobLogger.Log(warning);

            var error = new Error("This is the error");
            jobLogger.Log(error);

            AssertConsoleContent(mockedConsoleWriter.MockedMessages, new List<ILogMessage> { message, warning, error });

            Assert.AreEqual(3, mockedConsoleWriter.MockedMessages.Count);
        }

        [TestMethod]
        public void Log_ConfiguredForConsoleAndMessages_ReceivingWarning_ShouldNotLogToConsole()
        {
            var jobLogger = JobLogger.GetInstance();
            var mockedConsoleWriter = new MockedConsoleWriter();

            var loggerConfiguration = new LoggerConfiguration(new List<ILogger> { new ConsoleLogger(mockedConsoleWriter) }, new List<LogLevel> { LogLevel.Message });
            jobLogger.Configure(loggerConfiguration);

            var warning = new Warning("This is the warning");
            jobLogger.Log(warning);

            Assert.IsFalse(mockedConsoleWriter.MockedMessages.Any());
        }

        [TestMethod]
        public void Log_ConfiguredForConsoleAndMessages_ReceivingError_ShouldNotLogToConsole()
        {
            var jobLogger = JobLogger.GetInstance();
            var mockedConsoleWriter = new MockedConsoleWriter();

            var loggerConfiguration = new LoggerConfiguration(new List<ILogger> { new ConsoleLogger(mockedConsoleWriter) }, new List<LogLevel> { LogLevel.Message });
            jobLogger.Configure(loggerConfiguration);

            var error = new Error("This is the error");
            jobLogger.Log(error);

            Assert.IsFalse(mockedConsoleWriter.MockedMessages.Any());
        }

        [TestMethod]
        public void Log_ConfiguredForConsoleAndWarnings_ReceivingMessage_ShouldNotLogToConsole()
        {
            var jobLogger = JobLogger.GetInstance();
            var mockedConsoleWriter = new MockedConsoleWriter();

            var loggerConfiguration = new LoggerConfiguration(new List<ILogger> { new ConsoleLogger(mockedConsoleWriter) }, new List<LogLevel> { LogLevel.Message });
            jobLogger.Configure(loggerConfiguration);

            var message = new Error("This is the message");
            jobLogger.Log(message);

            Assert.IsFalse(mockedConsoleWriter.MockedMessages.Any());
        }

        [TestMethod]
        public void Log_ConfiguredForConsoleWarningsAndErrors_ReceivingMessageWarningAndError_ShouldLogToConsoleOnlyWarningAndError()
        {
            var jobLogger = JobLogger.GetInstance();
            var mockedConsoleWriter = new MockedConsoleWriter();

            var loggerConfiguration = new LoggerConfiguration(new List<ILogger> { new ConsoleLogger(mockedConsoleWriter) }, new List<LogLevel> { LogLevel.Warning, LogLevel.Error });
            jobLogger.Configure(loggerConfiguration);

            var message = new Message("This is the message");
            jobLogger.Log(message);

            var warning = new Warning("This is the warning");
            jobLogger.Log(warning);

            var error = new Error("This is the error");
            jobLogger.Log(error);

            AssertConsoleContent(mockedConsoleWriter.MockedMessages, new List<ILogMessage> { warning, error });

            Assert.AreEqual(2, mockedConsoleWriter.MockedMessages.Count);
        }

        [TestMethod]
        public void Log_ConfiguredForConsoleMessagesAndErrors_ReceivingMessageWarningAndError_ShouldLogToConsoleOnlyMessageAndError()
        {
            var jobLogger = JobLogger.GetInstance();
            var mockedConsoleWriter = new MockedConsoleWriter();

            var loggerConfiguration = new LoggerConfiguration(new List<ILogger> { new ConsoleLogger(mockedConsoleWriter) }, new List<LogLevel> { LogLevel.Message, LogLevel.Error });
            jobLogger.Configure(loggerConfiguration);

            var message = new Message("This is the message");
            jobLogger.Log(message);

            var warning = new Warning("This is the warning");
            jobLogger.Log(warning);

            var error = new Error("This is the error");
            jobLogger.Log(error);

            AssertConsoleContent(mockedConsoleWriter.MockedMessages, new List<ILogMessage> { message, error });

            Assert.AreEqual(2, mockedConsoleWriter.MockedMessages.Count);
        }

        [TestMethod]
        public void Log_ConfiguredForConsoleMessagesAndWarnings_ReceivingMessageWarningAndError_ShouldLogToConsoleOnlyMessageAndWarning()
        {
            var jobLogger = JobLogger.GetInstance();
            var mockedConsoleWriter = new MockedConsoleWriter();

            var loggerConfiguration = new LoggerConfiguration(new List<ILogger> { new ConsoleLogger(mockedConsoleWriter) }, new List<LogLevel> { LogLevel.Message, LogLevel.Warning });
            jobLogger.Configure(loggerConfiguration);

            var message = new Message("This is the message");
            jobLogger.Log(message);

            var warning = new Warning("This is the warning");
            jobLogger.Log(warning);

            var error = new Error("This is the error");
            jobLogger.Log(error);

            AssertConsoleContent(mockedConsoleWriter.MockedMessages, new List<ILogMessage> { message, warning });

            Assert.AreEqual(2, mockedConsoleWriter.MockedMessages.Count);
        }

        #endregion

        #region Combined Loggers Configuration

        [TestMethod]
        public void Log_ConfiguredForFileAndDatabaseWithMessagesWarningsAndErrors_ShouldLogToFileAndDatabase()
        {
            var jobLogger = JobLogger.GetInstance();

            var loggerConfiguration = new LoggerConfiguration(new List<ILogger> { FileLogger.GetInstance(), new DatabaseLogger()  }, new List<LogLevel> { LogLevel.Message, LogLevel.Warning, LogLevel.Error });
            jobLogger.Configure(loggerConfiguration);

            var message = new Message("This is the message");
            jobLogger.Log(message);

            var warning = new Warning("This is the warning");
            jobLogger.Log(warning);

            var error = new Error("This is the error");
            jobLogger.Log(error);

            var loggedContent = File.ReadAllLines(_filePath);

            Assert.AreEqual(3, loggedContent.Count());
            AssertFileContent(loggedContent, new List<ILogMessage> { message, warning, error });

            var count = AssertDatabaseContent(new List<ILogMessage> { message, warning, error });

            Assert.AreEqual(3, count);
        }

        [TestMethod]
        public void Log_ConfiguredForFileAndConsoleWithMessagesWarningsAndErrors_ShouldLogToFileAndConsole()
        {
            var jobLogger = JobLogger.GetInstance();
            var mockedConsoleWriter = new MockedConsoleWriter();

            var loggerConfiguration = new LoggerConfiguration(new List<ILogger> { FileLogger.GetInstance(), new ConsoleLogger(mockedConsoleWriter) }, new List<LogLevel> { LogLevel.Message, LogLevel.Warning, LogLevel.Error });
            jobLogger.Configure(loggerConfiguration);

            var message = new Message("This is the message");
            jobLogger.Log(message);

            var warning = new Warning("This is the warning");
            jobLogger.Log(warning);

            var error = new Error("This is the error");
            jobLogger.Log(error);

            var loggedContent = File.ReadAllLines(_filePath);
            Assert.AreEqual(3, loggedContent.Count());

            AssertFileContent(loggedContent, new List<ILogMessage> { message, warning, error });

            AssertConsoleContent(mockedConsoleWriter.MockedMessages, new List<ILogMessage> { message, warning, error });
            Assert.AreEqual(3, mockedConsoleWriter.MockedMessages.Count);
        }

        [TestMethod]
        public void Log_ConfiguredForDatabaseAndConsoleWithMessagesWarningsAndErrors_ShouldLogToDatabaseAndConsole()
        {
            var jobLogger = JobLogger.GetInstance();
            var mockedConsoleWriter = new MockedConsoleWriter();

            var loggerConfiguration = new LoggerConfiguration(new List<ILogger> { new DatabaseLogger(), new ConsoleLogger(mockedConsoleWriter) }, new List<LogLevel> { LogLevel.Message, LogLevel.Warning, LogLevel.Error });
            jobLogger.Configure(loggerConfiguration);

            var message = new Message("This is the message");
            jobLogger.Log(message);

            var warning = new Warning("This is the warning");
            jobLogger.Log(warning);

            var error = new Error("This is the error");
            jobLogger.Log(error);

            var count = AssertDatabaseContent(new List<ILogMessage> { message, warning, error });
            Assert.AreEqual(3, count);

            AssertConsoleContent(mockedConsoleWriter.MockedMessages, new List<ILogMessage> { message, warning, error });
            Assert.AreEqual(3, mockedConsoleWriter.MockedMessages.Count);
        }

        [TestMethod]
        public void Log_ConfiguredForFileDatabaseAndConsoleWithMessagesWarningsAndErrors_ShouldLogToFileDatabaseAndConsole()
        {
            var jobLogger = JobLogger.GetInstance();
            var mockedConsoleWriter = new MockedConsoleWriter();

            var loggerConfiguration = new LoggerConfiguration(new List<ILogger> { FileLogger.GetInstance(), new DatabaseLogger(), new ConsoleLogger(mockedConsoleWriter) }, new List<LogLevel> { LogLevel.Message, LogLevel.Warning, LogLevel.Error });
            jobLogger.Configure(loggerConfiguration);

            var message = new Message("This is the message");
            jobLogger.Log(message);

            var warning = new Warning("This is the warning");
            jobLogger.Log(warning);

            var error = new Error("This is the error");
            jobLogger.Log(error);

            var loggedContent = File.ReadAllLines(_filePath);
            Assert.AreEqual(3, loggedContent.Count());

            var count = AssertDatabaseContent(new List<ILogMessage> { message, warning, error });
            Assert.AreEqual(3, count);

            AssertConsoleContent(mockedConsoleWriter.MockedMessages, new List<ILogMessage> { message, warning, error });
            Assert.AreEqual(3, mockedConsoleWriter.MockedMessages.Count);
        }

        #endregion
        private void AssertFileContent(string[] loggedContent, IList<ILogMessage> messages)
        {
            var index = 0;
            foreach (var content in loggedContent)
            {
                var contentArray = content.Split('|');
                Assert.AreEqual(messages[index].Id.ToString(), contentArray[0]);
                Assert.AreEqual(messages[index].Date.ToString(CultureInfo.InvariantCulture), contentArray[1]);
                Assert.AreEqual(messages[index].LogLevel.ToString(), contentArray[2]);
                Assert.AreEqual(messages[index].LogMessage, contentArray[3]);
                index++;
            }
        }

        private int AssertDatabaseContent(IList<ILogMessage> messages)
        {
            var connection = new SQLiteConnection(ConfigurationHelper.ConnectionString);
            connection.Open();

            var command = new SQLiteCommand("SELECT * FROM Logger", connection);
            var reader = command.ExecuteReader();

            var index = 0;
            while (reader.Read())
            {
                var logId = reader["Id"];
                var logDate = reader["LogDate"];
                var logLevel = reader["LogLevel"];
                var logMessage = reader["LogMessage"];

                Assert.AreEqual(messages[index].Id.ToString(), logId);
                Assert.AreEqual(messages[index].Date.ToString(), logDate);
                Assert.AreEqual(messages[index].LogLevel.ToString(), logLevel);
                Assert.AreEqual(messages[index].LogMessage, logMessage);
                index++;
            }

            connection.Close();

            return index;
        }

        private void AssertConsoleContent(IList<MockedMessage> loggedContent, IList<ILogMessage> messages)
        {
            var index = 0;
            foreach (var content in loggedContent)
            {
                Assert.AreEqual(messages[index].Id, content.Id);
                Assert.AreEqual(messages[index].Date.ToString(CultureInfo.InvariantCulture), content.Date.ToString(CultureInfo.InvariantCulture));
                Assert.AreEqual(messages[index].LogLevel, content.LogLevel);
                Assert.AreEqual(messages[index].LogMessage, content.LogMessage);
                Assert.AreEqual(GetExpectedForegroundColor(messages[index]), content.ForegroundColor);
                index++;
            }
        }

        private ConsoleColor GetExpectedForegroundColor(ILogMessage message)
        {
            switch (message.LogLevel)
            {
                case LogLevel.Message:
                    return ConsoleColor.White;
                case LogLevel.Warning:
                    return ConsoleColor.Yellow;
                case LogLevel.Error:
                    return ConsoleColor.Red;
            }

            throw new Exception("Unrecognized LogLevel");
        }
    }
}
