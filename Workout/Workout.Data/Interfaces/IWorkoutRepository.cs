using Workouts.Entities.Database;

namespace Workouts.Data.Interfaces
{
    public interface IWorkoutRepository
    {
        void AddWorkout(Workout workout);
        Workout GetWorkoutById(int id);
        List<Workout> GetWorkoutsByUserId(int userId);
        void UpdateWorkout(Workout workout);
    }
}
