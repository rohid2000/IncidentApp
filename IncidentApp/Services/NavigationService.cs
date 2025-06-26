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
            var page = App.Current.MainPage.Handler.MauiContext.Services.GetService<T>();
            await Shell.Current.Navigation.PushAsync(page);
        }
    }
}
