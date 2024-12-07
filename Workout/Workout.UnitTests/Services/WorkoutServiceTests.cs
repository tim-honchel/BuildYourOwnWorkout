using System.Collections;
using Workouts.Entities.Database;
using Workouts.Entities.Dto;
using Workouts.Services;

namespace Workouts.UnitTests.Services
{
    public class WorkoutServiceTests
    {
        private WorkoutDto SampleWorkoutDto { get; set; }
        private Workout SampleWorkout { get; set; }

        public WorkoutServiceTests() 
        {
            SampleWorkoutDto = WorkoutService.GetSampleWorkoutDto();
            SampleWorkout = WorkoutService.ConvertWorkoutDtoToWorkout(SampleWorkoutDto);
        }

        [Fact]
        public void ConvertWorkoutToWorkoutDto_ReturnsEquivalentWorkout()
        {
            var returnedWorkoutDto = WorkoutService.ConvertWorkoutToWorkoutDto(SampleWorkout);

            Assert.Equivalent(returnedWorkoutDto, SampleWorkoutDto);
        }

        [Fact]
        public void ConvertWorkoutDtoToWorkout_ReturnsEquivalentWorkoutDto()
        {
            var returnedWorkout = WorkoutService.ConvertWorkoutDtoToWorkout(SampleWorkoutDto);

            Assert.Equivalent(returnedWorkout, SampleWorkout);
        }

        [Fact]
        public void GetSampleWorkoutDto_ReturnsWorkoutDtoWithMultipleExercises()
        {
            var returnedWorkoutDto = (WorkoutDto)WorkoutService.GetSampleWorkoutDto();

            Assert.True(returnedWorkoutDto.Exercises.Count() > 1);
        }

        [Fact]
        public void ValidateWorkout_ReturnsEmptyString_GivenValidWorkout()
        {
            string validationErrors = WorkoutService.ValidateWorkout(SampleWorkoutDto);

            Assert.True(validationErrors == string.Empty);
        }

        [Fact]
        public void ValidateWorkout_ReturnsNonEmptyString_GivenNewWorkoutWithId()
        {
            string validationErrors = WorkoutService.ValidateWorkout(SampleWorkoutDto, true);

            Assert.True(validationErrors != string.Empty);
        }

        [Theory]
        [MemberData(nameof(TestDataGenerator.GetInvalidWorkoutDtos), MemberType = typeof(TestDataGenerator))]
        public void ValidateWorkout_ReturnsNonEmptyString_GivenInvalidWorkout(WorkoutDto invalidWorkoutDto)
        {
            string validationErrors = WorkoutService.ValidateWorkout(invalidWorkoutDto);

            Assert.True(validationErrors != string.Empty);
        }
    }


    public class TestDataGenerator : IEnumerable<object[]>
    {
        public IEnumerator<object[]> GetEnumerator()
        {
            throw new NotImplementedException();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            throw new NotImplementedException();
        }

        public static IEnumerable<object[]> GetInvalidWorkoutDtos()
        {
            yield return new object[] {
                new WorkoutDto { Title = string.Empty, Exercises = new List<Exercise>() { new Exercise()}, TransitionTime = 1 }, // empty title
            };
            yield return new object[]
            {
                new WorkoutDto { Title = "<invalid>", Exercises = new List<Exercise>() { new Exercise()}, TransitionTime = 1 }, // invalid title
            };
            yield return new object[]
            {
                new WorkoutDto { Title = "title", Exercises = new List<Exercise>() {}, TransitionTime = 1 }, // no exercises
            };
            yield return new object[]
            {
                new WorkoutDto { Title = "title", Exercises = new List<Exercise>() { new Exercise()}, TransitionTime = 100 }, // invalid transition time
            };
        }
    }
}
