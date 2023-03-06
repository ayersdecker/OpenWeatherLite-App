using Newtonsoft.Json;

namespace openweather;

public partial class MainPage : ContentPage
{

	public MainPage()
	{
        

		InitializeComponent();
        Handle();
        
    }
    private async void Handle()
    {
        Report report = await OpenWeather(44, -73.6);
        tempText.Text = report.Temp;
        weatherText.Text = report.Weather;

    }
    public static async Task<Report> OpenWeather(double lat, double lng)
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
        string temp = result.main.temp.ToString();
        string weather = result.weather[0].main.ToString();

        return new Report(temp, weather);


    }
}

