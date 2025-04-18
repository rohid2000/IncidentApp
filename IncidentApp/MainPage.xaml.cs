using IncidentApp.Fetcher.Fetchers;
using IncidentApp.Models;
using System.Net.Http;
using System.Threading.Tasks;

namespace IncidentApp
{
    public partial class MainPage : ContentPage
    {
        private readonly ApiService _apiService;

        int count = 0;

        public MainPage(ApiService apiService)
        {
            InitializeComponent();
            _apiService = apiService;
        }

        private async void AddReportedIncident(object sender, EventArgs e)
        {
            var incident = new IncidentDataModel
            {
                Description = DescriptionEntry.Text
            };

            try
            {
                await _apiService.AddIncidentAsync(incident);

                DescriptionEntry.Text = string.Empty;

                await DisplayAlert("Success", "Incident saved!", "OK");
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", $"Failed to save: {ex.Message}", "OK");
            }
        }
    }
}
