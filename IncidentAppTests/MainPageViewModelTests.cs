using IncidentApp;
using IncidentApp.Models;
using IncidentApp.Services;
using IncidentApp.ViewModels;
using Moq;
using Windows.Services.Maps;
using Windows.System;
using Xunit;

namespace IncidentAppTests
{
    public class MainPageViewModelTests
    {
        private readonly Mock<ApiService> _apiService;
        private readonly Mock<LocationFetcher> _locationFetcher;
        private readonly Mock<DisplayAlertService> _displayAlertService;
        private readonly Mock<NavigationService> _navigationService;
        private readonly Mock<UserStateService> _userStateService;
        private readonly MainPageViewModel _viewModel;

        public MainPageViewModelTests()
        {
            _apiService = new Mock<ApiService>();
            _locationFetcher = new Mock<LocationFetcher>();
            _displayAlertService = new Mock<DisplayAlertService>();
            _navigationService = new Mock<NavigationService>();
            _userStateService = new Mock<UserStateService>();

            _viewModel = new MainPageViewModel(
                _apiService.Object,
                _locationFetcher.Object,
                _displayAlertService.Object,
                _navigationService.Object,
                _userStateService.Object
            );
        }

        [Fact]
        public async Task AddReportedIncident_Successful_WithAnonymousUser()
        {
            // Arrange
            var testLocation = new Location(47.6062, -122.3321);
            _viewModel.Description = "Test incident";

            _locationFetcher
                .Setup(x => x.GetCurrentLocation())  // Now works because method is virtual
                .ReturnsAsync(testLocation);

            _userStateService
                .Setup(x => x.user)
                .Returns(new UserDataModel { Id = 1 });

            _displayAlertService
                .Setup(x => x.ShowAlert(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
                .Returns(Task.CompletedTask);

            // Act
            await _viewModel.AddReportedIncident();

            // Assert
            _apiService.Verify(
                x => x.AddIncidentAsync(It.Is<IncidentDataModel>(i => i.UserId == null)),
                Times.Once
            );
        }
    }
}
