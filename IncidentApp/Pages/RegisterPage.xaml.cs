using IncidentApp.Models;
using IncidentApp.Services;
using IncidentApp.ViewModels;

namespace IncidentApp;

public partial class RegisterPage : ContentPage
{
	public RegisterPage(RegisterPageViewModel vM)
	{
		InitializeComponent();
		BindingContext = vM;
	}

	//private async void AddUser(object sender, EventArgs e)
	//{
	//	var user = new UserAdminDataModel
 //       {
	//		Username = UsernameEntry.Text,
	//		Password = PasswordEntry.Text
	//	};

	//	try
	//	{
	//		await ApiService.AddUserAsync(user);

	//		UsernameEntry.Text = string.Empty;
	//		PasswordEntry.Text = string.Empty;

	//		await DisplayAlert("Success", "User added", "OK");

	//		await Navigation.PushAsync(new LoginPage());
	//	}
	//	catch (Exception ex)
	//	{
 //           await DisplayAlert("Error", $"Failed to save: {ex.Message}", "OK");
 //       }
	//}
}