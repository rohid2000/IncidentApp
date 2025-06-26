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
        private string _username;
        private string _password;

        public Command AddUserAsyncCommand { get; }

        public RegisterPageViewModel()
        {
            AddUserAsyncCommand = new Command(async() => await AddUserAsync());
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

        private async Task AddUserAsync()
        {
            var user = new UserAdminDataModel
            {
                Username = _username,
                Password = _password
            };

            try
            {
                await ApiService.AddUserAsync(user);

                Username = string.Empty;
                Password = string.Empty;

                await DisplayAlertService.ShowAlert("Sucess", "User added!", "No");

                await NavigationService.PushAsync<LoginPage>();
            }
            catch (Exception ex)
            {
                await DisplayAlertService.ShowAlert("Failed", "User could not be added!", "Yes");
            }
        }
    }
}
