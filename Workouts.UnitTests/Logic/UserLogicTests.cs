using Microsoft.AspNetCore.Http;
using Moq;
using System.Security.Claims;
using System.Security.Principal;
using Workouts.Data.Interfaces;
using Workouts.Entities.CustomExceptions;
using Workouts.Entities.Database;
using Workouts.Logic.Implementations;

namespace Workouts.UnitTests.Logic
{
    public class UserLogicTests : IDisposable
    {
        private User DeactivatedUser { get; set; }
        private User ExistingUser { get; set; }
        private User NewUser { get; set; }
        private Mock<IUserRepository> MockRepository { get; set; }
        private UserLogic Logic { get; set; }

        public UserLogicTests() 
        { 
            ExistingUser = GetExistingUser();
            NewUser = GetNewUser();
            DeactivatedUser = GetDeactivatedUser();

            MockRepository = new Mock<IUserRepository>();
            MockRepository.Setup(m => m.GetUser(It.Is<string>(i => i == ExistingUser.NameIdentifierClaim))).Returns(ExistingUser);
            MockRepository.Setup(m => m.GetUser(It.Is<string>(i => i == DeactivatedUser.NameIdentifierClaim))).Returns(DeactivatedUser);
            Logic = new UserLogic(MockRepository.Object);
        }

        public void Dispose()
        {
            MockRepository.Reset();
        }

        private User GetDeactivatedUser()
        {
            return new User()
            {
                Id = 2,
                NameIdentifierClaim = "deactivatedIdentifier",
                Username = "deactivatedUsername",
                Active = false,
                AccountCreated = new DateTime(2024, 1, 1),
                LastLogin = new DateTime(2024, 2, 1),
            };
        }

        private User GetExistingUser()
        {
            return new User()
            {
                Id = 1,
                NameIdentifierClaim = "testIdentifier",
                Username = "testUsername",
                Active = true,
                AccountCreated = new DateTime(2024,1,1),
                LastLogin = new DateTime(2024,2,1)
            };
        }

        private User GetNewUser()
        {
            return new User()
            {
                NameIdentifierClaim = "newIdentifier",
                Username = "newUsername"
            };
        }

        [Fact]
        public void GetUser_ReturnsUser_GivenExistingUser()
        {
            var returnedUser = Logic.GetUser(ExistingUser.NameIdentifierClaim, ExistingUser.Username);

            Assert.Equivalent(returnedUser, ExistingUser);
        }

        [Fact]
        public void GetUser_AddsUser_GivenNonexistentUser()
        {
            Logic.GetUser(NewUser.NameIdentifierClaim, NewUser.Username);

            MockRepository.Verify(m => m.AddUser(It.Is<User>(u => u.NameIdentifierClaim == NewUser.NameIdentifierClaim)), Times.Once());
        }

        [Fact]
        public void GetCurrentUser_ThrowsCustomException_GivenDeactivatedUser()
        {
            Assert.Throws<DeactivatedUserException>(() => Logic.GetUser(DeactivatedUser.NameIdentifierClaim, DeactivatedUser.Username));
        }

        [Fact]
        public void UpdateUsername_UpdatesUser()
        {
            string  newUsername = "updatedUsername";
            
            Logic.UpdateUsername(ExistingUser, newUsername);

            MockRepository.Verify(m => m.UpdateUser(It.Is<User>(u => u.Username == newUsername)), Times.Once());
        }
    }
}
