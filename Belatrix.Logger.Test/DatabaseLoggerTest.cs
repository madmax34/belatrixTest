using System.Collections.Generic;
using System.Data.SQLite;
using System.IO;
using BelatrixTest.Logger;
using BelatrixTest.Logger.Messages;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Belatrix.Logger.Test
{
    [TestClass]
    public class DatabaseLoggerTest
    {
        [ClassInitialize]
        public static void ClassInitialize(TestContext context)
        {
            if (File.Exists("DatabaseLoggerDB.db"))
            {
                File.Delete("DatabaseLoggerDB.db");
            }

            var connection = new SQLiteConnection(ConfigurationHelper.DatabaseLoggerConnectionString);
            connection.Open();

            var createCommand = new SQLiteCommand("CREATE TABLE Logger (Id UID, LogDate varchar(25), LogLevel varchar(10), LogMessage varchar(500))", connection);
            createCommand.ExecuteNonQuery();

            connection.Close();
        }

        [TestInitialize]
        public void Setup()
        {
            var connection = new SQLiteConnection(ConfigurationHelper.DatabaseLoggerConnectionString);
            connection.Open();

            var deleteCommand = new SQLiteCommand("DELETE FROM Logger", connection);
            deleteCommand.ExecuteNonQuery();
            
            connection.Close();
        }

        [TestMethod]
        public void Log_WithValidMessage_ShouldLogMessage()
        {
            var logger = new DatabaseLogger(ConfigurationHelper.DatabaseLoggerConnectionString);

            var message = new Message("This is the message");
            logger.Log(message);

            var connection = new SQLiteConnection(ConfigurationHelper.DatabaseLoggerConnectionString);
            connection.Open();

            var command = new SQLiteCommand("SELECT * FROM Logger", connection);
            var reader = command.ExecuteReader();

            while (reader.Read())
            {
                var logId = reader["Id"];
                var logDate = reader["LogDate"];
                var logLevel = reader["LogLevel"];
                var logMessage = reader["LogMessage"];

                Assert.AreEqual(message.Id.ToString(), logId);
                Assert.AreEqual(message.Date.ToString(), logDate);
                Assert.AreEqual(message.LogLevel.ToString(), logLevel);
                Assert.AreEqual(message.LogMessage, logMessage);
            }

            connection.Close();
        }

        [TestMethod]
        public void Log_WithMultipleMessages_ShouldLogMessage()
        {
            var logger = new DatabaseLogger(ConfigurationHelper.DatabaseLoggerConnectionString);

            var message = new Message("This is the message");
            logger.Log(message);

            var message2 = new Message("This is the second message");
            logger.Log(message2);

            var connection = new SQLiteConnection(ConfigurationHelper.DatabaseLoggerConnectionString);
            connection.Open();

            var command = new SQLiteCommand("SELECT * FROM Logger", connection);
            var reader = command.ExecuteReader();

            var messages = new List<Message>
            {
                message,
                message2
            };

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
        }

        [TestMethod]
        public void Log_WithValidWarning_ShouldLogMessage()
        {
            var logger = new DatabaseLogger(ConfigurationHelper.DatabaseLoggerConnectionString);

            var warning = new Warning("This is the message");
            logger.Log(warning);

            var connection = new SQLiteConnection(ConfigurationHelper.DatabaseLoggerConnectionString);
            connection.Open();

            var command = new SQLiteCommand("SELECT * FROM Logger", connection);
            var reader = command.ExecuteReader();

            while (reader.Read())
            {
                var logId = reader["Id"];
                var logDate = reader["LogDate"];
                var logLevel = reader["LogLevel"];
                var logMessage = reader["LogMessage"];

                Assert.AreEqual(warning.Id.ToString(), logId);
                Assert.AreEqual(warning.Date.ToString(), logDate);
                Assert.AreEqual(warning.LogLevel.ToString(), logLevel);
                Assert.AreEqual(warning.LogMessage, logMessage);
            }

            connection.Close();
        }

        [TestMethod]
        public void Log_WithMultipleWarnings_ShouldLogMessage()
        {
            var logger = new DatabaseLogger(ConfigurationHelper.DatabaseLoggerConnectionString);

            var warning = new Warning("This is the message");
            logger.Log(warning);

            var warning2 = new Warning("This is the second message");
            logger.Log(warning2);

            var connection = new SQLiteConnection(ConfigurationHelper.DatabaseLoggerConnectionString);
            connection.Open();

            var command = new SQLiteCommand("SELECT * FROM Logger", connection);
            var reader = command.ExecuteReader();

            var messages = new List<Warning>
            {
                warning,
                warning2
            };

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
        }

        [TestMethod]
        public void Log_WithValidError_ShouldLogMessage()
        {
            var logger = new DatabaseLogger(ConfigurationHelper.DatabaseLoggerConnectionString);

            var error = new Error("This is the message");
            logger.Log(error);

            var connection = new SQLiteConnection(ConfigurationHelper.DatabaseLoggerConnectionString);
            connection.Open();

            var command = new SQLiteCommand("SELECT * FROM Logger", connection);
            var reader = command.ExecuteReader();

            while (reader.Read())
            {
                var logId = reader["Id"];
                var logDate = reader["LogDate"];
                var logLevel = reader["LogLevel"];
                var logMessage = reader["LogMessage"];

                Assert.AreEqual(error.Id.ToString(), logId);
                Assert.AreEqual(error.Date.ToString(), logDate);
                Assert.AreEqual(error.LogLevel.ToString(), logLevel);
                Assert.AreEqual(error.LogMessage, logMessage);
            }

            connection.Close();
        }

        [TestMethod]
        public void Log_WithMultipleErrors_ShouldLogMessage()
        {
            var logger = new DatabaseLogger(ConfigurationHelper.DatabaseLoggerConnectionString);

            var error = new Error("This is the message");
            logger.Log(error);

            var error2 = new Error("This is the second message");
            logger.Log(error2);

            var connection = new SQLiteConnection(ConfigurationHelper.DatabaseLoggerConnectionString);
            connection.Open();

            var command = new SQLiteCommand("SELECT * FROM Logger", connection);
            var reader = command.ExecuteReader();

            var messages = new List<Error>
            {
                error,
                error2
            };

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
        }

        [TestMethod]
        [ExpectedException(typeof(SQLiteException))]
        public void Log_WithValidMessage_InvalidConnectionString_ShouldThrowAnException()
        {
            var logger = new DatabaseLogger("Data Source=FakeLoggerDB.db;");
            var message = new Message("This is the message");

            logger.Log(message);
        }
    }
}
