using IncidentApp.Models;
using IncidentApp.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IncidentApp.ViewModels
{
    public class RegisterPageViewModel : BaseViewModel
    {
        private ApiService _apiService;
        private DisplayAlertService _displayAlertService;
        private NavigationService _navigationService;

        private string _username;
        private string _password;

        public Command AddUserAsyncCommand { get; }

        public RegisterPageViewModel()
        {
            _apiService = new ApiService();
            _displayAlertService = new DisplayAlertService();
            _navigationService = new NavigationService();

            AddUserAsyncCommand = new Command(async() => await AddUserAsync());
        }

        public RegisterPageViewModel(
            ApiService apiService,
            DisplayAlertService displayAlertService,
            NavigationService navigation)
        {
            _apiService = apiService;
            _displayAlertService = displayAlertService;
            _navigationService = navigation;
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

        public async Task AddUserAsync()
        {
            var user = new UserAdminDataModel
            {
                Username = _username,
                Password = _password
            };

            try
            {
                await _apiService.AddUserAsync(user);

                Username = string.Empty;
                Password = string.Empty;

                await _displayAlertService.ShowAlert("Success", "User added!", "No");

                await _navigationService.PushAsync<LoginPage>();
            }
            catch (Exception ex)
            {
                await _displayAlertService.ShowAlert("Failed", "User could not be added!", "Yes");
            }
        }
    }
}
