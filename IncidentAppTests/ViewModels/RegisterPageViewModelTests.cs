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
using Windows.Services.Maps;
using Xunit;

namespace IncidentAppTests.ViewModelTests
{
    public class RegisterPageViewModelTests
    {
        private readonly Mock<ApiService> _apiService;
        private readonly Mock<DisplayAlertService> _displayAlertService;
        private readonly Mock<NavigationService> _navigationService;
        private readonly RegisterPageViewModel _viewModel;

        public RegisterPageViewModelTests()
        {
            _apiService = new Mock<ApiService>();
            _displayAlertService = new Mock<DisplayAlertService>();
            _navigationService = new Mock<NavigationService>();

            _apiService.Setup(x => x.AddUserAsync(It.IsAny<UserAdminDataModel>()))
                          .Returns(Task.CompletedTask);
            _displayAlertService.Setup(x => x.ShowAlert(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
                                   .Returns(Task.CompletedTask);
            _navigationService.Setup(x => x.PushAsync<LoginPage>())
                                .Returns(Task.CompletedTask);

            _viewModel = new RegisterPageViewModel(
                _apiService.Object,
                _displayAlertService.Object,
                _navigationService.Object)
            {
                Username = "TestUser",
                Password = "testpass123"
            };
        }

        [Fact]
        public async Task AddUserAsync_Returns_UserAdded()
        {
            //Act
            await _viewModel.AddUserAsync();

            //Assert
            _apiService.Verify(x => x.AddUserAsync(It.Is<UserAdminDataModel>(u =>
                u.Username == "TestUser" &&
                u.Password == "testpass123"
            )), Times.Once);

            _displayAlertService.Verify(
                x => x.ShowAlert("Success", "User added!", "No"), 
                Times.Once);

            _navigationService.Verify(x => x.PushAsync<LoginPage>(), Times.Once);

            Assert.Equal(string.Empty, _viewModel.Username);
            Assert.Equal(string.Empty, _viewModel.Password);
        }

        [Fact]
        public async Task AddUserAsync_FailedRegistration_ShowsError()
        {
            //Arrange
            var errorMessage = "API error occurred";
            _apiService.Setup(x => x.AddUserAsync(It.IsAny<UserAdminDataModel>()))
                          .ThrowsAsync(new Exception(errorMessage));

            //Act
            await _viewModel.AddUserAsync();

            //Assert
            _displayAlertService.Verify(
                x => x.ShowAlert("Failed", "User could not be added!", "Yes"), 
                Times.Once);

            _navigationService.Verify(x => x.PushAsync<LoginPage>(), Times.Never);

            Assert.Equal("TestUser", _viewModel.Username);
            Assert.Equal("testpass123", _viewModel.Password);
        }
    }
}
