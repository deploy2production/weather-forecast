using DeployToProduction.WeatherForecast.Core.Models;

namespace DeployToProduction.WeatherForecast.Data.Psql
{
    public class DbWeather : IWeather
    {
        public DbWeather(IWeather weather)
        {
            Date = weather.Date;
            DbDate = weather.Date.Year * 10000 + weather.Date.Month * 100 + weather.Date.Day;
            Location = weather.Location;
            Temperature = weather.Temperature;
            DbTemperature = weather.Temperature.Value;
            Kind = weather.Kind;
            DbKind = weather.Kind.Code;
        }

        public DbWeather(int date, string location, int temperature, int kind)
        {
            DbDate = date;
            Location = location;
            DbTemperature = temperature;
            DbKind = kind;

            var year = date / 10000;
            var month = (date % 10000) / 100;
            var day = (date % 100);
            Date = new DateTime(year, month, day);
            Temperature = new Temperature(temperature);
            Kind = WeatherKind.WeatherKinds[kind];
        }

        public int DbDate { get; set; }
        public string Location { get; set; }
        public int DbTemperature { get; set; }
        public int DbKind { get; set; }
        public DateTime Date { get; set; }
        public Temperature Temperature { get; set; }
        public WeatherKind Kind { get; set; }
    }
}
