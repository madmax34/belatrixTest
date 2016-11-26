using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading;
using BelatrixTest.Logger;
using BelatrixTest.Logger.Messages;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Belatrix.Logger.Test
{
    [TestClass]
    public class FileLoggerTest
    {
        private string _filePath;

        [TestInitialize]
        public void Setup()
        {
            _filePath = Path.Combine(ConfigurationHelper.LogFileFolder, ConfigurationHelper.LogFileName);

            if (File.Exists(_filePath))
            {
                File.Delete(_filePath);
            }

            Thread.Sleep(500);
        }
        
        [TestMethod]
        public void Log_WithValidMessage_ShouldLogMessage()
        {
            var logger = FileLogger.GetInstance();
            
            var message = new Message("This is the message");
            logger.Log(message);

            var loggedContent = File.ReadAllLines(_filePath);

            Assert.AreEqual(1, loggedContent.Count());

            foreach (var content in loggedContent)
            {
                var contentArray = content.Split('|');
                Assert.AreEqual(message.Id.ToString(), contentArray[0]);
                Assert.AreEqual(message.Date.ToString(CultureInfo.InvariantCulture), contentArray[1]);
                Assert.AreEqual(message.LogLevel.ToString(), contentArray[2]);
                Assert.AreEqual(message.LogMessage, contentArray[3]);
            }
        }

        [TestMethod]
        public void Log_WithMultipleMessages_ShouldLogMessage()
        {
            var logger = FileLogger.GetInstance();

            var message1 = new Message("This is the message");
            logger.Log(message1);

            var message2 = new Message("This is the second message");
            logger.Log(message2);

            var loggedContent = File.ReadAllLines(_filePath);

            Assert.AreEqual(2, loggedContent.Count());

            var content = loggedContent[0];
            
            var contentArray = content.Split('|');
            Assert.AreEqual(message1.Id.ToString(), contentArray[0]);
            Assert.AreEqual(message1.Date.ToString(CultureInfo.InvariantCulture), contentArray[1]);
            Assert.AreEqual(message1.LogLevel.ToString(), contentArray[2]);
            Assert.AreEqual(message1.LogMessage, contentArray[3]);

            var content2 = loggedContent[1];

            var contentArray2 = content2.Split('|');
            Assert.AreEqual(message2.Id.ToString(), contentArray2[0]);
            Assert.AreEqual(message2.Date.ToString(CultureInfo.InvariantCulture), contentArray2[1]);
            Assert.AreEqual(message2.LogLevel.ToString(), contentArray2[2]);
            Assert.AreEqual(message2.LogMessage, contentArray2[3]);
        }

        [TestMethod]
        public void Log_WithValidWarning_ShouldLogMessage()
        {
            var logger = FileLogger.GetInstance();

            var warning = new Warning("This is the message");
            logger.Log(warning);

            var loggedContent = File.ReadAllLines(_filePath);

            Assert.AreEqual(1, loggedContent.Count());

            foreach (var content in loggedContent)
            {
                var contentArray = content.Split('|');
                Assert.AreEqual(warning.Id.ToString(), contentArray[0]);
                Assert.AreEqual(warning.Date.ToString(CultureInfo.InvariantCulture), contentArray[1]);
                Assert.AreEqual(warning.LogLevel.ToString(), contentArray[2]);
                Assert.AreEqual(warning.LogMessage, contentArray[3]);
            }
        }

        [TestMethod]
        public void Log_WithMultipleWarnings_ShouldLogMessage()
        {
            var logger = FileLogger.GetInstance();

            var warning1 = new Warning("This is the message");
            logger.Log(warning1);

            var warning2 = new Warning("This is the second message");
            logger.Log(warning2);

            var loggedContent = File.ReadAllLines(_filePath);

            Assert.AreEqual(2, loggedContent.Count());

            var content = loggedContent[0];

            var contentArray = content.Split('|');
            Assert.AreEqual(warning1.Id.ToString(), contentArray[0]);
            Assert.AreEqual(warning1.Date.ToString(CultureInfo.InvariantCulture), contentArray[1]);
            Assert.AreEqual(warning1.LogLevel.ToString(), contentArray[2]);
            Assert.AreEqual(warning1.LogMessage, contentArray[3]);

            var content2 = loggedContent[1];

            var contentArray2 = content2.Split('|');
            Assert.AreEqual(warning2.Id.ToString(), contentArray2[0]);
            Assert.AreEqual(warning2.Date.ToString(CultureInfo.InvariantCulture), contentArray2[1]);
            Assert.AreEqual(warning2.LogLevel.ToString(), contentArray2[2]);
            Assert.AreEqual(warning2.LogMessage, contentArray2[3]);
        }

        [TestMethod]
        public void Log_WithValidError_ShouldLogMessage()
        {
            var logger = FileLogger.GetInstance();

            var error = new Error("This is the message");
            logger.Log(error);

            var loggedContent = File.ReadAllLines(_filePath);

            Assert.AreEqual(1, loggedContent.Count());

            foreach (var content in loggedContent)
            {
                var contentArray = content.Split('|');
                Assert.AreEqual(error.Id.ToString(), contentArray[0]);
                Assert.AreEqual(error.Date.ToString(CultureInfo.InvariantCulture), contentArray[1]);
                Assert.AreEqual(error.LogLevel.ToString(), contentArray[2]);
                Assert.AreEqual(error.LogMessage, contentArray[3]);
            }
        }

        [TestMethod]
        public void Log_WithMultipleErrors_ShouldLogMessage()
        {
            var logger = FileLogger.GetInstance();

            var error1 = new Error("This is the message");
            logger.Log(error1);

            var error2 = new Error("This is the second message");
            logger.Log(error2);

            var loggedContent = File.ReadAllLines(_filePath);

            Assert.AreEqual(2, loggedContent.Count());

            var content = loggedContent[0];

            var contentArray = content.Split('|');
            Assert.AreEqual(error1.Id.ToString(), contentArray[0]);
            Assert.AreEqual(error1.Date.ToString(CultureInfo.InvariantCulture), contentArray[1]);
            Assert.AreEqual(error1.LogLevel.ToString(), contentArray[2]);
            Assert.AreEqual(error1.LogMessage, contentArray[3]);

            var content2 = loggedContent[1];

            var contentArray2 = content2.Split('|');
            Assert.AreEqual(error2.Id.ToString(), contentArray2[0]);
            Assert.AreEqual(error2.Date.ToString(CultureInfo.InvariantCulture), contentArray2[1]);
            Assert.AreEqual(error2.LogLevel.ToString(), contentArray2[2]);
            Assert.AreEqual(error2.LogMessage, contentArray2[3]);
        }
    }
}
