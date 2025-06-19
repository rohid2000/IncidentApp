using IncidentApp.Models;
using IncidentApp.Services;
using System.Collections.ObjectModel;

namespace IncidentApp;

public partial class UserReportedIncidentsPage : ContentPage
{
    public ObservableCollection<IncidentDataModel> Incidents { get; set; } = new ObservableCollection<IncidentDataModel>();

	public UserReportedIncidentsPage()
	{
		InitializeComponent();
        BindingContext = this;

        this.FillReportedIncidentsByUser();
    }

	private async void FillReportedIncidentsByUser()
	{
		var userId = UserStateService.user.Id;

        try
		{
			var response = await ApiService.GetIncidentsByUserId(userId);

            Incidents.Clear();

            foreach(var item in response)
            {
                Incidents.Add(item);
            }
		}
        catch (Exception ex)
        {
            await DisplayAlert("Error", $"Could not fetch Incidents: {ex.Message}", "OK");
        }
    }

	private async void AddReportedIncident(object sender, EventArgs e)
        {
            Location location = await LocationFetcher.GetCurrentLocation();

            var incident = new IncidentDataModel
            {
                Description = DescriptionEntry.Text,
                Status = "Gemeld",
                Location = $"{location.Latitude} {location.Longitude}",
                UserId = UserStateService.user?.Id ?? null
            };

            try
            {
                await ApiService.AddIncidentAsync(incident);

                DescriptionEntry.Text = string.Empty;

                await DisplayAlert("Success", "Incident saved!", "OK");

                this.FillReportedIncidentsByUser();
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", $"Failed to save: {ex.Message}", "OK");
            }
        }
}