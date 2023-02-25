using DeployToProduction.WeatherForecast.Core.Models;

namespace DeployToProduction.WeatherForecast.Data.Psql
{
    public class PgsqlForecast : IForecast
    {
        private readonly string _connectionString;

        public PgsqlForecast(string connectionString)
        {
            _connectionString = connectionString;
        }

        public async Task<IWeather> PredictAsync(string location)
        {
            IWeather weather = new RandomWeather(DateTime.Now, location);
            var dbWeather = new DbWeather(weather);

            using var connection = new Npgsql.NpgsqlConnection(_connectionString);
            await connection.OpenAsync();

            try
            {

                using (var checkCommand = connection.CreateCommand())
                {
                    checkCommand.CommandText = @"select date, location, temperature, kind from weather where date=@date and location=@location";
                    checkCommand.Parameters.Add(new("date", dbWeather.DbDate));
                    checkCommand.Parameters.Add(new("location", dbWeather.Location));
                    using var reader = await checkCommand.ExecuteReaderAsync();
                    if (reader?.HasRows == true)
                    {
                        var readSuccess = await reader.ReadAsync();
                        if (!readSuccess)
                        {
                            return weather;
                        }
                        var date = reader.GetInt32(0);
                        var loc = reader.GetString(1);
                        var temp = reader.GetInt32(2);
                        var kind = reader.GetInt32(3);

                        return new DbWeather(date, loc, temp, kind);
                    }
                }

                using (var insertCommand = connection.CreateCommand())
                {
                    insertCommand.CommandText = "insert into weather(date, location, temperature, kind) values(@date, @location, @temperature, @kind)";
                    insertCommand.Parameters.Add(new("date", dbWeather.DbDate));
                    insertCommand.Parameters.Add(new("location", dbWeather.Location));
                    insertCommand.Parameters.Add(new("temperature", dbWeather.DbTemperature));
                    insertCommand.Parameters.Add(new("kind", dbWeather.DbKind));

                    await insertCommand.ExecuteNonQueryAsync();
                }
                return dbWeather;
            }
            catch (Exception)
            {
                return weather;
            }
        }
    }
}
