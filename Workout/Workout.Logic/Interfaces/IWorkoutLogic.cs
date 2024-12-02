using Workouts.Entities.Database;
using Workouts.Entities.Dto;

namespace Workouts.Logic.Interfaces
{
    public interface IWorkoutLogic
    {
        long AddWorkout(WorkoutDto workout);
        void ArchiveWorkout(long workoutId);
        Workout ConvertWorkoutDtoToWorkout(WorkoutDto workoutDto);
        WorkoutDto ConvertWorkoutToWorkoutDto(Workout workout);
        WorkoutDto GetSampleWorkoutDto();
        List<Workout> GetWorkoutsByUserId(long userId);
        WorkoutDto GetWorkoutById(long workoutId); 
        void UpdateWorkout(WorkoutDto workout);
        void UnarchiveWorkout(long workoutId);
        string ValidateWorkout(WorkoutDto workout);

    }
}
