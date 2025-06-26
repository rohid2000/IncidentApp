using IncidentApp.Models;
using IncidentApp.Services;
using System.Windows.Input;

namespace IncidentApp.ViewModels
{
    public class MainPageViewModel : BaseViewModel
    {
        private string _description;

        public MainPageViewModel()
        {
            AddReportedIncidentCommand = new Command(async() => await AddReportedIncident());
            NavigateToRegisterPageCommand = new Command(async() => await NavigateToRegisterPage());
            NavigateToLoginPageCommand = new Command(async() => await NavigateToLoginPage());
        }

        public string Description
        {
            get => _description;
            set => SetProperty(ref _description, value);
        }

        public Command AddReportedIncidentCommand { get; }
        public Command NavigateToRegisterPageCommand { get; }
        public Command NavigateToLoginPageCommand { get; }

        private async Task AddReportedIncident()
        {
            try
            {
                Location location = await LocationFetcher.GetCurrentLocation();

                var incident = new IncidentDataModel
                {
                    Description = Description,
                    Location = $"{location.Latitude} {location.Longitude}",
                    UserId = UserStateService.user?.Id ?? null
                };

                await ApiService.AddIncidentAsync(incident);

                Description = string.Empty;

                DisplayAlertService.ShowAlert("Success", "Incident added!", "No");
            }
            catch (Exception ex)
            {
                DisplayAlertService.ShowAlert("Failed", $"{ex.Message}", "Yes");
            }
        }

        private async Task NavigateToRegisterPage()
        {
            await NavigationService.PushAsync<RegisterPage>();
        }

        private async Task NavigateToLoginPage()
        {
            await NavigationService.PushAsync<LoginPage>();
        }
    }   
}
