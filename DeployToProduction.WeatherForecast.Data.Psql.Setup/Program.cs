using DeployToProduction.WeatherForecast.Data.Psql;
using Microsoft.Extensions.Configuration;

var configuration = new ConfigurationBuilder()
    .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
    .AddEnvironmentVariables()
    .AddCommandLine(args)
    .Build();

var pgsqlConnectionString = configuration.GetConnectionString("Postgres")!;
new Db(pgsqlConnectionString).Setup();
