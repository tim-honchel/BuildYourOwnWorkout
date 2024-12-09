using Workouts.Entities.Dto;

namespace Workouts.Presentation.Services
{
    public interface IApiCallService
    {
        Task<long> GetUserId();
        Task<string> GetUsername();
        Task<WorkoutDto> GetWorkout(long workoutId);
        Task<Dictionary<long, string>> GetWorkouts(long userId);
        Task<long> AddWorkout(WorkoutDto workout);
        Task ArchiveWorkout(long workoutId);
        Task UnarchiveWorkout(long workoutId);
        Task UpdateUsername(string username);
        Task UpdateWorkout(WorkoutDto workout);

    }
}
