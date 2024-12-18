﻿using Newtonsoft.Json;
using System.Text.RegularExpressions;
using Workouts.Entities.Database;
using Workouts.Entities.Dto;

namespace Workouts.Services
{
    public static class WorkoutService
    {
        public static Workout ConvertWorkoutDtoToWorkout(WorkoutDto workoutDto)
        {
            var settings = new WorkoutSettings()
            {
                AudioEndExercise = workoutDto.AudioEndExercise,
                AudioSecondTick = workoutDto.AudioSecondTick,
                AudioSpeakExercise = workoutDto.AudioSpeakExercise,
                AudioSpeakGo = workoutDto.AudioSpeakGo,
                TransitionTime = workoutDto.TransitionTime
            };

            return new Workout()
            {
                Id = workoutDto.Id,
                Title = workoutDto.Title,
                ExercisesJson = JsonConvert.SerializeObject(workoutDto.Exercises),
                SettingsJson = JsonConvert.SerializeObject(settings)
            };
        }

        public static WorkoutDto ConvertWorkoutToWorkoutDto(Workout workout)
        {
            var settings = JsonConvert.DeserializeObject<WorkoutSettings>(workout.SettingsJson) ?? new WorkoutSettings();

            var workoutDto = new WorkoutDto()
            {
                Id = workout.Id,
                Title = workout.Title,
                Exercises = JsonConvert.DeserializeObject<List<Exercise>>(workout.ExercisesJson) ?? new List<Exercise>(),
                AudioEndExercise = settings.AudioEndExercise,
                AudioSecondTick = settings.AudioSecondTick,
                AudioSpeakExercise = settings.AudioSpeakExercise,
                AudioSpeakGo = settings.AudioSpeakGo,
                TransitionTime = settings.TransitionTime
            };
            return workoutDto;
        }
        public static WorkoutDto GetSampleWorkoutDto()
        {
            var workout = new WorkoutDto();

            workout.Id = -1;
            workout.Title = "Sample Workout";

            workout.Exercises = new List<Exercise>()
                {
                    new Exercise() {Name = "Arm Swings", Time = 10, Order = 0},
                    new Exercise() {Name = "Forward Arm Circles", Time = 10, Order = 1},
                    new Exercise() {Name = "Reverse Arm Circles", Time = 10, Order = 2},
                    new Exercise() {Name = "Right Leg Circles", Time = 10, Order = 3},
                    new Exercise() {Name = "Left Leg Circles", Time = 10, Order = 4},
                    new Exercise() {Name = "Right Leg Kicks", Time = 10, Order = 5},
                    new Exercise() {Name = "Left Leg Kicks", Time = 10, Order = 6},
                    new Exercise() {Name = "Deep Sit Bend", Time = 10, Order = 7},
                    new Exercise() {Name = "Push Ups", Time = 30, Order = 8},
                    new Exercise() {Name = "Sit Ups", Time = 30, Order = 9},
                    new Exercise() {Name = "Squats", Time = 30, Order = 10},
                    new Exercise() {Name = "Jumping Jacks", Time = 30, Order = 11},
                    new Exercise() {Name = "Deep Sit Bend", Time = 10, Order = 12},
                    new Exercise() {Name = "Push Ups", Time = 30, Order = 13},
                    new Exercise() {Name = "Sit Ups", Time = 30, Order = 14},
                    new Exercise() {Name = "Squats", Time = 30, Order = 15},
                    new Exercise() {Name = "Jumping Jacks", Time = 30, Order = 16}
                };
            workout.AudioEndExercise = true;
            workout.AudioSecondTick = true;
            workout.AudioSpeakExercise = true;
            workout.AudioSpeakGo = true;
            workout.TransitionTime = 2;

            return workout;
        }

        public static string ValidateWorkout(WorkoutDto workoutDto, bool isNew = false)
        {
            string validationErrorMessage = string.Empty;

            if (workoutDto == null)
            {
                validationErrorMessage = "Workout data is empty or was not received.";
                return validationErrorMessage;
            }
            if ((isNew
                && workoutDto.Id != 0))
            {
                validationErrorMessage += "New workout cannot contain a preassigned ID";
            }
            if (string.IsNullOrEmpty(workoutDto.Title))
            {
                validationErrorMessage += "The workout title is blank.";
            }
            if (!Regex.Match(workoutDto.Title, "^[a-zA-Z0-9-_'.,;?!#$%&* ]*$").Success)
            {
                validationErrorMessage += "The workout title is invalid.";
            }
            if (workoutDto.Exercises == null || workoutDto.Exercises.Count == 0)
            {
                validationErrorMessage += "The workout does not contain any exercises.";
            }
            if (workoutDto.TransitionTime < 0 || workoutDto.TransitionTime > 10)
            {
                validationErrorMessage += "Transition time value is invalid.";
            }

            return validationErrorMessage;
        }
    }
}
