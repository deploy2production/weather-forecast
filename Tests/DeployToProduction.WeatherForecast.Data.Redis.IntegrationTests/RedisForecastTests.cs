namespace DeployToProduction.WeatherForecast.Data.Redis.IntegrationTests
{
    [TestClass]
    public class RedisForecastTests
    {
        [TestMethod]
        public async Task PredictAsync_UseCache_OnRepeat()
        {
            var container = new ContainerBuilder()
                .WithImage("redis")
                .WithPortBinding(6380, 6379)
                .Build();

            await container.StartAsync().ConfigureAwait(false);
            try
            {
                var redisForecast = new RedisForecast(new RandomForecast(), "localhost:6380");
                var w1 = await redisForecast.PredictAsync("Test");
                var w2 = await redisForecast.PredictAsync("Test");

                Assert.IsTrue(w1.Temperature.Value == w2.Temperature.Value);
                Assert.IsTrue(w1.Kind.Code == w2.Kind.Code);
            }
            finally
            {
                await container.DisposeAsync().ConfigureAwait(false);
            }
        }
    }
}