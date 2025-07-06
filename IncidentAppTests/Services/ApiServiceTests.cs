using IncidentApp.Models;
using IncidentApp.Services;
using Moq;
using Moq.Protected;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Xunit;

namespace IncidentAppTests.Services
{
    public class ApiServiceTests
    {
        private readonly Mock<HttpMessageHandler> _handlerMock;
        private readonly HttpClient _httpClient;
        private readonly ApiService _apiService;

        public ApiServiceTests()
        {
            _handlerMock = new Mock<HttpMessageHandler>();
            _httpClient = new HttpClient(_handlerMock.Object)
            {
                BaseAddress = new Uri("https://localhost:7015")
            };
            _apiService = new ApiService(_httpClient);
        }

        [Fact]
        public async Task GetIncidentAsync_Returns_Incident()
        {
            //Arrange
            var expectedIncidents = new List<IncidentDataModel>
        {
            new IncidentDataModel { Id = 1, Description = "Test" }
        };

            _handlerMock.Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.Is<HttpRequestMessage>(req =>
                        req.Method == HttpMethod.Get &&
                        req.RequestUri.ToString().Contains("api/Incident")),
                    ItExpr.IsAny<CancellationToken>()
                )
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = new StringContent(JsonSerializer.Serialize(expectedIncidents))
                });

            //Act
            var result = await _apiService.GetIncidentAsync();

            //Assert
            Assert.NotNull(result);
            Assert.Single(result);
            Assert.Equal(1, result[0].Id);
        }

        [Fact]
        public async Task GetIncidentAsync_Throws_Exception()
        {
            //Arrange
            _handlerMock.Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>()
                )
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.BadRequest,
                    Content = new StringContent("Error message")
                });

            //Act
            var ex = await Assert.ThrowsAsync<Exception>(() => _apiService.GetIncidentAsync());

            //Assert
            Assert.Contains("API Error: BadRequest", ex.Message);
        }

        [Fact]
        public async Task GetIncidentsByUserId_Returns_Incident_Requested_By_User()
        {
            //Arrange
            var userId = 1;
            var expectedIncidents = new List<IncidentDataModel>
        {
            new IncidentDataModel { Id = 1, Description = "Test", UserId = userId }
        };

            _handlerMock.Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.Is<HttpRequestMessage>(req =>
                        req.Method == HttpMethod.Get &&
                        req.RequestUri.ToString().Contains($"api/Incident/{userId}")),
                    ItExpr.IsAny<CancellationToken>()
                )
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = new StringContent(JsonSerializer.Serialize(expectedIncidents))
                });

            //Act
            var result = await _apiService.GetIncidentsByUserId(userId);

            //Assert
            Assert.NotNull(result);
            Assert.Single(result);
            Assert.Equal(userId, result[0].UserId);
        }

        [Fact]
        public async Task AddIncidentAsync_Throws_Exception()
        {
            //Arrange
            var incident = new IncidentDataModel { Id = 1, Description = "Test" };

            _handlerMock.Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.Is<HttpRequestMessage>(req =>
                        req.Method == HttpMethod.Post &&
                        req.RequestUri.ToString().Contains("/api/Incident")),
                    ItExpr.IsAny<CancellationToken>()
                )
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.OK
                });

            //Act
            var exception = await Record.ExceptionAsync(() =>
                _apiService.AddIncidentAsync(incident));

            // Assert
            Assert.Null(exception);
        }

        [Fact]
        public async Task TryAuthenticate_Returns_User()
        {
            //Arrange
            var user = new UserAdminDataModel { Username = "test" };
            var expectedUser = new UserDataModel { Id = 1, Username = "test" };

            _handlerMock.Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.Is<HttpRequestMessage>(req =>
                        req.Method == HttpMethod.Post &&
                        req.RequestUri.ToString().Contains("/api/login")),
                    ItExpr.IsAny<CancellationToken>()
                )
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = new StringContent(JsonSerializer.Serialize(expectedUser))
                });

            //Act
            var result = await _apiService.TryAuthenticate(user);

            //Assert
            Assert.NotNull(result);
            Assert.Equal(1, result.Id);
            Assert.Equal("test", result.Username);
        }
    }
}
