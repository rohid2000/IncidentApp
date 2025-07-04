using IncidentApp.Models;
using IncidentApp.Services;
using System.Windows.Input;

namespace IncidentApp.ViewModels
{
    public class MainPageViewModel : BaseViewModel
    {
        private ApiService _apiService;
        private DisplayAlertService _displayAlertService;
        private NavigationService _navigationService;
        private UserStateService _userStateService;

        private string _description;

        public Command AddReportedIncidentCommand { get; }
        public Command NavigateToRegisterPageCommand { get; }
        public Command NavigateToLoginPageCommand { get; }

        public MainPageViewModel()
        {
            _apiService = new ApiService();
            _displayAlertService = new DisplayAlertService();
            _navigationService = new NavigationService();
            _userStateService = new UserStateService();

            AddReportedIncidentCommand = new Command(async() => await AddReportedIncident());
            NavigateToRegisterPageCommand = new Command(async() => await NavigateToRegisterPage());
            NavigateToLoginPageCommand = new Command(async() => await NavigateToLoginPage());
        }

        public string Description
        {
            get => _description;
            set => SetProperty(ref _description, value);
        }

        private async Task AddReportedIncident()
        {
            try
            {
                Location location = await LocationFetcher.GetCurrentLocation();

                var incident = new IncidentDataModel
                {
                    Description = Description,
                    Location = $"{location.Latitude} {location.Longitude}",
                    UserId = _userStateService.user?.Id ?? null
                };

                await _apiService.AddIncidentAsync(incident);

                Description = string.Empty;

                await _displayAlertService.ShowAlert("Success", "Incident added!", "No");
            }
            catch (Exception ex)
            {
                await _displayAlertService.ShowAlert("Failed", $"{ex.Message}", "Yes");
            }
        }

        private async Task NavigateToRegisterPage()
        {
            await _navigationService.PushAsync<RegisterPage>();
        }

        private async Task NavigateToLoginPage()
        {
            await _navigationService.PushAsync<LoginPage>();
        }
    }   
}
