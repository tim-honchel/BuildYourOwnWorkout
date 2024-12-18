﻿using Workouts.Data.Interfaces;
using Workouts.Entities.CustomExceptions;
using Workouts.Entities.Database;
using Workouts.Entities.Dto;
using Workouts.Logic.Interfaces;
using Workouts.Services;

namespace Workouts.Logic.Implementations
{
    public class WorkoutLogic : IWorkoutLogic
    {
        private IWorkoutRepository _repository;

        public WorkoutLogic(IWorkoutRepository repository)
        {
            _repository = repository;
        }
        public long AddWorkout(WorkoutDto workoutDto)
        {
            var workout = WorkoutService.ConvertWorkoutDtoToWorkout(workoutDto);
            return _repository.AddWorkout(workout);
        }

        public void ArchiveWorkout(long workoutId)
        {
            var workout = _repository.GetWorkoutById(workoutId);
            workout.Active = false;
            _repository.UpdateWorkout(workout);
        }

        public WorkoutDto GetWorkoutById(long workoutId)
        {
            var workout = _repository.GetWorkoutById(workoutId);
            if (workout == null)
            {
                throw new WorkoutDoesNotExistException();
            }
            var workoutDto = WorkoutService.ConvertWorkoutToWorkoutDto(workout);
            return workoutDto;
        }

        public List<WorkoutDto> GetWorkoutsByUserId(long userId)
        {
            var workouts = _repository.GetWorkoutsByUserId(userId) ?? new List<Workout>();
            var workoutDtos = new List<WorkoutDto>();
            foreach (var workout in workouts)
            {
                var workoutDto = WorkoutService.ConvertWorkoutToWorkoutDto(workout);
                workoutDtos.Add(workoutDto);
            }
            return workoutDtos;
        }

        public void UnarchiveWorkout(long workoutId)
        {
            var workout = _repository.GetWorkoutById(workoutId);
            workout.Active = true;
            _repository.UpdateWorkout(workout);
        }

        public void UpdateWorkout(WorkoutDto workoutDto)
        {
            var workout = WorkoutService.ConvertWorkoutDtoToWorkout(workoutDto);
            _repository.UpdateWorkout(workout);
        }

        
    }
}
