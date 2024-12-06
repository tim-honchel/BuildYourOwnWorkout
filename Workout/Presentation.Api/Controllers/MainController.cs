using Microsoft.AspNetCore.Mvc;
using Workouts.Entities.Dto;
using Workouts.Logic.Interfaces;

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
            throw new NotImplementedException();
        }

        [HttpGet("Username")]
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
