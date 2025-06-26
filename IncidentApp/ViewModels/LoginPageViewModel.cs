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
        public string _username;
        public string _password;

        public Command LoginCommand { get; }

        public LoginPageViewModel()
        {
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
                var response = ApiService.TryAuthenticate(user);

                await DisplayAlertService.ShowAlert("Sucess", "User logged in!", "No");
                UserStateService.user = await response;

                await NavigationService.PushAsync<UserReportedIncidentsPage>();
            }
            catch (Exception ex)
            {
                await DisplayAlertService.ShowAlert("Error", $"Failed to login: {ex.Message}", "Yes");
            }
        }
    }
}
