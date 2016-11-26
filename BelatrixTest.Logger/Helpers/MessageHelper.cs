using System.Globalization;
using BelatrixTest.Logger.Interfaces;

namespace BelatrixTest.Logger.Helpers
{
    public static class MessageHelper
    {
        public static string GetFormattedMessage(ILogMessage message)
        {
            return $"{message.Id}|{message.Date.ToString(CultureInfo.InvariantCulture)}|{message.LogLevel}|{message.LogMessage}";
        }
    }
}
