using Workouts.Entities.Dto;

namespace Workouts.Presentation.Services
{
    public class ApiCallService : IApiCallService
    {
        private HttpContext Context;
        private HttpClient Client;

        public ApiCallService(HttpContext context)
        {
            Context = context;
            Client = new HttpClient();
        }

        public ApiCallService(HttpContext context, HttpMessageHandler handler)
        {
            Context = context;
            Client = new HttpClient(handler);
        }

        public long GetUserId()
        {
            throw new NotImplementedException();
        }

        public string GetUsername()
        {
            throw new NotImplementedException();
        }

        public WorkoutDto GetWorkout(long workoutId)
        {
            throw new NotImplementedException();
        }

        public Dictionary<long, string> GetWorkouts(long userId)
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
