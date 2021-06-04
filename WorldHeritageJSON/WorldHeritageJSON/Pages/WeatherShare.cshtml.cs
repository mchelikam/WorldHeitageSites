using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace WorldHeritageJSON.Pages
{
    public class WeatherShareModel : PageModel
    {
        public JsonResult OnGet()
        {
            string city = HttpContext.Request.Query["city"];

            string key = System.IO.File.ReadAllText("WeatherAPIKey.txt");

            string url = "https://api.weatherbit.io/v2.0/current?city=" + city + "&key=" + key;



            string weatherDetails = getData(url);



            WeatherShare.WeatherShareApi array = WeatherShare.WeatherShareApi.FromJson(weatherDetails);



            return new JsonResult(array);
        }



        private string getData(string url)
        {
            using (WebClient webClient = new WebClient())
            {
                return webClient.DownloadString(url);
            }
        }
    }
}
