using DeployToProduction.WeatherForecast.Core.Models;
using System.Net.Http.Json;
using System.Runtime.Serialization;

namespace DeployToProduction.WeatherForecast.Data
{
    public class AdsService : IAdsService
    {
        private readonly HttpClient _httpClient;

        public AdsService(string adsServerUrl)
        {
            _httpClient = new HttpClient()
            {
                BaseAddress = new Uri(adsServerUrl),
                Timeout = TimeSpan.FromMilliseconds(300)
            };
        }

        public async Task<AdPost> RequestAsync(AdRequest request)
        {
            AdPost? post;
            try
            {
                var respose = await _httpClient.PostAsJsonAsync("ads", request);
                respose.EnsureSuccessStatusCode();
                post = await respose.Content.ReadFromJsonAsync<AdPost>();
            }
            catch (Exception ex)
            {
                throw new AdsServiceException("Ads error", ex);
            }

            if (post == null)
            {
                throw new AdsServiceException("No ads");
            }

            return post;
        }
    }

    [Serializable]
    public class AdsServiceException : Exception
    {
        public AdsServiceException()
        {
        }

        public AdsServiceException(string? message) : base(message)
        {
        }

        public AdsServiceException(string? message, Exception? innerException) : base(message, innerException)
        {
        }

        protected AdsServiceException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
