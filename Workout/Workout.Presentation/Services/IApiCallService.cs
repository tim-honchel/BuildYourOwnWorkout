using Workouts.Entities.Dto;

namespace Workouts.Presentation.Services
{
    public interface IApiCallService
    {
        long GetUserId();
        long GetUsername();
        WorkoutDto GetWorkout(long workoutId);
        List<WorkoutDto> GetWorkouts(long userId);
        long AddWorkout(WorkoutDto workout);
        void ArchiveWorkout(long workoutId);
        void UnarchiveWorkout(long workoutId);
        void UpdateUsername(string username);
        void UpdateWorkout(WorkoutDto workout);

    }
}
