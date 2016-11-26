using System.Data.SQLite;
using BelatrixTest.Logger.Interfaces;

namespace BelatrixTest.Logger
{
    public class DatabaseLogger : ILogger
    {
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

            var command = new SQLiteCommand("Insert into Logger (Id, LogDate, LogLevel, LogMessage) VALUES ('" + message.Id + "', '" + message.Date + "', '" + message.LogLevel + "', '" + message.LogMessage + "')", connection);
            command.ExecuteNonQuery();

            connection.Close();
        }
    }
}