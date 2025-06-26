using IncidentApp.Models;
using IncidentApp.Services;
using IncidentApp.ViewModels;

namespace IncidentApp;

public partial class RegisterPage : ContentPage
{
	public  RegisterPage(RegisterPageViewModel vM)
	{
		InitializeComponent();
		BindingContext = vM;
	}
}