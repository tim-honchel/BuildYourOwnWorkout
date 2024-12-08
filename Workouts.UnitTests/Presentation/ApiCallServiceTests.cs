using Microsoft.AspNetCore.Http;
using Moq;
using Moq.Protected;
using Newtonsoft.Json;
using System.Net;
using Workouts.Entities.CustomExceptions;
using Workouts.Entities.Dto;
using Workouts.Presentation.Services;
using Workouts.Services;

namespace Workouts.UnitTests.Presentation
{
    public class ApiCallServiceTests : IDisposable
    {
        private Mock<HttpMessageHandler> MockHandlerForActiveUserValidWorkout;
        private Mock<HttpMessageHandler> MockHandlerForDeactivatedUserInvalidWorkout;

        private ApiCallService ServiceWithActiveUserValidWorkout;
        private ApiCallService ServiceWithDeactivatedUserInvalidWorkout;

        private WorkoutDto ValidWorkout;
        private WorkoutDto InvalidWorkout;
        private Dictionary<long, string> WorkoutData;

        const long ActiveUserId = 1;
        const string ActiveUsername = "activeUsername";
        const long DeactivatedUserId = 2;

        public ApiCallServiceTests()
        {
            ValidWorkout = WorkoutService.GetSampleWorkoutDto();
            InvalidWorkout = WorkoutService.GetSampleWorkoutDto();
            InvalidWorkout.Title = string.Empty;
            InvalidWorkout.Id++;
            WorkoutData = new Dictionary<long, string>() 
            {
                {ValidWorkout.Id, ValidWorkout.Title }
            };

            MockHandlerForActiveUserValidWorkout = GetMockHandler(true);
            MockHandlerForDeactivatedUserInvalidWorkout = GetMockHandler(false);

            ServiceWithActiveUserValidWorkout = GetApiCallService(MockHandlerForActiveUserValidWorkout);
            ServiceWithDeactivatedUserInvalidWorkout = GetApiCallService(MockHandlerForDeactivatedUserInvalidWorkout);
        }

        public void Dispose()
        {
            MockHandlerForActiveUserValidWorkout.Reset();
            MockHandlerForDeactivatedUserInvalidWorkout.Reset();
        }

        private Mock<HttpMessageHandler> GetMockHandler(bool isActive)
        {
            var mockHandler = new Mock<HttpMessageHandler>();
            if (isActive)
            {
                AddHandlerSetup(mockHandler, HttpMethod.Get, "UserId", HttpStatusCode.OK, ActiveUserId);
                AddHandlerSetup(mockHandler, HttpMethod.Get, "Username", HttpStatusCode.OK, ActiveUsername);
                AddHandlerSetup(mockHandler, HttpMethod.Get, "Workout", HttpStatusCode.OK, ValidWorkout);
                AddHandlerSetup(mockHandler, HttpMethod.Get, "Workouts", HttpStatusCode.OK, WorkoutData);
                AddHandlerSetup(mockHandler, HttpMethod.Post, "Workout", HttpStatusCode.OK, ValidWorkout.Id);
                AddHandlerSetup(mockHandler, HttpMethod.Put, "ArchiveWorkout", HttpStatusCode.OK, string.Empty);
                AddHandlerSetup(mockHandler, HttpMethod.Put, "UnarchiveWorkout", HttpStatusCode.OK, string.Empty);
                AddHandlerSetup(mockHandler, HttpMethod.Put, "Username", HttpStatusCode.OK, string.Empty);
                AddHandlerSetup(mockHandler, HttpMethod.Put, "Workout", HttpStatusCode.OK, string.Empty);
            }
            else
            {
                AddHandlerSetup(mockHandler, HttpMethod.Get, "UserId", HttpStatusCode.Forbidden, string.Empty);
                AddHandlerSetup(mockHandler, HttpMethod.Get, "Username", HttpStatusCode.Forbidden, string.Empty);
                AddHandlerSetup(mockHandler, HttpMethod.Get, "Workout", HttpStatusCode.NotFound, string.Empty);
                AddHandlerSetup(mockHandler, HttpMethod.Post, "Workout", HttpStatusCode.BadRequest, "error");
                AddHandlerSetup(mockHandler, HttpMethod.Put, "Username", HttpStatusCode.Forbidden, string.Empty);
                AddHandlerSetup(mockHandler, HttpMethod.Put, "Workout", HttpStatusCode.BadRequest, "error");
            }
            return mockHandler;
        }

        private void AddHandlerSetup(Mock<HttpMessageHandler> mockHandler, HttpMethod requestMethod, string endpoint, HttpStatusCode responseCode, object responseContent)
        {
            var responseJson = JsonConvert.SerializeObject(responseContent);
            var content = new StringContent(responseJson);
            var response = new HttpResponseMessage() { StatusCode = HttpStatusCode.OK, Content = content };

            mockHandler.Protected().Setup<HttpResponseMessage>
                ("Send", It.Is<HttpRequestMessage>(r => r.Method == requestMethod 
                    && r.RequestUri.ToString().Contains(endpoint)))
                .Returns(response).Verifiable();
        }

        private ApiCallService GetApiCallService(Mock<HttpMessageHandler> mockHandler)
        {
            var context = new DefaultHttpContext();
            return new ApiCallService(context, mockHandler.Object);
        }


        [Fact]
        public void GetUserId_ShouldReturnId_GivenOk()
        {
            long returnedId = ServiceWithActiveUserValidWorkout.GetUserId();

            Assert.Equal(ActiveUserId, returnedId);
        }


        [Fact]
        public void GetUserId_ShouldThrowCustomException_GivenForbidden()
        {
            Assert.Throws<DeactivatedUserException>(() => ServiceWithDeactivatedUserInvalidWorkout.GetUserId());
        }

        [Fact]
        public void GetUsername_ShouldReturnUsername_GivenOk()
        {
            string returnedUsername = ServiceWithActiveUserValidWorkout.GetUsername();

            Assert.Equal(ActiveUsername, returnedUsername);
        }

        [Fact]
        public void GetUsername_ShouldThrowCustomException_GivenForbidden()
        {
            Assert.Throws<DeactivatedUserException>(() => ServiceWithDeactivatedUserInvalidWorkout.GetUsername());
        }

        [Fact]
        public void GetWorkout_ShouldReturnWorkout_GivenOk()
        {
            var returnedWorkout = ServiceWithActiveUserValidWorkout.GetWorkout(ValidWorkout.Id);

            Assert.Equivalent(ValidWorkout, returnedWorkout);
        }

        [Fact]
        public void GetWorkout_ShouldThrowCustomException_GivenNotFound()
        {
            Assert.Throws<WorkoutDoesNotExistException>(() => ServiceWithActiveUserValidWorkout.GetWorkout(InvalidWorkout.Id));
        }

        [Fact]
        public void GetWorkouts_ShouldReturnWorkouts_GivenOk()
        {
            var returnedWorkoutData = ServiceWithActiveUserValidWorkout.GetWorkouts(ActiveUserId);

            Assert.Equivalent(WorkoutData, returnedWorkoutData);
        }

        [Fact]
        public void AddWorkout_ShouldReturnId_GivenOk()
        {
            var returnedId = ServiceWithActiveUserValidWorkout.AddWorkout(ValidWorkout);

            Assert.Equal(ValidWorkout.Id, returnedId);
        }

        [Fact]
        public void AddWorkout_ShouldThrowCustomException_GivenBadRequest()
        {
            Assert.Throws<InvalidWorkoutException>(() => ServiceWithDeactivatedUserInvalidWorkout.AddWorkout(InvalidWorkout));
        }

        [Fact]
        public void ArchiveWorkout_ShouldNotThrowException_GivenOk()
        {
            ServiceWithActiveUserValidWorkout.ArchiveWorkout(ValidWorkout.Id);
        }

        [Fact]
        public void UnarchiveWorkout_ShouldNotThrowException_GivenOk()
        {
            ServiceWithActiveUserValidWorkout.UnarchiveWorkout(ValidWorkout.Id);
        }

        [Fact]
        public void UpdateUsername_ShouldNotThrowException_GivenOk()
        {
            ServiceWithActiveUserValidWorkout.UpdateUsername("newUsername");
        }

        [Fact]
        public void UpdateUsername_ShouldThrowCustomException_GivenForbidden()
        {
            Assert.Throws<DeactivatedUserException>(() => ServiceWithDeactivatedUserInvalidWorkout.UpdateUsername("newUsername"));
        }

        [Fact]
        public void UpdateWorkout_ShouldNotThrowException_GivenOk()
        {
            ServiceWithActiveUserValidWorkout.UpdateWorkout(ValidWorkout);
        }

        [Fact]
        public void UpdateWorkout_ShouldThrowCustomException_GivenBadRequest()
        {
            Assert.Throws<InvalidWorkoutException>(() => ServiceWithDeactivatedUserInvalidWorkout.UpdateWorkout(InvalidWorkout));
        }
    }
}
