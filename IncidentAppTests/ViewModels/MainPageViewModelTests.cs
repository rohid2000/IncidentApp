using IncidentApp;
using IncidentApp.Models;
using IncidentApp.Services;
using IncidentApp.ViewModels;
using Moq;
using Windows.Services.Maps;
using Windows.System;
using Xunit;

namespace IncidentAppTests.ViewModelTests
{
    public class MainPageViewModelTests
    {
        private readonly Mock<ApiService> _apiService;
        private readonly Mock<LocationFetcher> _locationFetcher;
        private readonly Mock<DisplayAlertService> _displayAlertService;
        private readonly Mock<NavigationService> _navigationService;
        private readonly Mock<UserStateService> _userSTateService;
        private readonly MainPageViewModel _viewModel;

        public MainPageViewModelTests()
        {
            _apiService = new Mock<ApiService>();
            _locationFetcher = new Mock<LocationFetcher>();
            _displayAlertService = new Mock<DisplayAlertService>();
            _navigationService = new Mock<NavigationService>();
            _userSTateService = new Mock<UserStateService>();

            _locationFetcher.Setup(x => x.GetCurrentLocation())
                .ReturnsAsync(new Location(0, 0));
            _displayAlertService.Setup(x => x.ShowAlert(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
                .Returns(Task.CompletedTask);
            _apiService.Setup(x => x.AddIncidentAsync(It.IsAny<IncidentDataModel>()))
                .Returns(Task.CompletedTask);

            _viewModel = new MainPageViewModel(
                _apiService.Object,
                _locationFetcher.Object,
                _displayAlertService.Object,
                _navigationService.Object,
                _userSTateService.Object);
        }

        [Fact]
        public async Task AddReportedIncident_Successful_WithLoggedInUser()
        {
            //Arrange
            var testUser = new UserDataModel { Id = 1 };
            var testDescription = "Test incident description";

            _viewModel.Description = testDescription;
            _userSTateService.Setup(x => x.user).Returns(testUser);

            //Act
            await _viewModel.AddReportedIncident();

            //Assert
            _apiService.Verify(
                x => x.AddIncidentAsync(It.Is<IncidentDataModel>(i =>
                    i.Description == testDescription &&
                    i.UserId == testUser.Id
                )),
                Times.Once,
                "AddIncidentAsync should be called once with the correct incident data"
            );

            _displayAlertService.Verify(
                x => x.ShowAlert("Success", "Incident added!", "No"),
                Times.Once
            );

            Assert.Equal(string.Empty, _viewModel.Description);
        }

        [Fact]
        public async Task AddReportedIncident_Successful_WithoutLoggedInUser()
        {
            //Arrange
            var testLocation = new Location(47.6062, -122.3321);
            var testDescription = "Test incident description";

            _viewModel.Description = testDescription;
            _userSTateService.Setup(x => x.user).Returns((UserDataModel)null);
            _locationFetcher.Setup(x => x.GetCurrentLocation()).ReturnsAsync(testLocation);

            //Act
            await _viewModel.AddReportedIncident();

            //Assert
            _apiService.Verify(x => x.AddIncidentAsync(It.Is<IncidentDataModel>(i =>
                i.UserId == null
            )), Times.Once);
        }

        [Fact]
        public async Task AddReportedIncident_HandlesLocationFetchError()
        {
            //Arrange
            var errorMessage = "Location services not available";
            _locationFetcher.Setup(x => x.GetCurrentLocation())
                               .ThrowsAsync(new Exception(errorMessage));

            //Act
            await _viewModel.AddReportedIncident();

            //Assert
            _displayAlertService.Verify(x => x.ShowAlert(
                "Failed",
                It.Is<string>(s => s.Contains(errorMessage)),
                "Yes"),
                Times.Once);

            _apiService.Verify(x => x.AddIncidentAsync(It.IsAny<IncidentDataModel>()), Times.Never);
        }

        [Fact]
        public async Task AddReportedIncident_HandlesApiError()
        {
            //Arrange
            var testLocation = new Location(47.6062, -122.3321);
            var errorMessage = "API unavailable";

            _locationFetcher.Setup(x => x.GetCurrentLocation()).ReturnsAsync(testLocation);
            _apiService.Setup(x => x.AddIncidentAsync(It.IsAny<IncidentDataModel>()))
                           .ThrowsAsync(new Exception(errorMessage));

            //Act
            await _viewModel.AddReportedIncident();

            //Assert
            _displayAlertService.Verify(x => x.ShowAlert(
                "Failed",
                It.Is<string>(s => s.Contains(errorMessage)),
                "Yes"),
                Times.Once);
        }

        [Fact]
        public async Task AddReportedIncident_DescriptionEmptyAfterSuccess()
        {
            //Arrange
            var testLocation = new Location(47.6062, -122.3321);
            _viewModel.Description = "Test description";

            _locationFetcher.Setup(x => x.GetCurrentLocation()).ReturnsAsync(testLocation);

            //Act
            await _viewModel.AddReportedIncident();

            //Assert
            Assert.Equal(string.Empty, _viewModel.Description);
        }
    }
}
