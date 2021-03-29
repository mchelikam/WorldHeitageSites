using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using QuickType;
using QuickTypeWeather;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Schema;


namespace WorldHeritageJSON.Pages
{
    
    public class Item
    {
       
        public double lat { get; set; }
        public double lon { get; set; }

        public string country { get; set; }

        public string name { get; set; }

        public double tem { get; set; }

    }
    public class WorldHeritageSitesModel : PageModel
    {
        
        public void OnGet()
        {
    
            using (var webClient = new WebClient())
            {

                string key=System.IO.File.ReadAllText("WeatherAPIKey.txt");
                

                String jsonString = webClient.DownloadString("https://examples.opendatasoft.com/api/records/1.0/search/?dataset=world-heritage-unesco-list&q=&rows=1052&facet=category&facet=country_en&facet=country_fr&facet=continent_en&facet=continent_fr");
                JSchema schema = JSchema.Parse(System.IO.File.ReadAllText("unesco.json"));
                JObject jsonObject = JObject.Parse(jsonString);
                QuickType.Welcome welcome = QuickType.Welcome.FromJson(jsonString);
                List<Item> items = new List<Item>();
                double t;
                for (int i = 0; i < 10; i++)
                {
                    Fields field = welcome.Records[i].Fields;
                    
                    {
                        string weatherString = webClient.DownloadString("https://api.weatherbit.io/v2.0/current?lat=" + field.Latitude + "&lon=" + field.Longitude + "&key=" + key + "&include=minutely");
                        //JSchema schema = JSchema.Parse(System.IO.File.ReadAllText("unesco.json"));
                        //JObject jsonObject = JObject.Parse(jsonString);
                        QuickTypeWeather.Welcome welcomeWeather = QuickTypeWeather.Welcome.FromJson(weatherString);
                        
                        t = welcomeWeather.Data[0].Temp;
                        //new Item{ lat=field.Latitude, lon= field.Longitude }
                        items.Add(new Item { lat = field.Latitude, lon = field.Longitude, country = field.CountryEn, name = field.NameEn, tem = t });

                };
                   
                    ViewData["Items"] = items;
                   
                    
                }
                
                
            }
        }
    }
}
