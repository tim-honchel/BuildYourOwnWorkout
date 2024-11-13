using Microsoft.EntityFrameworkCore;
using Workouts.Data.Interfaces;
using Workouts.Entities.Database;

namespace Workouts.Data.Repositories
{
    public class WorkoutRepository : IWorkoutRepository
    {
        public IDbContextFactory<Context> ContextFactory { get; set; }

        public WorkoutRepository(IDbContextFactory<Context> contextFactory)
        {
            ContextFactory = contextFactory;
        }

        public void AddWorkout(Workout workout)
        {
            throw new NotImplementedException();
        }

        public Workout GetWorkoutById(long id)
        {
            throw new NotImplementedException();
        }

        public List<Workout> GetWorkoutsByUserId(long userId)
        {
            throw new NotImplementedException();
        }

        public void UpdateWorkout(Workout workout)
        {
            throw new NotImplementedException();
        }
    }
}
