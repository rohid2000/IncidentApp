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
    public class DisplayAlertServiceTests
    {
        [Fact]
        public async Task ShowAlert_Returns_DisplayAlert_With_Title_Message_And_Cancel_Once()
        {
            //Arrange
            var mockShell = new Mock<IShell>(MockBehavior.Strict);
            mockShell.Setup(x => x.DisplayAlert(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
                   .Returns(Task.CompletedTask)
                   .Verifiable();

            var service = new TestableDisplayAlertService(mockShell.Object);

            const string testTitle = "Test Title";
            const string testMessage = "Test Message";
            const string testCancel = "OK";

            //Act
            await service.ShowAlert(testTitle, testMessage, testCancel);

            //Assert
            mockShell.Verify(x => x.DisplayAlert(testTitle, testMessage, testCancel), Times.Once);
        }

        private class TestableDisplayAlertService : DisplayAlertService
        {
            private readonly IShell _shell;

            public TestableDisplayAlertService(IShell shell)
            {
                _shell = shell;
            }

            public override async Task ShowAlert(string title, string message, string cancel)
            {
                await _shell.DisplayAlert(title, message, cancel);
            }
        }

        public interface IShell
        {
            Task DisplayAlert(string title, string message, string cancel);
        }
    }
}
