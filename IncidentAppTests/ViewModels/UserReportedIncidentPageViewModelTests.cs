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
    public class UserReportedIncidentPageViewModelTests
    {
        private readonly Mock<ApiService> _mockApiService;
        private readonly Mock<LocationFetcher> _mockLocationFetcher;
        private readonly Mock<DisplayAlertService> _mockDisplayAlertService;
        private readonly Mock<UserStateService> _mockUserStateService;
        private readonly UserReportedIncidentPageViewModel _viewModel;

        public UserReportedIncidentPageViewModelTests()
        {
            _mockApiService = new Mock<ApiService>();
            _mockLocationFetcher = new Mock<LocationFetcher>();
            _mockDisplayAlertService = new Mock<DisplayAlertService>();
            _mockUserStateService = new Mock<UserStateService>();

            _mockUserStateService.Setup(x => x.user).Returns(new UserDataModel { Id = 1 });

            _viewModel = new UserReportedIncidentPageViewModel(
                _mockApiService.Object,
                _mockLocationFetcher.Object,
                _mockDisplayAlertService.Object,
                _mockUserStateService.Object);
        }

        [Fact]
        public async Task FillReportedIncidentsByUser_SuccessfulLoad()
        {
            //Arrange
            var testIncidents = new List<IncidentDataModel>
        {
            new IncidentDataModel { Id = 1, Description = "Test 1" },
            new IncidentDataModel { Id = 2, Description = "Test 2" }
        };

            _mockApiService.Setup(x => x.GetIncidentsByUserId(1))
                         .ReturnsAsync(testIncidents);

            //Act
            await _viewModel.FillReportedIncidentsByUser();

            //Assert
            Assert.Equal(2, _viewModel.Incidents.Count);
            Assert.Equal("Test 1", _viewModel.Incidents[0].Description);
            Assert.Equal("Test 2", _viewModel.Incidents[1].Description);

            _mockApiService.Verify(x => x.GetIncidentsByUserId(1), Times.Once);
        }

        [Fact]
        public async Task FillReportedIncidentsByUser_FailedLoad_ShowsError()
        {
            //Arrange
            var errorMessage = "API error";
            _mockApiService.Setup(x => x.GetIncidentsByUserId(1))
                         .ThrowsAsync(new Exception(errorMessage));

            //Act
            await _viewModel.FillReportedIncidentsByUser();

            //Assert
            Assert.Empty(_viewModel.Incidents);

            _mockDisplayAlertService.Verify(x => x.ShowAlert(
                "Error",
                $"Could not fetch Incidents: {errorMessage}",
                "No"),
                Times.Once);
        }

        [Fact]
        public async Task AddIncidentAsync_SuccessfulReport()
        {
            //Arrange
            var testLocation = new Location(52.3676, 4.9041);
            _viewModel.Description = "Broken streetlight";

            //Setup
            _mockLocationFetcher.Setup(x => x.GetCurrentLocation())
                              .ReturnsAsync(testLocation);
            _mockApiService.Setup(x => x.AddIncidentAsync(It.IsAny<IncidentDataModel>()))
                         .Returns(Task.CompletedTask);
            _mockApiService.Setup(x => x.GetIncidentsByUserId(1))
                         .ReturnsAsync(new List<IncidentDataModel>());

            //Act
            await _viewModel.AddIncidentAsync();

            //Assert
            _mockLocationFetcher.Verify(x => x.GetCurrentLocation(), Times.Once);
            _mockApiService.Verify(x => x.AddIncidentAsync(It.Is<IncidentDataModel>(i =>
                i.Description == "Broken streetlight" &&
                i.Status == "Gemeld" &&
                (i.Location == "52,3676 4,9041" ||
                 i.Location == "52.3676 4.9041") &&
                i.UserId == 1
            )), Times.Once);

            _mockDisplayAlertService.Verify(x => x.ShowAlert(
                "Success",
                "Incident reported!",
                "OK"),
                Times.Once);

            Assert.Equal(string.Empty, _viewModel.Description);
        }

        [Fact]
        public async Task AddIncidentAsync_FailedReport_ShowsError()
        {
            //Arrange
            var testLocation = new Location(52.3676, 4.9041);
            var errorMessage = "Network error";
            _viewModel.Description = "Broken streetlight";

            _mockLocationFetcher.Setup(x => x.GetCurrentLocation())
                              .ReturnsAsync(testLocation);
            _mockApiService.Setup(x => x.AddIncidentAsync(It.IsAny<IncidentDataModel>()))
                         .ThrowsAsync(new Exception(errorMessage));

            //Act
            await _viewModel.AddIncidentAsync();

            //Assert
            _mockDisplayAlertService.Verify(x => x.ShowAlert(
                "Error",
                $"Failed to save: {errorMessage}",
                "OK"),
                Times.Once);

            Assert.Equal("Broken streetlight", _viewModel.Description);
        }

        [Fact]
        public async Task AddIncidentAsync_NoUser_StillReportsWithNullUserId()
        {
            //Arrange
            var testLocation = new Location(52.3676, 4.9041);
            _viewModel.Description = "Anonymous report";

            //Setup no user
            _mockUserStateService.Setup(x => x.user).Returns((UserDataModel)null);
            _mockLocationFetcher.Setup(x => x.GetCurrentLocation())
                              .ReturnsAsync(testLocation);

            //Act
            await _viewModel.AddIncidentAsync();

            //Assert
            _mockApiService.Verify(x => x.AddIncidentAsync(It.Is<IncidentDataModel>(i =>
                i.UserId == null
            )), Times.Once);
        }
    }
}
