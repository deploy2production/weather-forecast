using DotNet.Testcontainers.Builders;
using System.Net;
using System.Net.Http.Json;

namespace DeployToProduction.Ads.WebApi.IntegrationTests
{
    [TestClass]
    public class AdsWebApiTests
    {
        [TestMethod]
        public async Task PostAdsController_ReturnAds()
        {
            var adsContainer = new ContainerBuilder()
                .WithImage("weather-forecast-ads-img")
                .WithName("test-weather-forecast-ads")
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

            try
            {
                var baseAddress = new Uri($"http://localhost:{adsContainer.GetMappedPublicPort(80)}/");
                var http = new HttpClient
                {
                    BaseAddress = baseAddress
                };
                var response = await http.PostAsJsonAsync("ads", new { Location = "Moscow" });
                response.EnsureSuccessStatusCode();
                var content = await response.Content.ReadAsStringAsync();

                Assert.IsFalse(string.IsNullOrEmpty(content));
            }
            finally
            {
                await adsContainer.DisposeAsync().ConfigureAwait(false);
            }
        }
    }
}