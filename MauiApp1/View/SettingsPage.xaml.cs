using MauiApp1.Entities;

namespace MauiApp1.View;

public partial class SettingsPage : ContentPage
{
	public SettingsPage()
	{
		InitializeComponent();
	}

	public async void ChangeApiKey(object sender, EventArgs e)
	{
		try
		{
			if (ApiKeyEntry.Text.Trim() != string.Empty)
			{
                using (AppContext db = new())
                {
                    Options opt;
                    
                    if (db.Options.Any())
                    {
                        opt = db.Options.First();
                        db.Options.Add(new Entities.Options() { ApiKey = ApiKeyEntry.Text });
                        db.SaveChanges();
                    }
                    else
                    {
                        opt = new();
                        opt.ApiKey = ApiKeyEntry.Text;
                        db.Options.Update(opt);
                        db.SaveChanges();
                    }
                }
            }
        }
		catch (Exception ex) { await DisplayAlert("Error", ex.Message, "Ok"); }

    }
}