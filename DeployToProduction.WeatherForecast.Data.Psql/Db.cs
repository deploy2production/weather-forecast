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

        public bool Setup()
        {
            var upgrader = DbUp.DeployChanges.To
                .PostgresqlDatabase(_connectionString)
                .WithScriptsEmbeddedInAssembly(Assembly.GetExecutingAssembly())
                .LogToConsole()
                .Build();

            var result = upgrader.PerformUpgrade();

            return result.Successful;
        }
    }
}
