using IncidentApp.Fetcher.Fetchers;
using IncidentApp.Models;
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
            var incident = new IncidentDataModel
            {
                Description = DescriptionEntry.Text
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
    }
}
