using IncidentApp.Models;
using IncidentApp.Services;
using IncidentApp.ViewModels;
using System.Collections.ObjectModel;

namespace IncidentApp;

public partial class UserReportedIncidentsPage : ContentPage
{
	public UserReportedIncidentsPage(UserReportedIncidentPageViewModel vM)
	{
		InitializeComponent();
		BindingContext = vM;
    }
}