using DeployToProduction.WeatherForecast.Core.Models;
using StackExchange.Redis;
using System.Globalization;

namespace DeployToProduction.WeatherForecast.Data.Redis
{
    public class RedisWeather : IWeather
    {
        public RedisWeather(RedisValue value)
        {
            var array = value.ToString().Split(";");
            Date = DateTime.ParseExact(array[0], "yyyyMMdd", CultureInfo.CurrentCulture, DateTimeStyles.None);
            Location = array[1];
            Temperature = new Temperature(int.Parse(array[2]));
            Kind = WeatherKind.WeatherKinds[int.Parse(array[3])];
        }

        public RedisWeather(IWeather weather)
        {
            Date = weather.Date;
            Location = weather.Location;
            Temperature = weather.Temperature;
            Kind = weather.Kind;
        }

        public DateTime Date { get; set; }
        public string Location { get; set; }
        public Temperature Temperature { get; set; }
        public WeatherKind Kind { get; set; }

        public string ToRedisString()
        {
            return $"{Date:yyyyMMdd};{Location};{Temperature.Value};{Kind.Code}";
        }
    }
}