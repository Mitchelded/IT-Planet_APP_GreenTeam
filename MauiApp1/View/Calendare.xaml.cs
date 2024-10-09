namespace MauiApp1.View;
using MauiApp1.Entities;
using Microsoft.EntityFrameworkCore;
public partial class Calendare : ContentPage
{
	public Calendare()
	{
		InitializeComponent();
    }

    private async void SetupCalendar(object sender, NavigatedToEventArgs e)
    {
        try
        {
            using (AppContext db = new())
            {
                var events = new Plugin.Maui.Calendar.Models.EventCollection();
                var eventsFromDB = db.Tasks.Include(x => x.TaskDecorations).OrderBy(x => x.DayToComplete).AsQueryable().ToList();
                var bind = EventCalendar.BindingContext as List<DayTask>;
                if (eventsFromDB.Any())
                {
                    if (bind == null || bind != eventsFromDB)
                    {
                        for (DateTime i = eventsFromDB[0].DayToComplete; i <= eventsFromDB.Last().DayToComplete; i = i.AddDays(1))
                        {
                            var thisDayEvents = eventsFromDB.Where(x => x.DayToComplete == i).ToList();
                            if (thisDayEvents.Count > 0 && !events.ContainsKey(i))
                            {
                                events.Add(i, thisDayEvents);
                            }
                            else
                            {
                                continue;
                            }
                        }
                        EventCalendar.Events = events;
                        EventCalendar.BindingContext = events.Values.ToList();
                    }
                }
            }
        }
        catch (Exception ex) { await DisplayAlert("Error", ex.Message, "Ok"); }
    }

}