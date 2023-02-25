using DeployToProduction.WeatherForecast.Core.Models;

namespace DeployToProduction.WeatherForecast.Data
{
    public interface IForecast
    {
        Task<IWeather> PredictAsync(string location);
    }
}