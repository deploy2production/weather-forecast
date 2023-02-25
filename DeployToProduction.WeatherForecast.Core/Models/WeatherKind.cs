namespace DeployToProduction.WeatherForecast.Core.Models
{
    public class WeatherKind
    {
        public static readonly WeatherKind[] WeatherKinds = new[]
        {
            new WeatherKind(0, "Ясно"),
            new WeatherKind(1, "Облачно"),
            new WeatherKind(2, "Пасмурно"),
            new WeatherKind(3, "Осадки")
        };

        public WeatherKind(int code, string description)
        {
            Code = code;
            Description = description;
        }

        public int Code { get; set; }
        public string Description { get; set; }
    }
}
