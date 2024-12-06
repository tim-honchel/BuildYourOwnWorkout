using Workouts.Entities.Database;
using Workouts.Entities.Dto;

namespace Workouts.Logic.Interfaces
{
    public interface IWorkoutLogic
    {
        long AddWorkout(WorkoutDto workoutDto);
        void ArchiveWorkout(long workoutId);
        List<WorkoutDto> GetWorkoutsByUserId(long userId);
        WorkoutDto GetWorkoutById(long workoutId); 
        void UpdateWorkout(WorkoutDto workoutDto);
        void UnarchiveWorkout(long workoutId);
        string ValidateWorkout(WorkoutDto workoutDto);

    }
}
