namespace DeployToProduction.WeatherForecast.Data.Psql.IntegrationTests
{
    [TestClass]
    public class DbTests
    {
        [TestMethod]
        public async Task Setup_Success_Migration()
        {
            var userName = "postgres";
            var password = "postgres";

            var containerName = Guid.NewGuid().ToString("D");

            var container = new TestcontainersBuilder<PostgreSqlTestcontainer>()
                .WithName(containerName)
                .WithDatabase(new PostgreSqlTestcontainerConfiguration
                {
                    Database = "weather",
                    Username = userName,
                    Password = password,
                    Port = 5432
                })
                .Build();

            await container.StartAsync().ConfigureAwait(false);

            try
            {
                var db = new Db(container.ConnectionString);

                var success = db.Setup();

                Assert.IsTrue(success);
            }
            finally
            {
                await container.DisposeAsync().ConfigureAwait(false);
            }
        }
    }
}