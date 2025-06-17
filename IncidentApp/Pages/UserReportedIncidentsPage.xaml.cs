using IncidentApp.Models;
using IncidentApp.Services;
using System.Security.Cryptography.X509Certificates;

namespace IncidentApp;

public partial class UserReportedIncidentsPage : ContentPage
{
	public List<IncidentDataModel> Incidents = new List<IncidentDataModel>();
	public UserReportedIncidentsPage()
	{
		InitializeComponent();
	}

	private async void GetReportedIncidentsByUser(object sender, EventArgs e)
	{
		var userId = UserStateService.user.Id;

		try
		{
			var response = await ApiService.GetIncidentByUserId(userId);

			Incidents = response;
		}
        catch (Exception ex)
        {
            await DisplayAlert("Error", $"Could not fetch Incidents: {ex.Message}", "OK");
        }
    }
}