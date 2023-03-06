using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace openweather
{
    public class openCall 
    {
        

        public static Report ReportIt = new Report("","");

        public static async Task OpenWeather(double lat, double lng)
        {

            // Key and URL Build
            string key = "3101f82226ed18424cb3d3077d5ffd37";
            string url = $"https://api.openweathermap.org/data/2.5/weather?lat={lat}&lon={lng}&appid={key}";


            // Build and Connect to OpenWeather API
            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Add("User-Agent", "Mozilla/5.0 (platform; rv:geckoversion) Gecko/geckotrail Firefox/firefoxversion");
            HttpResponseMessage response = await client.GetAsync(url);
            response.EnsureSuccessStatusCode();

            string content = await response.Content.ReadAsStringAsync();
            dynamic result = JsonConvert.DeserializeObject<dynamic>(content);

            ReportIt = new Report(result.main.temp, result.weather);


        }
        public openCall()
        {
            OpenWeather(44, -73.6);
        }
    }
}
