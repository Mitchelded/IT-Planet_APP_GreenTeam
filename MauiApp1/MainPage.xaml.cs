using Newtonsoft.Json.Linq;
using Microsoft.EntityFrameworkCore;
using MauiApp1.Entities;
using MauiApp1;
namespace MauiApp1
{
    public partial class MainPage : ContentPage
    {


        public MainPage()
        {
            InitializeComponent();
        }






        public async Task<string> GetData(string city)
        {
            try
            {

                HttpClient client = new();

                HttpResponseMessage response = await client.GetAsync($"https://api.weatherapi.com/v1/current.json?key=96b19124cdd04d8194894711241903&q={city}&aqi=no");


                if (response.IsSuccessStatusCode)
                {
                    return await response.Content.ReadAsStringAsync();
                }
                else
                {
                    return null;
                }
            }
            catch (Exception exp)
            {
                await DisplayAlert("Error", exp.ToString(), "OK");
                return null;
            }
        }


        public async Task<(int?, string, string, string, double?, string)> GetWeather(string lat, string lon)
        {
            try
            {

                HttpClient client = new();

                HttpResponseMessage response = await client.GetAsync($"https://api.openweathermap.org/data/2.5/weather?lat={lat}&lon={lon}&APPID=da5930582d92824ee4bf933fac29240e");


                if (response.IsSuccessStatusCode)
                {
                    string data = await response.Content.ReadAsStringAsync();

                    if (data != null)
                    {
                        var json_data = JObject.Parse(data);
                        string image_code = json_data["weather"][0]["icon"].ToString();
                        string image_url = $"https://openweathermap.org/img/wn/{image_code}@4x.png";
                        string name_weather = json_data["weather"][0]["main"].ToString();
                        string description_weather = json_data["weather"][0]["description"].ToString();
                        string city = json_data["timezone"]["name"].ToString();
                        int temp = (int)json_data["main"]["temp"];
                        double speed_wind = (double)json_data["wind"]["speed"];

                        return (temp, image_url, name_weather, description_weather, speed_wind, city);


                    }
                    else
                    {
                        await DisplayAlert("Error", "Error", "OK");
                        return (null, null, null, null, null, null);
                    }

                }
                else
                {
                    return (null, null, null, null, null, null);
                }
            }
            catch (Exception exp)
            {
                await DisplayAlert("Error", exp.ToString(), "OK");
                return (null, null, null, null, null, null);
            }
        }


        private async void OnCounterClicked(object sender, EventArgs e)
        {

            var (temp, image_url, name_weather, description_weather, speed_wind, city) = await GetWeather("37.42", "122.08");
            if (temp != null && image_url != null)
            {

                image.Source = new UriImageSource
                {
                    Uri = new Uri(image_url),
                };
                label.Text = temp.ToString();
                //var progress = await DisplayPromptAsync("progress", "Введите progress:", "OK", "Отмена");
                //progressBar.Progress = double.Parse(progress) / 100;


            }
            else
            {
                await DisplayAlert("Error", "Error", "OK");
            }
        }

        private async void OnAddClicked(object sender, EventArgs e)
        {
            await Navigation.PushModalAsync(new PrewMainPage());
        }



        private void SwipeGestureRecognizer_Swiped(object sender, SwipedEventArgs e)
        {
            var item = sender as Label;
            item.TranslateTo(100, 0, 500);
            DisplayAlert("Task", "Swipe", "OK");
        }

        private void CounterBtn_Clicked2(object sender, EventArgs e)
        {
            
        }
    }
}