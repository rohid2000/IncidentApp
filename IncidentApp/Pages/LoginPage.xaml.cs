using IncidentApp.Models;
using IncidentApp.Services;

namespace IncidentApp;

public partial class LoginPage : ContentPage
{
	public LoginPage()
	{
		InitializeComponent();
	}

	public async void Login(object sender, EventArgs e)
	{
		var user = new UserAdminDataModel
		{
			Username = UsernameEntry.Text,
			Password = PasswordEntry.Text
		};

        try
        {
            var response = await ApiService.TryAuthenticate(user);

            await DisplayAlert("Success", "User Logged in!", "OK");
			UserStateService.user = response;
			
			await Navigation.PushAsync(new UserReportedIncidentsPage());

        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", $"Failed to login: {ex.Message}", "OK");
        }
	}
}