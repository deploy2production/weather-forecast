using DeployToProduction.Ads.Core.Models;
using Microsoft.AspNetCore.Mvc;

namespace DeployToProduction.Ads.WebApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AdsController : ControllerBase
    {
        private static readonly AdPost[] _adPosts = new[]
        {
            new AdPost {Content = "Московский клуб программистов. Общайся. Развивайся.", Href="https://prog.msk.ru/"},
            new AdPost {Content = "Сообщество программистов в VK.", Href = "https://vk.com/progmsk"},
            new AdPost {Content = "Лучшие видео по программированию", Href= "https://www.youtube.com/c/progmsk"},
        };

        private static readonly ISet<string> _locations = new HashSet<string> { "Moscow", "Москва" };

        private readonly ILogger<AdsController> _logger;

        public AdsController(ILogger<AdsController> logger)
        {
            _logger = logger;
        }

        [HttpPost]
        public Task<AdPost> Post(AdRequest request)
        {
            if (!_locations.Contains(request.Location))
            {
                return Task.FromResult(new AdPost { Content = "Лучший город для жизни - Москва!", Href = "https://www.mos.ru/" });
            }

            var index = new Random(DateTime.Now.Microsecond).Next(_adPosts.Length - 1);
            return Task.FromResult(_adPosts[index]);
        }
    }
}