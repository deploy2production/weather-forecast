using DeployToProduction.WeatherForecast.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DeployToProduction.WeatherForecast.Data
{
    public interface IAdsService
    {
        Task<AdPost> RequestAsync(AdRequest request);
    }
}
