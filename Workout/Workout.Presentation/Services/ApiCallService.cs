using Workouts.Entities.Dto;

namespace Workouts.Presentation.Services
{
    public class ApiCallService : IApiCallService
    {
        private HttpContext Context { get; set; }
        public ApiCallService(HttpContext context)
        {
            Context = context;
        }
        public long GetUserId()
        {
            throw new NotImplementedException();
        }

        public long GetUsername()
        {
            throw new NotImplementedException();
        }

        public WorkoutDto GetWorkout(long workoutId)
        {
            throw new NotImplementedException();
        }

        public List<WorkoutDto> GetWorkouts(long userId)
        {
            throw new NotImplementedException();
        }

        public long AddWorkout(WorkoutDto workout)
        {
            throw new NotImplementedException();
        }

        public void ArchiveWorkout(long workoutId)
        {
            throw new NotImplementedException();
        }

        public void UnarchiveWorkout(long workoutId)
        {
            throw new NotImplementedException();
        }

        public void UpdateUsername(string username)
        {
            throw new NotImplementedException();
        }

        public void UpdateWorkout(WorkoutDto workout)
        {
            throw new NotImplementedException();
        }
    }
}
