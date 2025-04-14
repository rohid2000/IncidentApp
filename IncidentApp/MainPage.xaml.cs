using IncidentApp.Fetcher.Fetchers;
using IncidentApp.Models;
using System.Net.Http;
using System.Threading.Tasks;

namespace IncidentApp
{
    public partial class MainPage : ContentPage
    {
        int count = 0;

        public MainPage()
        {
            InitializeComponent();
        }

        private void OnReportIncidentClicked(object sender, EventArgs e)
        {
            count++;

            if (count == 1)
            {
                ReportIncidentBtn.Text = "Incident gemeldt!";

                SemanticScreenReader.Announce(ReportIncidentBtn.Text);
            }
        }

        private async void AddReportedIncident(object sender, EventArgs e)
        {
            var data = new IncidentDataModel
            {
                Description = "test",
            };

            await ApiService.AddIncidentAsync(data);
        }
    }
}
