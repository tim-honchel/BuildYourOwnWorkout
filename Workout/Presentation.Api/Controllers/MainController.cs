using Microsoft.AspNetCore.Mvc;
using Workouts.Entities.Dto;
using Workouts.Logic.Implementations;

namespace Workouts.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class MainController : ControllerBase
    {
        private UserLogic UserLogic { get; set; }
        private WorkoutLogic WorkoutLogic { get; set; }

        public MainController(UserLogic userLogic, WorkoutLogic workoutLogic) 
        {
            UserLogic = userLogic;
            WorkoutLogic = workoutLogic;
        }

        // use headers for authentication and authorization

        [HttpGet("CurrentUserId")]
        public IActionResult OnGetCurrentUserId()
        {
            throw new NotImplementedException();
        }

        [HttpGet("CurrentUsername")]
        public IActionResult OnGetUsername()
        {
            throw new NotImplementedException();
        }

        [HttpGet("Workout/{workoutId}")]
        public IActionResult OnGetWorkout(long workoutId)
        {
            throw new NotImplementedException();
        }

        [HttpGet("Workouts/{userId}")]
        public IActionResult OnGetWorkouts(long userId)
        {
            throw new NotImplementedException();
        }

        [HttpPost("AddWorkout")]
        public IActionResult OnPostAddWorkout([FromBody]WorkoutDto workout)
        {
            throw new NotImplementedException();
        }

        [HttpPost("ValidateWorkout")]
        public IActionResult OnPostValidateWorkout([FromBody]WorkoutDto workout)
        {
            throw new NotImplementedException();
        }

        [HttpPut("ArchiveWorkout/{workoutId}")]
        public IActionResult OnPutArchiveWorkout(long workoutId)
        {
            throw new NotImplementedException();
        }

        [HttpPut("UnarchiveWorkout/{workoutId}")]
        public IActionResult OnPutUnarchiveWorkout(long workoutId)
        {
            throw new NotImplementedException();
        }

        [HttpPut("UpdateUsername/{username}")]
        public IActionResult OnPutUpdateUsername(string username)
        {
            throw new NotImplementedException();
        }

        [HttpPut("UpdateWorkout")]
        public IActionResult OnPutUpdateWorkout([FromBody]WorkoutDto workout)
        {
            throw new NotImplementedException(); 
        }
        
    }
}
