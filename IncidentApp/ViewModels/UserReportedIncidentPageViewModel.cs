using IncidentApp.Models;
using IncidentApp.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IncidentApp.ViewModels
{
    public class UserReportedIncidentPageViewModel : BaseViewModel
    {
        private List<IncidentDataModel> _incidents = [];

        public string _description;

        public Command AddIncidentAsyncCommand { get; }

        public UserReportedIncidentPageViewModel()
        {
            AddIncidentAsyncCommand = new Command(async () => await AddIncidentAsync());

            FillReportedIncidentsByUser();
        }

        public List<IncidentDataModel> Incidents
        {
            get => _incidents;
            set => SetProperty(ref _incidents, value);
        }

        public string Description
        {
            get => _description;
            set => SetProperty(ref _description, value);
        }

        protected async Task FillReportedIncidentsByUser()
        {
            var userId = UserStateService.user.Id;

            try
            {
                var response = await ApiService.GetIncidentsByUserId(userId);

                Incidents.Clear();

                foreach (var item in response)
                {
                    Incidents.Add(item);
                }

                Incidents = [..Incidents];
            }
            catch (Exception ex)
            {
                DisplayAlertService.ShowAlert("Error", $"Could not fetch Incidents: {ex.Message}", "No");
            }
        }

        private async Task AddIncidentAsync()
        {
            Location location = await LocationFetcher.GetCurrentLocation();

            var incident = new IncidentDataModel
            {
                Description = _description,
                Status = "Gemeld",
                Location = $"{location.Latitude} {location.Longitude}",
                UserId = UserStateService.user?.Id ?? null
            };

            try
            {
                await ApiService.AddIncidentAsync(incident);

                Description = string.Empty;

                await DisplayAlertService.ShowAlert("Success", "Incident reported!", "OK");

                await this.FillReportedIncidentsByUser();
            }
            catch (Exception ex)
            {
                await DisplayAlertService.ShowAlert("Error", $"Failed to save: {ex.Message}", "OK");
            }
        }
    }
}
