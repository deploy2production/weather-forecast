using System.Reflection;

namespace DeployToProduction.WeatherForecast.Data.Psql
{
    public class Db
    {
        private readonly string _connectionString;

        public Db(string connectionString)
        {
            _connectionString = connectionString;
        }

        private void _waitDatabaseConnectionReady()
        {
            do
            {
                using var connection = new Npgsql.NpgsqlConnection(_connectionString);
                try
                {
                    connection.Open();
                    if (connection.State == System.Data.ConnectionState.Open)
                    {
                        break;
                    }
                    else
                    {
                        Thread.Sleep(500);
                    }
                }
                catch
                {
                    // wait and try again
                    Thread.Sleep(500);
                }
            }
            while (true);
        }

        public bool Setup()
        {
            var upgrader = DbUp.DeployChanges.To
                .PostgresqlDatabase(_connectionString)
                .WithScriptsEmbeddedInAssembly(Assembly.GetExecutingAssembly())
                .LogToConsole()
                .Build();

            _waitDatabaseConnectionReady();

            var result = upgrader.PerformUpgrade();

            return result.Successful;
        }
    }
}
