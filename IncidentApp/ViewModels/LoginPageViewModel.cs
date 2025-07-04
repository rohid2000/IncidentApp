using IncidentApp.Models;
using IncidentApp.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IncidentApp.ViewModels
{
    public class LoginPageViewModel : BaseViewModel
    {
        private ApiService _apiService;
        private DisplayAlertService _displayAlertService;
        private NavigationService _navigationService;
        private UserStateService _userStateService;

        public string _username;
        public string _password;

        public Command LoginCommand { get; }

        public LoginPageViewModel()
        {
            _apiService = new ApiService();
            _displayAlertService = new DisplayAlertService();
            _navigationService = new NavigationService();
            _userStateService = new UserStateService();

            LoginCommand = new Command(async () => await Login());
        }

        public string Username
        {
            get => _username;
            set => SetProperty(ref _username, value);
        }

        public string Password
        {
            get => _password;
            set => SetProperty(ref _password, value);
        }

        private async Task Login()
        {
            var user = new UserAdminDataModel
            { 
                Username = _username,
                Password = _password
            };

            try
            {
                var response = _apiService.TryAuthenticate(user);

                await _displayAlertService.ShowAlert("Sucess", "User logged in!", "No");
                _userStateService.user = await response;

                await _navigationService.PushAsync<UserReportedIncidentsPage>();
            }
            catch (Exception ex)
            {
                await _displayAlertService.ShowAlert("Error", $"Failed to login: {ex.Message}", "Yes");
            }
        }
    }
}
