using DeployToProduction.WeatherForecast.Core.Models;
using DeployToProduction.WeatherForecast.Data;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace DeployToProduction.WeatherForecast.App.Pages;

public class IndexModel : PageModel
{
    private readonly ILogger<IndexModel> _logger;
    private readonly IForecast _forecast;

    public IndexModel(IForecast forecast, ILogger<IndexModel> logger)
    {
        _forecast = forecast;
        _logger = logger;
    }

    public IWeather? Weather { get; set; } = default;

    public async Task OnGetAsync(string? location = null)
    {
        if (!string.IsNullOrWhiteSpace(location))
        {
            Weather = await _forecast.PredictAsync(location);
        }
    }
}
