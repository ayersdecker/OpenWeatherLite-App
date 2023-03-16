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
        LoadingPage.IsVisible = true;
        RunningPage.IsVisible = false;
        // Get Data from OpenWeather API and Display
        Report report = await OpenWeather();
        tempText.Text = report.Temp;
        weatherText.Text = report.Weather;
        weatherIcon.Source = report.Icon;
        scrollBack.BackgroundColor = Color.FromHex(report.Color);
        altitudeText.Text = report.Altitude;
        currentText.Text = report.Current;
        windText.Text = report.Wind;
        LoadingPage.IsVisible = false;
        RunningPage.IsVisible = true;

        // Recursion to update every 1/2 hour
        //Handle();
        //Thread.Sleep(1800000); 
    }
    public static async Task<Report> OpenWeather()
    {
        // Get Location from device (Latitude and Longitude) using manifest
        var location = await Geolocation.GetLocationAsync();
        double latitude = location.Latitude;
        double longitude = location.Longitude;

        // Altitude Assignment
        string altitude = $"{location.Altitude:F2} m";
      
        // Key and URL Build
        string key = "3101f82226ed18424cb3d3077d5ffd37";
        string url = $"https://api.openweathermap.org/data/2.5/weather?lat={latitude}&lon={longitude}&appid={key}&units=imperial";

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

        // Get Weather Icon
        ImageSource icon = $"https://openweathermap.org/img/wn/{result.weather[0].icon.ToString()}.png";

        // Set Background Color
        string color;
        if(result.dt > result.sys.sunset || result.dt < result.sys.sunrise){color = "#666666";}
        else{color = "#48afff";}

        // Get Current
        string current = result.name;

        // Get Wind
        string wind = $"{result.wind.speed} mph";

        // Return Data
        return new Report(temp, weather, icon, color, altitude, current, wind);
    }

    private void Button_Clicked(object sender, EventArgs e)
    {
        Handle();
    }
}

