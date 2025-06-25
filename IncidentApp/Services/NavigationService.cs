using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IncidentApp.Services
{
    public static class NavigationService
    {
        public static async Task PushAsync<T>() where T : Page
        {
            await Shell.Current.Navigation.PushAsync(Activator.CreateInstance<T>());
        }

        public static async Task ShowAlert(string title, string message, string cancel)
        {
            await Shell.Current.DisplayAlert(title, message, cancel);
        }
    }
}
