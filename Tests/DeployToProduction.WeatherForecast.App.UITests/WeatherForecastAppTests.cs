using OpenQA.Selenium;
using OpenQA.Selenium.Edge;
using System.Net;

namespace DeployToProduction.WeatherForecast.App.UITests
{
    [TestClass]
    public class WeatherForecastAppTests
    {
        [TestMethod]
        public async Task Generate_RandomWeather()
        {
            var network = new NetworkBuilder()
              .WithName(Guid.NewGuid().ToString("D"))
              .Build();

            await network.CreateAsync().ConfigureAwait(false);

            var pgsqlContainer = new ContainerBuilder()
                .WithImage("postgres")
                .WithName("test-weather-forecast-postgres")
                .WithNetwork(network)
                .WithPortBinding(5432, 5432)
                .WithEnvironment("POSTGRES_PASSWORD", "postgres")
                .Build();

            await pgsqlContainer.StartAsync().ConfigureAwait(false);

            var redisContainer = new ContainerBuilder()
                .WithImage("redis")
                .WithName("test-weather-forecast-redis")
                .WithNetwork(network)
                .WithPortBinding(6379, 6379)
                .Build();

            await redisContainer.StartAsync().ConfigureAwait(false);

            var adsContainer = new ContainerBuilder()
                .WithImage("weather-forecast-ads-img")
                .WithName("test-weather-forecast-ads")
                .WithNetwork(network)
                .WithPortBinding(80, true)
                .WithEnvironment("ASPNETCORE_ENVIRONMENT", "Development")
                .WithEnvironment("DOTNET_PRINT_TELEMETRY_MESSAGE", "false")
                .WithWaitStrategy(
                    Wait.ForUnixContainer()
                        .UntilHttpRequestIsSucceeded(
                            request => request.ForPath("/health").ForStatusCode(HttpStatusCode.OK)
                        )
                )
                .Build();

            await adsContainer.StartAsync().ConfigureAwait(false);

            var appContainer = new ContainerBuilder()
                .WithImage("weather-forecast-app-img")
                .WithName("test-weather-forecast-app")
                .WithNetwork(network)
                .WithPortBinding(80, true)
                .WithEnvironment("ASPNETCORE_ENVIRONMENT", "Development")
                .WithEnvironment("DOTNET_PRINT_TELEMETRY_MESSAGE", "false")
                .WithEnvironment(
                    "ConnectionStrings__Postgres",
                    "Host=test-weather-forecast-postgres;Username=postgres;Password=postgres;Database=postgres"
                )
                .WithEnvironment(
                    "ConnectionStrings__Redis",
                    "test-weather-forecast-redis:6379"
                )
                .WithEnvironment(
                    "ConnectionStrings__AdsServerUrl",
                    "http://test-weather-forecast-ads"
                )
                .Build();

            await appContainer.StartAsync().ConfigureAwait(false);

            var driver = new EdgeDriver();
            try
            {
                driver.Navigate().GoToUrl($"http://localhost:{appContainer.GetMappedPublicPort(80)}/");

                driver.FindElement(By.Id("location")).SendKeys("Moscow");
                driver.FindElement(By.Id("submit_btn")).Click();

                var forecastBlock = driver.FindElement(By.Id("forecast_block"));

                Assert.IsNotNull(forecastBlock);

                var adsBlock = driver.FindElement(By.Id("ads_block"));

                Assert.IsNotNull(adsBlock);
            }
            finally
            {
                driver.Quit();
                driver.Dispose();

                await appContainer.DisposeAsync().ConfigureAwait(false);
                await adsContainer.DisposeAsync().ConfigureAwait(false);
                await pgsqlContainer.DisposeAsync().ConfigureAwait(false);
                await redisContainer.DisposeAsync().ConfigureAwait(false);
                await network.DeleteAsync().ConfigureAwait(false);
            }
        }
    }
}