using IncidentApp.Models;
using IncidentApp.Services;

namespace IncidentApp;

public partial class UserReportedIncidentsPage : ContentPage
{
	public UserReportedIncidentsPage()
	{
		InitializeComponent();
	}

	private async void GetReportedIncidentsByUser(object sender, EventArgs e)
	{
		try
		{
			var response = await ApiService.GetIncidentAsync();
		}
        catch (Exception ex)
        {
            await DisplayAlert("Error", $"Could not fetch Incidents: {ex.Message}", "OK");
        }
    }
}