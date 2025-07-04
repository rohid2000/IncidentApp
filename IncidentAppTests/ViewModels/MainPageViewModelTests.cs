using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IncidentAppTests.ViewModels
{
    using Xunit;
    using Moq;
    using IncidentApp.Services;
    using IncidentApp.Models;
    using System.Threading.Tasks;
    using System;
    using IncidentApp.ViewModels;

    public class MainPageViewModelTests
    {
        //ToDo: Create (non-static) Interfaces that inherit the static versions of the classes, to be able to Mock the static classes: see Deepseek

        //    private readonly Mock<ILocationFetcher> _mockLocationFetcher;
        //    private readonly Mock<IApiService> _mockApiService;
        //    private readonly Mock<IDisplayAlertService> _mockAlertService;
        //    private readonly Mock<INavigationService> _mockNavigationService;
        //    private readonly Mock<IUserStateService> _mockUserStateService;
        //    private readonly MainPageViewModel _viewModel;

        //    public MainPageViewModelTests()
        //    {
        //        _mockLocationFetcher = new Mock<ILocationFetcher>();
        //        _mockApiService = new Mock<IApiService>();
        //        _mockAlertService = new Mock<IDisplayAlertService>();
        //        _mockNavigationService = new Mock<INavigationService>();
        //        _mockUserStateService = new Mock<IUserStateService>();

        //        // Initialize services with mocks
        //        LocationFetcher.Instance = _mockLocationFetcher.Object;
        //        ApiService.Instance = _mockApiService.Object;
        //        DisplayAlertService.Instance = _mockAlertService.Object;
        //        NavigationService.Instance = _mockNavigationService.Object;
        //        UserStateService.Instance = _mockUserStateService.Object;

        //        _viewModel = new MainPageViewModel();
        //    }

        //    [Fact]
        //    public void Description_PropertyChanged_ShouldUpdateValue()
        //    {
        //        // Arrange
        //        var newDescription = "Test description";
        //        var propertyChangedRaised = false;
        //        _viewModel.PropertyChanged += (sender, e) =>
        //        {
        //            if (e.PropertyName == nameof(MainPageViewModel.Description))
        //            {
        //                propertyChangedRaised = true;
        //            }
        //        };

        //        // Act
        //        _viewModel.Description = newDescription;

        //        // Assert
        //        Assert.Equal(newDescription, _viewModel.Description);
        //        Assert.True(propertyChangedRaised);
        //    }

        //    [Fact]
        //    public async Task AddReportedIncidentCommand_ShouldAddIncident_WhenValidData()
        //    {
        //        // Arrange
        //        var testDescription = "Test incident";
        //        var testLocation = new Location { Latitude = 1.23, Longitude = 4.56 };
        //        var testUserId = "user123";

        //        _viewModel.Description = testDescription;
        //        _mockLocationFetcher.Setup(x => x.GetCurrentLocation()).ReturnsAsync(testLocation);
        //        _mockUserStateService.Setup(x => x.user).Returns(new User { Id = testUserId });
        //        _mockApiService.Setup(x => x.AddIncidentAsync(It.IsAny<IncidentDataModel>()))
        //            .Returns(Task.CompletedTask);

        //        // Act
        //        await _viewModel.AddReportedIncidentCommand.ExecuteAsync(null);

        //        // Assert
        //        _mockLocationFetcher.Verify(x => x.GetCurrentLocation(), Times.Once);
        //        _mockApiService.Verify(x => x.AddIncidentAsync(It.Is<IncidentDataModel>(i =>
        //            i.Description == testDescription &&
        //            i.Location == $"{testLocation.Latitude} {testLocation.Longitude}" &&
        //            i.UserId == testUserId
        //        )), Times.Once);

        //        Assert.Equal(string.Empty, _viewModel.Description);
        //        _mockAlertService.Verify(x => x.ShowAlert("Success", "Incident added!", "No"), Times.Once);
        //    }

        //    [Fact]
        //    public async Task AddReportedIncidentCommand_ShouldHandleError_WhenLocationFails()
        //    {
        //        // Arrange
        //        var errorMessage = "Location failed";
        //        _viewModel.Description = "Test";
        //        _mockLocationFetcher.Setup(x => x.GetCurrentLocation())
        //            .ThrowsAsync(new Exception(errorMessage));

        //        // Act
        //        await _viewModel.AddReportedIncidentCommand.ExecuteAsync(null);

        //        // Assert
        //        _mockAlertService.Verify(x => x.ShowAlert("Failed", errorMessage, "Yes"), Times.Once);
        //        _mockApiService.Verify(x => x.AddIncidentAsync(It.IsAny<IncidentDataModel>()), Times.Never);
        //    }

        //    [Fact]
        //    public async Task AddReportedIncidentCommand_ShouldHandleError_WhenApiFails()
        //    {
        //        // Arrange
        //        var errorMessage = "API failed";
        //        _viewModel.Description = "Test";
        //        _mockLocationFetcher.Setup(x => x.GetCurrentLocation())
        //            .ReturnsAsync(new Location());
        //        _mockApiService.Setup(x => x.AddIncidentAsync(It.IsAny<IncidentDataModel>()))
        //            .ThrowsAsync(new Exception(errorMessage));

        //        // Act
        //        await _viewModel.AddReportedIncidentCommand.ExecuteAsync(null);

        //        // Assert
        //        _mockAlertService.Verify(x => x.ShowAlert("Failed", errorMessage, "Yes"), Times.Once);
        //    }

        //    [Fact]
        //    public async Task AddReportedIncidentCommand_ShouldWork_WhenUserNotLoggedIn()
        //    {
        //        // Arrange
        //        _viewModel.Description = "Test";
        //        _mockLocationFetcher.Setup(x => x.GetCurrentLocation())
        //            .ReturnsAsync(new Location());
        //        _mockUserStateService.Setup(x => x.user).Returns((User)null);
        //        _mockApiService.Setup(x => x.AddIncidentAsync(It.IsAny<IncidentDataModel>()))
        //            .Returns(Task.CompletedTask);

        //        // Act
        //        await _viewModel.AddReportedIncidentCommand.ExecuteAsync(null);

        //        // Assert
        //        _mockApiService.Verify(x => x.AddIncidentAsync(It.Is<IncidentDataModel>(i =>
        //            i.UserId == null
        //        )), Times.Once);
        //    }

        //    [Fact]
        //    public async Task NavigateToRegisterPageCommand_ShouldNavigateToRegisterPage()
        //    {
        //        // Act
        //        await _viewModel.NavigateToRegisterPageCommand.ExecuteAsync(null);

        //        // Assert
        //        _mockNavigationService.Verify(x => x.PushAsync<RegisterPage>(), Times.Once);
        //    }

        //    [Fact]
        //    public async Task NavigateToLoginPageCommand_ShouldNavigateToLoginPage()
        //    {
        //        // Act
        //        await _viewModel.NavigateToLoginPageCommand.ExecuteAsync(null);

        //        // Assert
        //        _mockNavigationService.Verify(x => x.PushAsync<LoginPage>(), Times.Once);
        //    }

        //    [Fact]
        //    public async Task AddReportedIncident_ShouldNotProceed_WhenDescriptionIsEmpty()
        //    {
        //        // Arrange
        //        _viewModel.Description = string.Empty;

        //        // Act
        //        await _viewModel.AddReportedIncident();

        //        // Assert
        //        _mockLocationFetcher.Verify(x => x.GetCurrentLocation(), Times.Never);
        //        _mockApiService.Verify(x => x.AddIncidentAsync(It.IsAny<IncidentDataModel>()), Times.Never);
        //    }
        //}
    }
}
