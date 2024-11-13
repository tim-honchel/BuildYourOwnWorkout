﻿using Workouts.Entities.Database;

namespace Workouts.Data.Interfaces
{
    public interface IWorkoutRepository
    {
        void AddWorkout(Workout workout);
        Workout GetWorkoutById(long id);
        List<Workout> GetWorkoutsByUserId(long userId);
        void UpdateWorkout(Workout workout);
    }
}
