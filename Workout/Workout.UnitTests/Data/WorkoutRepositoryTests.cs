using Microsoft.EntityFrameworkCore;
using Moq;
using Workouts.Data.Repositories;
using Workouts.Data;
using Workouts.Entities.Database;

namespace Workouts.UnitTests.Data
{
    public class WorkoutRepositoryTests : IDisposable
    {
        private IQueryable<Workout> ExistingWorkouts;
        private Mock<DbSet<Workout>> MockSet;
        private Mock<Context> MockContext;
        private Mock<IDbContextFactory<Context>> MockFactory;
        private WorkoutRepository Repository;

        public WorkoutRepositoryTests() 
        {
            ExistingWorkouts = GetSampleWorkouts();
            MockSet = new Mock<DbSet<Workout>>();
            MockSet.As<IQueryable<Workout>>().Setup(m => m.Provider).Returns(ExistingWorkouts.Provider);
            MockSet.As<IQueryable<Workout>>().Setup(m => m.Expression).Returns(ExistingWorkouts.Expression);
            MockSet.As<IQueryable<Workout>>().Setup(m => m.ElementType).Returns(ExistingWorkouts.ElementType);
            MockSet.As<IQueryable<Workout>>().Setup(m => m.GetEnumerator()).Returns(ExistingWorkouts.GetEnumerator());
            MockContext = new Mock<Context>(new DbContextOptionsBuilder<Context>().Options);
            MockContext.Setup(c => c.Workout).Returns(MockSet.Object);
            MockFactory = new Mock<IDbContextFactory<Context>>();
            MockFactory.Setup(f => f.CreateDbContext()).Returns(MockContext.Object);
            Repository = new WorkoutRepository(MockFactory.Object);
        }

        public void Dispose()
        {
            MockSet.Reset();
            MockContext.Reset();
            MockFactory.Reset();
        }

        private IQueryable<Workout> GetSampleWorkouts()
        {
            return new List<Workout>()
            {
                new Workout()
                {
                    Id = 1,
                    Active = true,
                    Title = "testTitle1",
                    ExercisesJson = "{}",
                    SettingsJson = "{}",
                    UserId = 1
                },
                new Workout()
                {
                    Id = 2,
                    Active = true,
                    Title = "testTitle2",
                    ExercisesJson = "{}",
                    SettingsJson = "{}",
                    UserId = 1
                }
            }.AsQueryable();
        }

        [Fact]
        public void AddWorkout_AddsWorkout()
        {
            var newWorkout = new Workout()
            {
                Active = true,
                Title = "testTitle",
                ExercisesJson = "{}",
                SettingsJson = "{}",
                UserId = 1
            };

            Repository.AddWorkout(newWorkout);

            MockSet.Verify(c => c.Add(It.Is<Workout>(u => u.Title == newWorkout.Title)), Times.Once());
            MockContext.Verify(c => c.SaveChanges(), Times.Once());
        }

        [Fact]
        public void GetWorkoutById_ReturnsWorkout_GivenExistingId()
        {
            var existingWorkout = ExistingWorkouts.First();

            var returnedWorkout = Repository.GetWorkoutById(existingWorkout.Id);

            Assert.NotNull(returnedWorkout);
        }

        [Fact]
        public void GetWorkoutById_ReturnsWorkoutWithAllProperties_GivenExistingId()
        {
            var existingWorkout = ExistingWorkouts.First();

            var returnedWorkout = Repository.GetWorkoutById(existingWorkout.Id);

            Assert.Equivalent(returnedWorkout, existingWorkout);
        }

        [Fact]
        public void GetWorkoutById_ReturnsNull_GivenNonexistentId()
        {
            long nonexistentId = 999;

            var returnedWorkout = Repository.GetWorkoutById(nonexistentId);

            Assert.Null(returnedWorkout);
        }

        [Fact]
        public void GetWorkoutsByUserId_ReturnsWorkouts_GivenUserIdWithWorkouts()
        {
            long userId = ExistingWorkouts.First().UserId;

            var returnedWorkouts = Repository.GetWorkoutsByUserId(userId);

            Assert.NotNull(returnedWorkouts);
        }

        [Fact]
        public void GetWorkoutsByUserId_ReturnsAllMatchingWorkouts_GivenUserIdWithWorkouts()
        {
            long userId = ExistingWorkouts.First().UserId;

            var returnedWorkouts = Repository.GetWorkoutsByUserId(userId);

            Assert.Equivalent(returnedWorkouts, ExistingWorkouts.Where(w => w.UserId == userId));
        }

        [Fact]
        public void GetWorkoutsByUserId_ReturnsEmpty_GivenUserIdWithoutWorkouts()
        {
            long nonexistentUserId = 999;

            var returnedWorkouts = Repository.GetWorkoutsByUserId(nonexistentUserId);

            Assert.Empty(returnedWorkouts);
        }

        [Fact]
        public void UpdateWorkout_UpdatesWorkout()
        {
            var workoutToUpdate = ExistingWorkouts.First();
            workoutToUpdate.Title = "changedTitle";

            Repository.UpdateWorkout(workoutToUpdate);

            MockSet.Verify(c => c.Update(It.Is<Workout>(u => u.Title == workoutToUpdate.Title)), Times.Once());
            MockContext.Verify(c => c.SaveChanges(), Times.Once());
        }
    }
}
