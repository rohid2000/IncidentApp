using IncidentApp;
using IncidentApp.Models;
using IncidentApp.Services;
using IncidentApp.ViewModels;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.System;
using Xunit;

namespace IncidentAppTests.ViewModelTests
{
    public class LoginViewModelTests
    {
        private readonly Mock<ApiService> _apiService;
        private readonly Mock<DisplayAlertService> _displayAlertService;
        private readonly Mock<NavigationService> _navigationService;
        private readonly Mock<UserStateService> _userStateService;
        private readonly LoginPageViewModel _viewModel;

        public LoginViewModelTests()
        {
            _apiService = new Mock<ApiService>();
            _displayAlertService = new Mock<DisplayAlertService>();
            _navigationService = new Mock<NavigationService>();
            _userStateService = new Mock<UserStateService>();

            _apiService.Setup(x => x.TryAuthenticate(It.IsAny<UserAdminDataModel>()))
                          .Returns(Task.FromResult(new UserDataModel()));
            _displayAlertService.Setup(x => x.ShowAlert(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
                                   .Returns(Task.CompletedTask);
            _navigationService.Setup(x => x.PushAsync<UserReportedIncidentsPage>())
                                .Returns(Task.CompletedTask);

            _viewModel = new LoginPageViewModel(
                _apiService.Object,
                _displayAlertService.Object,
                _navigationService.Object,
                _userStateService.Object)
            {
                Username = "TestUser",
                Password = "testpass123"
            };
        }

        [Fact]
        public async Task Login_Succeeded_And_Sends_User_To_UserReportedIncidentsPage()
        {
            //Arrange
            var testUser = new UserDataModel { Id = 1, Username = "TestUser" };
            _apiService.Setup(x => x.TryAuthenticate(It.IsAny<UserAdminDataModel>()))
                          .Returns(Task.FromResult(testUser));

            //Act
            await _viewModel.Login();

            //Assert
            _apiService.Verify(x => x.TryAuthenticate(It.Is<UserAdminDataModel>(u =>
                u.Username == "TestUser" &&
                u.Password == "testpass123"
            )), Times.Once);

            _displayAlertService.Verify(
                x => x.ShowAlert("Sucess", "User logged in!", "No"),
                Times.Once);

            _navigationService.Verify(
                x => x.PushAsync<UserReportedIncidentsPage>(),
                Times.Once);

            _userStateService.VerifySet(
                x => x.user = testUser,
                Times.Once);
        }

        [Fact]
        public async Task Login_Failed_And_Returns_Error()
        {
            //Arrange
            var errorMessage = "Invalid credentials";
            _apiService.Setup(x => x.TryAuthenticate(It.IsAny<UserAdminDataModel>()))
                          .ThrowsAsync(new Exception(errorMessage));

            //Act
            await _viewModel.Login();

            //Assert
            _displayAlertService.Verify(
                x => x.ShowAlert("Error", $"Failed to login: {errorMessage}", "Yes"),
                Times.Once);

            _navigationService.Verify(
                x => x.PushAsync<UserReportedIncidentsPage>(),
                Times.Never);

            _userStateService.VerifySet(
                x => x.user = It.IsAny<UserDataModel>(),
                Times.Never);
        }
    }
}
