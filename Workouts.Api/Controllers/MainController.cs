using Castle.Components.DictionaryAdapter.Xml;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Primitives;
using Workouts.Entities.CustomExceptions;
using Workouts.Entities.Dto;
using Workouts.Entities.Misc;
using Workouts.Logic.Interfaces;
using Workouts.Services;

namespace Workouts.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class MainController : ControllerBase
    {
        private IUserLogic UserLogic { get; set; }
        private IWorkoutLogic WorkoutLogic { get; set; }

        public MainController(IUserLogic userLogic, IWorkoutLogic workoutLogic) 
        {
            UserLogic = userLogic;
            WorkoutLogic = workoutLogic;
        }

        // use headers for authentication and authorization

        [HttpGet("UserId")]
        public IActionResult OnGetUserId()
        {
            var headers = GetHeaders();

            if (string.IsNullOrEmpty(headers.UserIdentifier))
            {
                return new UnauthorizedResult();
            }
            try
            {
                var id = UserLogic.GetUser(headers.UserIdentifier, headers.Username).Id;
                return new OkObjectResult(id);
            }
            catch (DeactivatedUserException)
            {
                return new ForbidResult();
            }
        }

        [HttpGet("Username")]
        public IActionResult OnGetUsername()
        {
            var headers = GetHeaders();

            if (string.IsNullOrEmpty(headers.UserIdentifier))
            {
                return new UnauthorizedResult();
            }
            try
            {
                var id = UserLogic.GetUser(headers.UserIdentifier, headers.Username).Username;
                return new OkObjectResult(id);
            }
            catch (DeactivatedUserException)
            {
                return new ForbidResult();
            }
        }

        [HttpGet("Workout/{workoutId}")]
        public IActionResult OnGetWorkout(long workoutId)
        {
            try
            {
                var workoutDto = WorkoutLogic.GetWorkoutById(workoutId);
                return new OkObjectResult(workoutDto);
            }
            catch (WorkoutDoesNotExistException)
            {
                return new NotFoundResult();
            }
            throw new NotImplementedException();
        }

        [HttpGet("Workouts/{userId}")]
        public IActionResult OnGetWorkouts(long userId)
        {
            var workoutsDto = WorkoutLogic.GetWorkoutsByUserId(userId);
            var workoutsData = new Dictionary<long, string>();
            foreach (var workout in workoutsDto)
            {
                workoutsData[workout.Id] = workout.Title;
            }
            return new OkObjectResult(workoutsData);
        }

        [HttpPost("Workout")]
        public IActionResult OnPostWorkout([FromBody]WorkoutDto workout)
        {
            string validationErrors = WorkoutService.ValidateWorkout(workout, true);
            if (!string.IsNullOrEmpty(validationErrors))
            {
                return new BadRequestObjectResult(validationErrors);
            }
            var workoutId = WorkoutLogic.AddWorkout(workout);
            return new OkObjectResult(workoutId);
        }

        [HttpPut("ArchiveWorkout/{workoutId}")]
        public IActionResult OnPutArchiveWorkout(long workoutId)
        {
            WorkoutLogic.ArchiveWorkout(workoutId);
            return new OkResult();
        }

        [HttpPut("UnarchiveWorkout/{workoutId}")]
        public IActionResult OnPutUnarchiveWorkout(long workoutId)
        {
            WorkoutLogic.UnarchiveWorkout(workoutId);
            return new OkResult();
        }

        [HttpPut("Username/{username}")]
        public IActionResult OnPutUsername(string username)
        {
            var headers = GetHeaders();
            if (string.IsNullOrEmpty(headers.UserIdentifier))
            {
                return new UnauthorizedResult();
            }
            try
            {
                var user = UserLogic.GetUser(headers.UserIdentifier, headers.Username);
                UserLogic.UpdateUsername(user, username);
                return new OkResult();
            }
            catch (DeactivatedUserException)
            {
                return new ForbidResult();
            }
        }

        [HttpPut("Workout")]
        public IActionResult OnPutWorkout([FromBody]WorkoutDto workout)
        {
            string validationErrors = WorkoutService.ValidateWorkout(workout);
            if (!string.IsNullOrEmpty(validationErrors))
            {
                return new BadRequestObjectResult(validationErrors);
            }
            WorkoutLogic.UpdateWorkout(workout);
            return new OkResult();  
        }
        
        private ApiHeader GetHeaders()
        {
            var headers = Request.Headers;

            StringValues identifier = string.Empty;
            headers.TryGetValue("UserIdentifier", out identifier);

            StringValues username = string.Empty;
            headers.TryGetValue("Username", out username);

            var apiHeader = new ApiHeader()
            {
                UserIdentifier = identifier.ToString(),
                Username = username.ToString()
            };

            return apiHeader;
        }
    }
}
