﻿using Microsoft.EntityFrameworkCore;
using Workouts.Data.Interfaces;
using Workouts.Entities.Database;

namespace Workouts.Data.Repositories
{
    public class WorkoutRepository : IWorkoutRepository
    {
        private IDbContextFactory<Context> _contextFactory;

        public WorkoutRepository(IDbContextFactory<Context> contextFactory)
        {
            _contextFactory = contextFactory;
        }

        public long AddWorkout(Workout workout)
        {
            using var context = _contextFactory.CreateDbContext();
            context.Workout.Add(workout);
            context.SaveChanges();
            context.Dispose();
            return workout.Id;
        }

        public Workout GetWorkoutById(long id)
        {
            using var context = _contextFactory.CreateDbContext();
            var workout = context.Workout
                .FirstOrDefault(w => w.Id == id);
            context.Dispose();
            return workout;
        }

        public List<Workout> GetWorkoutsByUserId(long userId)
        {
            using var context = _contextFactory.CreateDbContext();
            var workouts = context.Workout
                .Where(w => w.UserId == userId)
                .ToList();
            context.Dispose();
            return workouts;
        }

        public void UpdateWorkout(Workout workout)
        {
            using var context = _contextFactory.CreateDbContext();
            context.Workout.Update(workout);
            context.SaveChanges();
            context.Dispose();
        }
    }
}
