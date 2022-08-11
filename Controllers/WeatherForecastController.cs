using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace EcommerseApplication.Controllers
{
    //[Authorize]
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
    };

        private readonly ILogger<WeatherForecastController> _logger;
        private readonly IHttpContextAccessor baseUrl;

        public WeatherForecastController(ILogger<WeatherForecastController> logger, IHttpContextAccessor baseUrl)
        {
            _logger = logger;
            this.baseUrl = baseUrl;
        }

        [HttpGet(Name = "GetWeatherForecast")]
        public IEnumerable<WeatherForecast> Get()
        {
            return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateTime.Now.AddDays(index),
                TemperatureC = Random.Shared.Next(-20, 55),
                Summary = Summaries[Random.Shared.Next(Summaries.Length)]
            })
            .ToArray();
        }
        [HttpGet("asd")]
        public string GetMe()
        {
            string userId = User?.FindFirstValue("UserId");
            var baseUrl2 = string.Format("{0}://{1}//", baseUrl.HttpContext.Request.Scheme, baseUrl.HttpContext.Request.Host.Value);

            //List<string> asf = new List<string>();
            //asf.Add( baseUrl.HttpContext.Request.Host.Value);
            //asf.Add( baseUrl.HttpContext.Request.PathBase.Value);
            //asf.Add( baseUrl.HttpContext.Request.Path.Value);
            //asf.Add( baseUrl.HttpContext.Request.Query.ToString());
            //asf.Add( baseUrl.HttpContext.Request.Scheme);
            //asf.Add( baseUrl.HttpContext.Request.Protocol);
            return baseUrl2;
        }
    }
}