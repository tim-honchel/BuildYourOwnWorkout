using Workouts.Data.Interfaces;
using Workouts.Entities.Database;
using Workouts.Entities.Dto;
using Workouts.Logic.Interfaces;

namespace Workouts.Logic.Implementations
{
    public class WorkoutLogic : IWorkoutLogic
    {
        private IWorkoutRepository Repository { get; set; }

        public WorkoutLogic(IWorkoutRepository repository)
        {
            Repository = repository;
        }
        public long AddWorkout(WorkoutDto workout)
        {
            throw new NotImplementedException();
        }

        public void ArchiveWorkout(long workoutId)
        {
            throw new NotImplementedException();
        }

        public WorkoutDto GetWorkoutById(long workoutId)
        {
            throw new NotImplementedException();
        }

        public List<Workout> GetWorkoutsByUserId(long userId)
        {
            throw new NotImplementedException();
        }

        public void UnarchiveWorkout(long workoutId)
        {
            throw new NotImplementedException();
        }

        public void UpdateWorkout(WorkoutDto workout)
        {
            throw new NotImplementedException();
        }
    }
}
