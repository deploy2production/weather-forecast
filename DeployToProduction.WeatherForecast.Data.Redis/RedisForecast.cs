using DeployToProduction.WeatherForecast.Core.Models;
using StackExchange.Redis;

namespace DeployToProduction.WeatherForecast.Data.Redis
{
    public class RedisForecast : IForecast
    {
        private readonly IForecast _origin;
        private readonly string _connectionString;

        public RedisForecast(IForecast origin, string connectionString)
        {
            _origin = origin;
            _connectionString = connectionString;
        }

        public async Task<IWeather> PredictAsync(string location)
        {
            var redis = ConnectionMultiplexer.Connect(_connectionString);
            try
            {
                var db = redis.GetDatabase();
                var now = DateTime.Now;
                var key = $"{now:yyyyMMdd}-{location}";
                RedisValue value = await db.StringGetAsync(key);

                IWeather weather;
                if (value.IsNullOrEmpty)
                {
                    weather = await _origin.PredictAsync(location);
                    var redisValue = new RedisWeather(weather);
                    await db.StringSetAsync(key, redisValue.ToRedisString());
                }
                else
                {
                    weather = new RedisWeather(value);
                }

                return weather;
            }
            finally
            {
                redis.Dispose();
            }
        }
    }
}