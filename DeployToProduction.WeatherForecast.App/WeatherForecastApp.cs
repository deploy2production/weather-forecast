using DeployToProduction.WeatherForecast.Data.Psql;
using DeployToProduction.WeatherForecast.Data;
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
            builder.Services.AddTransient<IForecast>((serviceProvider) =>
            {
                IForecast forecast = new PgsqlForecast(pgsqlConnectionString);
                forecast = new RedisForecast(forecast, redisConnectionString);
                return forecast;
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
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseStaticFiles();

            app.UseRouting();

            app.MapRazorPages();

            return app;
        }
    }
}
