using Workouts.Entities.Dto;

namespace Workouts.Presentation.Services
{
    public interface IApiCallService
    {
        long GetUserId();
        string GetUsername();
        WorkoutDto GetWorkout(long workoutId);
        Dictionary<long, string> GetWorkouts(long userId);
        long AddWorkout(WorkoutDto workout);
        void ArchiveWorkout(long workoutId);
        void UnarchiveWorkout(long workoutId);
        void UpdateUsername(string username);
        void UpdateWorkout(WorkoutDto workout);

    }
}
