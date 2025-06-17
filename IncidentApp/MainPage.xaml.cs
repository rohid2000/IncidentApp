using IncidentApp.Models;
using IncidentApp.Services;
using System.Net.Http;
using System.Threading.Tasks;

namespace IncidentApp
{
    public partial class MainPage : ContentPage
    {

        public MainPage()
        {
            InitializeComponent();
        }

        private async void AddReportedIncident(object sender, EventArgs e)
        {
            Location location = await LocationFetcher.GetCurrentLocation();

            var incident = new IncidentDataModel
            {
                Description = DescriptionEntry.Text,
                Location = $"{location.Latitude} {location.Longitude}",
                UserId = UserStateService.user?.Id ?? null
            };

            try
            {
                await ApiService.AddIncidentAsync(incident);

                DescriptionEntry.Text = string.Empty;

                await DisplayAlert("Success", "Incident saved!", "OK");
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", $"Failed to save: {ex.Message}", "OK");
            }
        }

        private async void NavigateToRegisterPage(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new RegisterPage());
        }

        private async void NavigateToLoginPage(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new LoginPage());
        }
    }
}
