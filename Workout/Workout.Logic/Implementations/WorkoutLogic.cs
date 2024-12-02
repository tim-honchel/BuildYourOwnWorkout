using Newtonsoft.Json;
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

        public Workout ConvertWorkoutDtoToWorkout(WorkoutDto workoutDto)
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

        public WorkoutDto ConvertWorkoutToWorkoutDto(Workout workout)
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

        public WorkoutDto GetSampleWorkoutDto()
        {
            var workout = new WorkoutDto();

            workout.Id = -1;

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

        public string ValidateWorkout(WorkoutDto workout)
        {
            throw new NotImplementedException();
        }
    }
}
