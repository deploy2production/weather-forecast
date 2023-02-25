namespace DeployToProduction.WeatherForecast.Core.Models
{
    public class RandomWeather : IWeather
    {
        public RandomWeather(DateTime date, string location)
        {
            Date = date;
            Location = location;
            var random = new Random(date.Millisecond + location.GetHashCode());
            Temperature = new Temperature(random.Next(101) - 50);
            Kind = WeatherKind.WeatherKinds[random.Next(4)];
        }

        public DateTime Date { get; set; }
        public string Location { get; set; }
        public Temperature Temperature { get; set; }
        public WeatherKind Kind { get; set; }
    }
}
