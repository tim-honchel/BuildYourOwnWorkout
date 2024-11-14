using Workouts.Entities.Database;
using Workouts.Entities.Dto;

namespace Workouts.Logic.Interfaces
{
    public interface IWorkoutLogic
    {
        long AddWorkout(WorkoutDto workout);
        void ArchiveWorkout(long workoutId);
        List<Workout> GetWorkoutsByUserId(long userId);
        WorkoutDto GetWorkoutById(long workoutId); 
        void UpdateWorkout(WorkoutDto workout);
        void UnarchiveWorkout(long workoutId);

    }
}
