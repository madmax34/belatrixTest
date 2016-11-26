using System.Configuration;

namespace BelatrixTest.Logger
{
    public static class ConfigurationHelper
    {
        public static string LogFileFolder => ConfigurationManager.AppSettings["logFolder"];
        public static string LogFileName => ConfigurationManager.AppSettings["logFileName"];
        public static string ConnectionString => ConfigurationManager.ConnectionStrings["databaseConnectionString"].ConnectionString;
        public static string DatabaseLoggerConnectionString => ConfigurationManager.ConnectionStrings["databaseLoggerConnectionString"].ConnectionString;
    }
}
