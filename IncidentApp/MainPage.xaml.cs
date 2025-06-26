using IncidentApp.Models;
using IncidentApp.Services;
using IncidentApp.ViewModels;
using System.Net.Http;
using System.Threading.Tasks;

namespace IncidentApp
{
    public partial class MainPage : ContentPage
    {
        public MainPage(MainPageViewModel vM)
        {
            InitializeComponent();
            BindingContext = vM;
        }
    }
}
