using Microsoft.EntityFrameworkCore;
using Moq;
using Moq.EntityFrameworkCore;
using Workouts.Data;
using Workouts.Data.Repositories;
using Workouts.Entities.Database;
using Xunit;

namespace Workouts.UnitTests.Data
{
    public class UserRepositoryTests : IDisposable
    {
        private List<User> ExistingUsers;
        private Mock<Context> MockContext;
        private UserRepository Repository;

        public UserRepositoryTests() 
        {
            ExistingUsers = GetSampleUsers();
            MockContext = new Mock<Context>(new DbContextOptionsBuilder<Context>().Options);
            MockContext.Setup(c => c.User).ReturnsDbSet(ExistingUsers);
            Repository = new UserRepository(MockContext.Object);

        }

        public void Dispose()
        {
            MockContext.Reset();
        }
        public List<User> GetSampleUsers()
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
            };
        }

        [Fact]
        public void AddUser_AddsUser()
        {
            var newUser = new User()
            {
                NameIdentifierClaim = "testIdentifier",
            };

            Repository.AddUser(newUser);

            var wasUserAddedToDb = MockContext.Object.User.Any(
                u => u.NameIdentifierClaim == newUser.NameIdentifierClaim);
            Assert.True(wasUserAddedToDb);
        }

        [Fact]
        public void AddUser_AddsUserWithAllProperties()
        {
            var newUser = new User()
            {
                NameIdentifierClaim = "testIdentifier",
                Username = "testUsername"
            };

            Repository.AddUser(newUser);      

            var wasUserAddedToDb = MockContext.Object.User.Any(
                u => u.NameIdentifierClaim == newUser.NameIdentifierClaim
                && u.Username == newUser.Username
                && u.Active == true
                && u.AccountCreated == DateTime.Today
                && u.LastLogin == DateTime.Today
                && u.Id > 0);
            Assert.True(wasUserAddedToDb);
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
        public void UpdateUser_UpdatesUser_GivenChangedProperties()
        {
            var userToUpdate = ExistingUsers.First(u => u.Active == false);
            userToUpdate.Active = true;
            userToUpdate.Username = "changedUsername";
            userToUpdate.LastLogin = DateTime.Today;
            
            Repository.UpdateUser(userToUpdate);

            var userInDb = MockContext.Object.User.Where(u => u.Id == userToUpdate.Id);
            Assert.Equivalent(userInDb, userToUpdate);
        }
    
    }
}
