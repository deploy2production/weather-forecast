namespace DeployToProduction.WeatherForecast.Core.Models
{
    public class Temperature
    {
        public Temperature(int value)
        {
            Value = value;
        }

        public int Value { get; set; }

        public string Celsius()
        {
            return $"{Value} °C";
        }

        public string Fahrenheit()
        {
            return $"{Value * 9/5 + 32} °F";
        }
    }
}
