using Microsoft.AspNetCore.Http;
using Moq;
using Workouts.Presentation.Components.Shared;
using Workouts.Presentation.Services;

namespace Workouts.UnitTests.Presentation
{
    public class NavMenuTests : IDisposable
    {
        private Mock<IApiCallService> _mockService;
        public NavMenuTests() 
        {
            _mockService = new Mock<IApiCallService>();
        }
        public void Dispose()
        {
            _mockService.Reset();
        }

        [Fact]
        public void OnInitializedAsync_GetsUserId_GivenAuthentication()
        {
            var menu = new NavMenu(_mockService.Object);

            menu.LoadUserInfo();

            _mockService.Verify(m => m.GetUserId(), Times.Once());
        }

        [Fact]
        public void OnInitializedAsync_GetsUsername_GivenAuthentication()
        {
            var menu = new NavMenu(_mockService.Object);

            menu.LoadUserInfo();

            _mockService.Verify(m => m.GetUsername(), Times.Once());
        }

    }
}
