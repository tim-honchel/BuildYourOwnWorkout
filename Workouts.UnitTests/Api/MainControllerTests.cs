﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Workouts.Api.Controllers;
using Workouts.Entities.CustomExceptions;
using Workouts.Entities.Database;
using Workouts.Entities.Dto;
using Workouts.Logic.Interfaces;
using Workouts.Services;

namespace Workouts.UnitTests.Api
{
    public class MainControllerTests : IDisposable
    {
        private MainController ControllerWithoutUser { get; set; }
        private MainController ControllerWithActiveUser { get; set; }
        private MainController ControllerWithDeactivatedUser { get; set; }
        private Mock<IUserLogic> MockUserLogic { get; set; }
        private Mock<IWorkoutLogic> MockWorkoutLogic { get; set; }
        private User ActiveUser { get; set; }
        private User DeactivatedUser { get; set; }
        private WorkoutDto SampleWorkoutDto { get; set; }
        private WorkoutDto InvalidWorkoutDto { get; set; }
        private List<WorkoutDto> SampleWorkoutsDto { get; set; }
        private Dictionary<long, string> SampleWorkoutsData {get; set;}

        public MainControllerTests()
        {
            ActiveUser = GetActiveUser();
            DeactivatedUser = GetDeactivatedUser();

            SampleWorkoutDto = WorkoutService.GetSampleWorkoutDto();
            InvalidWorkoutDto = WorkoutService.GetSampleWorkoutDto();
            InvalidWorkoutDto.Id = 2;
            InvalidWorkoutDto.Title = string.Empty;
            SampleWorkoutsDto = new List<WorkoutDto>() { SampleWorkoutDto };
            SampleWorkoutsData = GetWorkoutsData(SampleWorkoutsDto);
            
            MockUserLogic = new Mock<IUserLogic>();
            MockWorkoutLogic = new Mock<IWorkoutLogic>();
            SetupMockUserLogic();
            SetupMockWorkoutLogic();

            ControllerWithoutUser = GetCustomController(string.Empty, string.Empty);
            ControllerWithActiveUser = GetCustomController(ActiveUser.NameIdentifierClaim, ActiveUser.Username);
            ControllerWithDeactivatedUser = GetCustomController(DeactivatedUser.NameIdentifierClaim, DeactivatedUser.Username);
        }

        public void Dispose()
        {
            MockUserLogic.Reset();
            MockWorkoutLogic.Reset();
        }

        private User GetActiveUser()
        {
            return new User()
            {
                Id = 1,
                Username = "activeUser",
                Active = true,
                NameIdentifierClaim = "activeIdentifier"
            };
        }

        private User GetDeactivatedUser()
        {
            return new User()
            {
                Id = 2,
                Username = "deactivatedUser",
                Active = false,
                NameIdentifierClaim = "deactivatedIdentifier"
            };
        }

        private Dictionary<long, string> GetWorkoutsData(List<WorkoutDto> workouts)
        {
            var data = new Dictionary<long, string>();

            foreach (var workout in workouts)
            {
                data[workout.Id] = workout.Title;
            }

            return data;
        }

        private MainController GetCustomController(string userIdentifier, string username)
        {
            var context = new DefaultHttpContext();
            if (!string.IsNullOrEmpty(userIdentifier))
            {
                context.Request.Headers.Append("UserIdentifier", userIdentifier);
                context.Request.Headers.Append("Username", username);
            }
            var controller = new MainController(MockUserLogic.Object, MockWorkoutLogic.Object)
            {
                ControllerContext = new ControllerContext()
                {
                    HttpContext = context
                }
            };
            return controller;
        }

        private void SetupMockUserLogic()
        {
            MockUserLogic.Setup(m => m.GetUser(
                It.Is<string>(i => i == ActiveUser.NameIdentifierClaim),
                It.Is<string>(i => i == ActiveUser.Username)))
                .Returns(ActiveUser);

            MockUserLogic.Setup(m => m.GetUser(
                     It.Is<string>(i => i == DeactivatedUser.NameIdentifierClaim),
                     It.Is<string>(i => i == DeactivatedUser.Username)))
                     .Throws(new DeactivatedUserException());
        }

        private void SetupMockWorkoutLogic()
        {
            MockWorkoutLogic.Setup(m => m.AddWorkout(
                It.Is<WorkoutDto>(i => i.Id == 0)))
                .Returns(1);

            MockWorkoutLogic.Setup(m => m.GetWorkoutById(
                It.Is<long>(i => i == SampleWorkoutDto.Id)))
                .Returns(SampleWorkoutDto);

            MockWorkoutLogic.Setup(m => m.GetWorkoutById(
                It.Is<long>(i => i != SampleWorkoutDto.Id)))
                .Throws(new WorkoutDoesNotExistException());

            MockWorkoutLogic.Setup(m => m.GetWorkoutsByUserId(
                It.Is<long>(i => i == ActiveUser.Id)))
                .Returns(SampleWorkoutsDto);
        }

        [Fact]
        public void OnGetCurrentUserId_ReturnsOkWithId_GivenValidHeaderAndUser()
        {
            var response = (OkObjectResult)ControllerWithActiveUser.OnGetUserId();
            var returnedId = response.Value;

            Assert.Equal(returnedId, ActiveUser.Id);
        }

        [Fact]
        public void OnGetUserId_ReturnsForbidden_GivenDeactivatedUserException()
        {
            var response = ControllerWithDeactivatedUser.OnGetUserId();

            Assert.IsType<ForbidResult>(response);
        }

        [Fact]
        public void OnGetUserId_ReturnsUnauthorized_GivenInvalidHeader()
        {
            var response = ControllerWithoutUser.OnGetUserId();

            Assert.IsType<UnauthorizedResult>(response);
        }

        [Fact]
        public void OnGetUsername_ReturnsOkWithUsername_GivenValidHeaderAndUser()
        {
            var response = (OkObjectResult)ControllerWithActiveUser.OnGetUsername();
            var returnedUsername = response.Value;

            Assert.Equal(returnedUsername, ActiveUser.Username);
        }

        [Fact]
        public void OnGetUsername_ReturnsForbidden_GivenDeactivatedUserException()
        {
            var response = ControllerWithDeactivatedUser.OnGetUsername();

            Assert.IsType<ForbidResult>(response);
        }

        [Fact]
        public void OnGetUsername_ReturnsUnauthorized_GivenInvalidHeader()
        {
            var response = ControllerWithoutUser.OnGetUsername();

            Assert.IsType<UnauthorizedResult>(response);
        }

        [Fact]
        public void OnGetWorkout_ReturnsOkWithWorkout_GivenValidId()
        {
            var response = (OkObjectResult)ControllerWithoutUser.OnGetWorkout(SampleWorkoutDto.Id);
            var returnedWorkoutDto = response.Value;

            Assert.Equivalent(returnedWorkoutDto, SampleWorkoutDto);
        }

        [Fact]
        public void OnGetWorkout_ReturnsNotFound_GivenWorkoutDoesNotExistException()
        {
            var response = ControllerWithoutUser.OnGetWorkout(InvalidWorkoutDto.Id);

            Assert.IsType<NotFoundResult>(response);
        }

        [Fact]
        public void OnGetWorkouts_ReturnsOkWithWorkoutDictionary()
        {
            var response = (OkObjectResult)ControllerWithoutUser.OnGetWorkouts(ActiveUser.Id);
            var returnedWorkoutsData = response.Value;

            Assert.Equivalent(returnedWorkoutsData, SampleWorkoutsData);
        }

        [Fact]
        public void OnPostWorkout_ReturnsOkWithId_GivenValidWorkout()
        {
            var workout = WorkoutService.GetSampleWorkoutDto();
            workout.Id = 0;
            var response = (OkObjectResult)ControllerWithoutUser.OnPostWorkout(workout);
            var returnedId = response.Value;

            Assert.True(returnedId != null && (long)returnedId > 0);
        }

        [Fact]
        public void OnPostWorkout_ReturnsBadRequestWithValidationErrors_GivenInvalidWorkoutException()
        {
            var response = (BadRequestObjectResult)ControllerWithoutUser.OnPostWorkout(InvalidWorkoutDto);
            var validationErrors = response.Value;

            Assert.NotNull(validationErrors);
        }

        [Fact]
        public void OnPutArchiveWorkout_ReturnsOk()
        {
            var response = ControllerWithoutUser.OnPutArchiveWorkout(SampleWorkoutDto.Id);

            Assert.IsType<OkResult>(response);
        }

        [Fact]
        public void OnPutUnarchiveWorkout_ReturnsOk()
        {
            var response = ControllerWithoutUser.OnPutUnarchiveWorkout(SampleWorkoutDto.Id);

            Assert.IsType<OkResult>(response);
        }


        [Fact]
        public void OnPutUsername_ReturnsOk_GivenValidHeader()
        {
            var response = ControllerWithActiveUser.OnPutUsername("updateUsername");

            Assert.IsType<OkResult>(response);
        }

        [Fact]
        public void OnPutUsername_ReturnsForbidden_GivenDeactivatedUser()
        {
            var response = ControllerWithDeactivatedUser.OnPutUsername("updateUsername");

            Assert.IsType<ForbidResult>(response);
        }

        [Fact]
        public void OnPutUsername_ReturnsUnauthorized_GivenInvalidHeader()
        {
            var response = ControllerWithoutUser.OnPutUsername("updateUsername");

            Assert.IsType<UnauthorizedResult>(response);
        }

        [Fact]
        public void OnPutWorkout_ReturnsOk_GivenValidWorkout()
        {
            var response = ControllerWithoutUser.OnPutWorkout(SampleWorkoutDto);

            Assert.IsType<OkResult>(response);
        }

        [Fact]
        public void OnPutWorkout_ReturnsBadRequestWithValidationErrors_GivenInvalidWorkout()
        {
            var response = (BadRequestObjectResult)ControllerWithoutUser.OnPutWorkout(InvalidWorkoutDto);
            var validationErrors = response.Value;

            Assert.NotNull(validationErrors);
        }
    }
}
