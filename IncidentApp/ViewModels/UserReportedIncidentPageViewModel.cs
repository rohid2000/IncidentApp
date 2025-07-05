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
        private ApiService _apiService;
        private LocationFetcher _locationFetcher;
        private DisplayAlertService _displayAlertService;
        private UserStateService _userStateService;

        private List<IncidentDataModel> _incidents = [];

        public string _description;

        public Command AddIncidentAsyncCommand { get; }

        public UserReportedIncidentPageViewModel()
        {
            _apiService = new ApiService();
            _locationFetcher = new LocationFetcher();
            _displayAlertService = new DisplayAlertService();
            _userStateService = new UserStateService();

            AddIncidentAsyncCommand = new Command(async () => await AddIncidentAsync());

            FillReportedIncidentsByUser();
        }

        public UserReportedIncidentPageViewModel(
            ApiService apiService,
            LocationFetcher locationFetcher,
            DisplayAlertService displayAlertService,
            UserStateService userStateService)

        {
            _apiService = apiService;
            _locationFetcher = locationFetcher;
            _displayAlertService = displayAlertService;
            _userStateService = userStateService;
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

        public async Task FillReportedIncidentsByUser()
        {
            var userId = _userStateService.user.Id;

            try
            {
                var response = await _apiService.GetIncidentsByUserId(userId);

                Incidents.Clear();

                foreach (var item in response)
                {
                    Incidents.Add(item);
                }

                Incidents = [..Incidents];
            }
            catch (Exception ex)
            {
                await _displayAlertService.ShowAlert("Error", $"Could not fetch Incidents: {ex.Message}", "No");
            }
        }

        public async Task AddIncidentAsync()
        {
            Location location = await _locationFetcher.GetCurrentLocation();

            var incident = new IncidentDataModel
            {
                Description = _description,
                Status = "Gemeld",
                Location = $"{location.Latitude} {location.Longitude}",
                UserId = _userStateService.user?.Id ?? null
            };

            try
            {
                await _apiService.AddIncidentAsync(incident);

                Description = string.Empty;

                await _displayAlertService.ShowAlert("Success", "Incident reported!", "OK");

                await this.FillReportedIncidentsByUser();
            }
            catch (Exception ex)
            {
                await _displayAlertService.ShowAlert("Error", $"Failed to save: {ex.Message}", "OK");
            }
        }
    }
}
