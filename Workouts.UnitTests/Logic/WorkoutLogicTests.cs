using Moq;
using Workouts.Data.Interfaces;
using Workouts.Entities.CustomExceptions;
using Workouts.Entities.Database;
using Workouts.Entities.Dto;
using Workouts.Logic.Implementations;
using Workouts.Services;

namespace Workouts.UnitTests.Logic
{
    public class WorkoutLogicTests : IDisposable
    {
        private WorkoutDto SampleWorkoutDto { get; set; }
        private Workout SampleWorkout { get; set; }
        private Workout DeactivatedWorkout { get; set; }
        private List<Workout> SampleWorkouts { get; set; } = new List<Workout>();
        private List<WorkoutDto> SampleWorkoutsDto { get; set; } = new List<WorkoutDto>();
        private Mock<IWorkoutRepository> MockRepository { get; set; }
        private WorkoutLogic Logic { get; set; }

        public WorkoutLogicTests()
        {
            SampleWorkoutDto = WorkoutService.GetSampleWorkoutDto();
            SampleWorkout = WorkoutService.ConvertWorkoutDtoToWorkout(SampleWorkoutDto);
            SampleWorkout.UserId = 1;
            SampleWorkouts = new List<Workout>() { SampleWorkout };
            SampleWorkoutsDto = new List<WorkoutDto>() { SampleWorkoutDto };
            DeactivatedWorkout = GetDeactivatedWorkout();

            MockRepository = new Mock<IWorkoutRepository>();
            MockRepository.Setup(m => m.GetWorkoutById(It.Is<long>(i => i == SampleWorkoutDto.Id))).Returns(SampleWorkout);
            MockRepository.Setup(m => m.GetWorkoutById(It.Is<long>(i => i == DeactivatedWorkout.Id))).Returns(DeactivatedWorkout);
            MockRepository.Setup(m => m.GetWorkoutsByUserId(It.Is<long>(i => i == SampleWorkout.UserId))).Returns(SampleWorkouts);
            Logic = new WorkoutLogic(MockRepository.Object);
        }

        public void Dispose()
        {
            MockRepository.Reset();
        }

        private Workout GetDeactivatedWorkout()
        {
            var deactivatedWorkout = WorkoutService.ConvertWorkoutDtoToWorkout(SampleWorkoutDto);
            deactivatedWorkout.Id = -2;
            deactivatedWorkout.UserId = 2;
            deactivatedWorkout.Active = false;
            return deactivatedWorkout;
        }

        [Fact]
        public void AddWorkout_AddsWorkout()
        {
            Logic.AddWorkout(SampleWorkoutDto);

            MockRepository.Verify(m => m.AddWorkout(It.Is<Workout>(w => w.Id == SampleWorkout.Id)), Times.Once());
        }

        [Fact]
        public void ArchiveWorkout_UpdatesWorkoutToInactive()
        {
            Logic.ArchiveWorkout(SampleWorkoutDto.Id);

            MockRepository.Verify(m => m.UpdateWorkout(It.Is<Workout>(w => w.Id == SampleWorkout.Id && w.Active == false)), Times.Once());
        }

        [Fact]
        public void GetWorkoutById_ReturnsWorkout_GivenExistingWorkout()
        {
            var returnedWorkout = Logic.GetWorkoutById(SampleWorkoutDto.Id);

            Assert.Equivalent(returnedWorkout, SampleWorkoutDto);
        }

        [Fact]
        public void GetWorkoutById_ThrowsCustomException_GivenNonexistentWorkout()
        {
            long nonexistentWorkoutId = 999;

            Assert.Throws<WorkoutDoesNotExistException>( () => Logic.GetWorkoutById(nonexistentWorkoutId));
        }

        [Fact]
        public void GetWorkoutsByUserId_ReturnsWorkouts_GivenExistingUserId()
        {
            var returnedWorkouts = Logic.GetWorkoutsByUserId(SampleWorkout.UserId);

            Assert.Equivalent(returnedWorkouts, SampleWorkoutsDto);
        }

        [Fact]
        public void GetWorkoutsByUserId_ReturnsEmptyList_GivenUserIdWithNoWorkouts()
        {
            long nonexistentUserId = 999;

            var returnedWorkouts = Logic.GetWorkoutsByUserId(nonexistentUserId);

            Assert.True(returnedWorkouts.Count() == 0);
        }

        [Fact]
        public void UnarchiveWorkout_UpdatesWorkout()
        {
            Logic.UnarchiveWorkout(DeactivatedWorkout.Id);

            MockRepository.Verify(m => m.UpdateWorkout(It.Is<Workout>(w => w.Id == DeactivatedWorkout.Id && w.Active == true)), Times.Once());
        }

        [Fact]
        public void UpdateWorkout_UpdatesWorkout()
        {
            var workoutToUpdate = WorkoutService.GetSampleWorkoutDto();
            workoutToUpdate.Title = "updatedTitle";

            Logic.UpdateWorkout(workoutToUpdate);

            MockRepository.Verify(m => m.UpdateWorkout(It.Is<Workout>(w => w.Id == workoutToUpdate.Id && w.Title == workoutToUpdate.Title)), Times.Once());
        }
  
    }
}
