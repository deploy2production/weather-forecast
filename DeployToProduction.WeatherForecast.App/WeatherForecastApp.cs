using DeployToProduction.WeatherForecast.Data;
using DeployToProduction.WeatherForecast.Data.Psql;
using DeployToProduction.WeatherForecast.Data.Redis;

namespace DeployToProduction.WeatherForecast.App
{
    public class WeatherForecastApp
    {

        public WebApplication Create(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            var pgsqlConnectionString = builder.Configuration.GetConnectionString("Postgres")!;
            var redisConnectionString = builder.Configuration.GetConnectionString("Redis")!;

            // Add services to the container.
            builder.Services.AddRazorPages();
            builder.Services.AddTransient((serviceProvider) =>
            {
                IForecast forecast = new PgsqlForecast(pgsqlConnectionString);
                forecast = new RedisForecast(forecast, redisConnectionString);
                return forecast;
            });

            var adsServerUrl = builder.Configuration.GetConnectionString("AdsServerUrl")!;
            builder.Services.AddTransient((serviceProvider) =>
            {
                IAdsService adsService = new AdsService(adsServerUrl);
                return adsService;
            });

            var app = builder.Build();

            if (app.Environment.IsDevelopment())
            {
                new Db(pgsqlConnectionString).Setup();
            }

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Error");
            }

            app.UseStaticFiles();

            app.UseRouting();

            app.MapRazorPages();

            return app;
        }
    }
}
