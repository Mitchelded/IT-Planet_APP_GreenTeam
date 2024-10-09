using MauiApp1.View;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using MauiApp1.Entities;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System.Net.Http.Json;

namespace MauiApp1;

public partial class PrewMainPage : ContentPage
{
    private string apiKey;
    private List<DayTask> tasks = new List<DayTask>();
    private CancellationTokenSource _cancelTokenSource;
    public static bool isOpen = false;
    private WeatherData weatherData;
    public PrewMainPage()
    {
        InitializeComponent();
    }

    public async void RefreshView()
    {
        try
        {
            using (AppContext db = new AppContext())
            {
                var today = DateTime.Now;
                db.Tasks.Load();
                db.TaskDecorations.Load();
                tasks = db.Tasks.Include(x => x.TaskDecorations).Where(x => x.DayToComplete.Date == today.Date).OrderBy(x => x.IsCompleted).ToList();
            }
            listView.ItemsSource = tasks;
            var total = tasks.Count > 0 ? tasks.Count : 0;
            lCountTasks.Text = $"{tasks.Where(x => x.IsCompleted == true).Count()}/{total} выполнено";
        }
        catch (Exception ex) { await DisplayAlert("Error", ex.Message, "Ok"); }
    }

    public void RefreshProgressBar()
    {
        try
        {
            double res;
            int completed;
            int total = tasks.Count();
            completed = tasks.Where(x => x.IsCompleted == true).Count();
            if (total == 0)
            {
                res = 0;
            }
            else
            {
                res = (double)completed / total;
            }
            progressBar.ProgressTo(res, 1000, Easing.CubicInOut);
            lCountTasks.Text = $"Прогресс {Math.Round(res * 100, 1)}% ({completed}/{total} выполнено)";
        }
        catch (Exception ex) { DisplayAlert("Error", ex.Message, "Ok"); }
    }

    private async void GetCurLocation()
    {
        try
        {

            GeolocationRequest request = new GeolocationRequest(GeolocationAccuracy.Medium, TimeSpan.FromSeconds(10));

            _cancelTokenSource = new CancellationTokenSource();

            Location location = await Geolocation.Default.GetLocationAsync(request, _cancelTokenSource.Token);

            if (location != null)
            {
                var tmp = await GetWeather(location.Latitude.ToString(), location.Longitude.ToString());
                weatherData = tmp;
                if (tmp == null)
                {
                    await DisplayAlert("Error", "Wheather API Error", "Ok");
                }
                else
                {
                    weather.Source = new Uri(tmp.Weather[0].Icon);
                    weatherlbl.Text = tmp.Weather[0].Main;
                    templbl.Text = $"{tmp.Main.Temp} °C";
                    desclbl.Text = tmp.Weather[0].Description;

                }
            }
        }
        catch (Exception ex) { await DisplayAlert("Error", ex.Message, "Ok"); }
    }


    public async Task<WeatherData> GetWeather(string lat, string lon)
    {
        try
        {

            HttpClient client = new HttpClient();
            using (AppContext db = new())
            {
                Options opt;
                if (db.Options.Any())
                {
                    opt = db.Options.First();
                    apiKey = opt.ApiKey;
                }
            }
            HttpResponseMessage response;
            if (string.IsNullOrEmpty(apiKey))
            {
                response = await client.GetAsync($"https://api.openweathermap.org/data/2.5/weather?lat={lat}&lon={lon}&APPID=da5930582d92824ee4bf933fac29240e");
            }
            else
            {
                response = await client.GetAsync($"https://api.openweathermap.org/data/2.5/weather?lat={lat}&lon={lon}&APPID={apiKey}");
            }

            if (response.IsSuccessStatusCode)
            {
                var data = await response.Content.ReadFromJsonAsync<WeatherData>();

                

                if (response != null)
                {
                    
                    return data;


                }
                else
                {
                    await DisplayAlert("Error", "Error", "OK");
                    return null;
                }

            }
            else
            {
                return null;
            }
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", ex.Message, "OK");
            return null;
        }
    }

    private async void DelTask(object sender, EventArgs e)
    {
        try
        {
            bool continueDel = await DisplayAlert("Подтверждение", "Вы действительно хотите удалить задачу", "Да", "Нет");
            if (continueDel == true)
            {
                var del = (sender as SwipeItem).BindingContext as DayTask;
                using (AppContext db = new())
                {
                    db.Tasks.Remove(del);
                    db.SaveChanges();
                }
                RefreshView();
                RefreshProgressBar();
            }
        }
        catch (Exception ex) { await DisplayAlert("Error", ex.Message, "Ok"); }
    }


    private void SetViewTemplateNavigation(object sender, NavigatedToEventArgs e)
    {
        RefreshView();
        RefreshProgressBar();
        if (weatherData==null)
            GetCurLocation();
        else
            weather.Source = new Uri(weatherData.Weather[0].Icon);


    }

    private void SetViewTemplate(object sender, EventArgs e)
    {
        GetCurLocation();
        RefreshView();
        RefreshProgressBar();
    }

    private async void CompleteTask(object sender, EventArgs e)
    {
        try
        {
            var row = (sender as SwipeItem).BindingContext as DayTask;
            row.IsCompleted = !row.IsCompleted;
            using (AppContext db = new AppContext())
            {
                db.Tasks.Update(row);
                db.SaveChanges();
            }
            RefreshProgressBar();
            RefreshView();
        }
        catch (Exception ex) { await DisplayAlert("Error", ex.Message, "Ok"); }
    }

    private void GoToFullTextEditor(object sender, TappedEventArgs e)
    {
        if (!isOpen)
        {
            isOpen = true;
            var t = ((sender as Label)).BindingContext as DayTask;
            Navigation.PushAsync(new Prew_Add(t));
        }
    }

    private async void GoToAddTaskPage(object sender, EventArgs e)
    {
        if (!isOpen)
        {
            isOpen = true;
            await Navigation.PushAsync(new Prew_Add(null));
        }
    }
}