namespace DeployToProduction.WeatherForecast.Data.Psql.IntegrationTests
{
    [TestClass]
    public class PgsqlForecastTests
    {
        [TestMethod]
        public async Task PredictAsync_ReturnTheSameWeather_OnRepeatLocation()
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
                new Db(container.ConnectionString).Setup();

                var forecast = new PgsqlForecast(container.ConnectionString);

                var weather1 = await forecast.PredictAsync("Moscow");
                var weather2 = await forecast.PredictAsync("Moscow");

                Assert.AreEqual(weather1.Temperature.Value, weather2.Temperature.Value);
                Assert.AreEqual(weather1.Kind.Code, weather2.Kind.Code);
            }
            finally
            {
                await container.DisposeAsync().ConfigureAwait(false);
            }
        }
    }
}