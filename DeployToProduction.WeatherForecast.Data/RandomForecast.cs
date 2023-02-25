using DeployToProduction.WeatherForecast.Core.Models;

namespace DeployToProduction.WeatherForecast.Data
{
    public class RandomForecast : IForecast
    {
        public Task<IWeather> PredictAsync(string location)
        {
            IWeather weather = new RandomWeather(DateTime.Now, location);
            return Task.FromResult(weather);
        }
    }
}