using Newtonsoft.Json;

namespace openweather;

public partial class MainPage : ContentPage
{

	public MainPage()
	{
        
        Shell.SetNavBarIsVisible(this, false);
		InitializeComponent();
        Handle();
        
    }
    private async void Handle()
    {
        Report report = await OpenWeather(44, -73.6);
        tempText.Text = report.Temp;
        weatherText.Text = report.Weather;
        weatherIcon.Source = report.Icon;
        scrollBack.BackgroundColor = Color.FromHex(report.Color);

    }
    public static async Task<Report> OpenWeather(double lat, double lng)
    {
        string color;
        // Key and URL Build
        string key = "3101f82226ed18424cb3d3077d5ffd37";
        string url = $"https://api.openweathermap.org/data/2.5/weather?lat={lat}&lon={lng}&appid={key}&units=imperial";


        // Build and Connect to OpenWeather API
        HttpClient client = new HttpClient();
        client.DefaultRequestHeaders.Add("User-Agent", "Mozilla/5.0 (platform; rv:geckoversion) Gecko/geckotrail Firefox/firefoxversion");
        HttpResponseMessage response = await client.GetAsync(url);
        response.EnsureSuccessStatusCode();

        // Get JSON Data
        string content = await response.Content.ReadAsStringAsync();
        dynamic result = JsonConvert.DeserializeObject<dynamic>(content);
        string temp = result.main.temp.ToString() + " F";
        string weather = result.weather[0].main.ToString();
        ImageSource icon = $"https://openweathermap.org/img/wn/{result.weather[0].icon.ToString()}.png";

        if(result.dt > result.sys.sunset)
        {
            color = "#666666";
        }
        else
        {
            color = "#48afff";
        }

        // Return Data
        return new Report(temp, weather, icon, color);


    }
}

