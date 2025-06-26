using IncidentApp.Models;
using IncidentApp.Services;
using IncidentApp.ViewModels;

namespace IncidentApp;

public partial class LoginPage : ContentPage
{
	public LoginPage(LoginPageViewModel vM)
	{
		InitializeComponent();
		BindingContext = vM;
	}
}