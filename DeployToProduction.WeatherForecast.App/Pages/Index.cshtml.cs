using DeployToProduction.WeatherForecast.Core.Models;
using DeployToProduction.WeatherForecast.Data;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace DeployToProduction.WeatherForecast.App.Pages;

public class IndexModel : PageModel
{
    private readonly ILogger<IndexModel> _logger;
    private readonly IForecast _forecast;
    private readonly IAdsService _adsService;

    public IndexModel(IForecast forecast, IAdsService adsService, ILogger<IndexModel> logger)
    {
        _forecast = forecast;
        _adsService = adsService;
        _logger = logger;
    }

    public IWeather? Weather { get; set; } = default;

    public AdPost? AdPost { get; set; }

    public async Task OnGetAsync(string? location = null)
    {
        if (!string.IsNullOrWhiteSpace(location))
        {
            try
            {
                Weather = await _forecast.PredictAsync(location);

                AdPost = await _adsService.RequestAsync(new AdRequest { Location = location });
            }
            catch (Exception)
            {
                // do nothing
            }
        }
    }
}
