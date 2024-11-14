using Microsoft.EntityFrameworkCore;
using Moq;
using Workouts.Data;
using Workouts.Data.Repositories;
using Workouts.Entities.Database;

namespace Workouts.UnitTests.Data
{
    public class UserRepositoryTests : IDisposable
    {
        private IQueryable<User> ExistingUsers;
        private Mock<DbSet<User>> MockSet;
        private Mock<Context> MockContext;
        private Mock<IDbContextFactory<Context>> MockFactory;
        private UserRepository Repository;

        public UserRepositoryTests() 
        {
            ExistingUsers = GetSampleUsers();
            MockSet = new Mock<DbSet<User>>();
            MockSet.As<IQueryable<User>>().Setup(m => m.Provider).Returns(ExistingUsers.Provider);
            MockSet.As<IQueryable<User>>().Setup(m => m.Expression).Returns(ExistingUsers.Expression);
            MockSet.As<IQueryable<User>>().Setup(m => m.ElementType).Returns(ExistingUsers.ElementType);
            MockSet.As<IQueryable<User>>().Setup(m => m.GetEnumerator()).Returns(ExistingUsers.GetEnumerator());
            MockContext = new Mock<Context>(new DbContextOptionsBuilder<Context>().Options);
            MockContext.Setup(c => c.User).Returns(MockSet.Object);
            MockFactory = new Mock<IDbContextFactory<Context>>();
            MockFactory.Setup(f => f.CreateDbContext()).Returns(MockContext.Object);
            Repository = new UserRepository(MockFactory.Object);
        }

        public void Dispose()
        {
            MockSet.Reset();
            MockContext.Reset();
        }

        private IQueryable<User> GetSampleUsers()
        {
            return new List<User>()
            {
                new User()
                {
                    Id = 1,
                    NameIdentifierClaim = "testIdentifier1",
                    Username = "activeUser1",
                    Active = true,
                    AccountCreated = new DateTime(2024,1,1),
                    LastLogin = new DateTime(2024,2,1)
                },
                new User()
                {
                    Id = 2,
                    NameIdentifierClaim = "testIdentifier2",
                    Username = "inactiveUser2",
                    Active = false,
                    AccountCreated = new DateTime(2024,1,1),
                    LastLogin = new DateTime(2024,2,1)
                }
            }.AsQueryable();
        }

        [Fact]
        public void AddUser_AddsUser()
        {
            var newUser = new User()
            {
                NameIdentifierClaim = "testIdentifier",
                Username = "testUsername",
                Active = true,
                AccountCreated = DateTime.Today,
                LastLogin = DateTime.Today
            };

            Repository.AddUser(newUser);

            MockSet.Verify(c => c.Add(It.Is<User>(u => u.NameIdentifierClaim == newUser.NameIdentifierClaim)), Times.Once());
            MockContext.Verify(c => c.SaveChanges(), Times.Once());
        }

        [Fact]
        public void GetUser_ReturnsUser_GivenExistingIdentifier()
        {
            var existingUser = ExistingUsers.First();

            var returnedUser = Repository.GetUser(existingUser.NameIdentifierClaim);

            Assert.NotNull(returnedUser);
        }

        [Fact]
        public void GetUser_ReturnsUserWithAllProperties_GivenExistingIdentifier()
        {
            var existingUser = ExistingUsers.First();

            var returnedUser = Repository.GetUser(existingUser.NameIdentifierClaim);

            Assert.Equivalent(returnedUser, existingUser);
        }

        [Fact]
        public void GetUser_ReturnsNull_GivenNonexistentIdentifier()
        {
            string nonexistentIdentifier = "nonexistentIdentifier";

            var returnedUser = Repository.GetUser(nonexistentIdentifier);

            Assert.Null(returnedUser);
        }

        [Fact]
        public void UpdateUser_UpdatesUser()
        {
            var userToUpdate = ExistingUsers.First(u => u.Active == false);
            userToUpdate.Active = true;
            userToUpdate.Username = "changedUsername";
            userToUpdate.LastLogin = DateTime.Today;
            
            Repository.UpdateUser(userToUpdate);

            MockSet.Verify(c => c.Update(It.Is<User>(u => u.NameIdentifierClaim == userToUpdate.NameIdentifierClaim)), Times.Once());
            MockContext.Verify(c => c.SaveChanges(), Times.Once());
        }
    
    }
}
