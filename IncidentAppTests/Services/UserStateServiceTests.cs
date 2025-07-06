using IncidentApp.Models;
using IncidentApp.Services;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace IncidentAppTests.Services
{
    public class UserStateServiceTests
    {
        [Fact]
        public void UserProperties_CanSetAndGet_UserDataModel_Properties()
        {
            //Arrange
            var service = new UserStateService();
            var testUser = new UserDataModel
            {
                Id = 1,
                Username = "TestUser",
                IsAdmin = false
            };

            //Act
            service.user = testUser;
            var retrievedUser = service.user;

            //Assert
            Assert.NotNull(retrievedUser);
            Assert.Equal(1, retrievedUser.Id);
            Assert.Equal("TestUser", retrievedUser.Username);
            Assert.False(retrievedUser.IsAdmin);
        }

        [Fact]
        public void User_CanBeSetToNull()
        {
            //Arrange
            var service = new UserStateService();
            service.user = new UserDataModel();

            //Act
            service.user = null;

            //Assert
            Assert.Null(service.user);
        }

        [Fact]
        public void UserProperties_CanBe_Updated()
        {
            //Arrange
            var service = new UserStateService();
            var testUser = new UserDataModel { Id = 1 };
            service.user = testUser;

            //Act
            testUser.Username = "Updated";
            testUser.IsAdmin = true;

            //Assert
            Assert.Equal("Updated", service.user.Username);
            Assert.True(service.user.IsAdmin);
        }

        [Fact]
        public void User_CanBeMocked()
        {
            //Arrange
            var mockService = new Mock<UserStateService>();
            var testUser = new UserDataModel { Id = 1 };

            mockService.SetupProperty(x => x.user);

            //Act
            mockService.Object.user = testUser;

            //Assert
            mockService.VerifySet(x => x.user = testUser, Times.Once);
            Assert.Equal(testUser, mockService.Object.user);
        }
    }
}
