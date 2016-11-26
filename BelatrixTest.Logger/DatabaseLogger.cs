using System.Data.SQLite;
using BelatrixTest.Logger.Interfaces;

namespace BelatrixTest.Logger
{
    public class DatabaseLogger : ILogger
    {
        private const string InsertTemplate = "Insert into Logger (Id, LogDate, LogLevel, LogMessage) VALUES ('{0}', '{1}', '{2}', '{3}')";
        private readonly string _connectionString;

        public DatabaseLogger()
        {
            _connectionString = ConfigurationHelper.ConnectionString;
        }

        public DatabaseLogger(string connectionString)
        {
            _connectionString = connectionString;
        }

        public void Log(ILogMessage message)
        {
            var connection = new SQLiteConnection(_connectionString);
            connection.Open();

            var commandText = string.Format(InsertTemplate, message.Id, message.Date, message.LogLevel, message.LogMessage);
            var command = new SQLiteCommand(commandText, connection);
            command.ExecuteNonQuery();

            connection.Close();
        }
    }
}