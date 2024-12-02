using Microsoft.AspNetCore.Http;
using Moq;
using System.Security.Claims;
using System.Security.Principal;
using Workouts.Data.Interfaces;
using Workouts.Entities.Database;
using Workouts.Logic.Implementations;

namespace Workouts.UnitTests.Logic
{
    public class UserLogicTests : IDisposable
    {
        private User ExistingUser { get; set; }
        private User NewUser { get; set; }
        private Mock<HttpContext> MockContextForExistingUser { get; set; }
        private Mock<HttpContext> MockContextForNewUser { get; set; }
        private Mock<IUserRepository> MockRepository { get; set; }
        private UserLogic Logic { get; set; }

        public UserLogicTests() 
        { 
            ExistingUser = GetExistingUser();
            NewUser = GetNewUser();

            var existingUserClaims = new List<Claim>() { new Claim(ClaimTypes.NameIdentifier, ExistingUser.NameIdentifierClaim) };
            var existingUserIdentity = new ClaimsIdentity(existingUserClaims);
            var existingUserPrincipal = new ClaimsPrincipal( new List<ClaimsIdentity>() { existingUserIdentity});

            MockContextForExistingUser = new Mock<HttpContext>();
            MockContextForExistingUser.Setup(m => m.User).Returns(existingUserPrincipal);

            var newUserClaims = new List<Claim>() { new Claim(ClaimTypes.NameIdentifier, NewUser.NameIdentifierClaim) };
            var newUserIdentity = new ClaimsIdentity(newUserClaims);
            var newUserPrincipal = new ClaimsPrincipal(new List<ClaimsIdentity>() { newUserIdentity });

            MockContextForNewUser = new Mock<HttpContext>();
            MockContextForNewUser.Setup(m => m.User).Returns(newUserPrincipal);

            MockRepository = new Mock<IUserRepository>();
            MockRepository.Setup(m => m.GetUser(It.Is<string>(i => i == ExistingUser.NameIdentifierClaim))).Returns(ExistingUser);
            Logic = new UserLogic(MockRepository.Object);
        }

        public void Dispose()
        {
            MockContextForExistingUser.Reset();
            MockContextForNewUser.Reset();
            MockRepository.Reset();
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
        public void GetCurrentUser_ReturnsUser_GivenExistingUser()
        {
            var identifier = ExistingUser.NameIdentifierClaim;

            var returnedUser = Logic.GetCurrentUser(MockContextForExistingUser.Object);

            Assert.Equivalent(returnedUser, ExistingUser);
        }

        [Fact]
        public void GetCurrentUser_AddsUser_GivenNonexistentUser()
        {
            Logic.GetCurrentUser(MockContextForNewUser.Object);

            MockRepository.Verify(m => m.AddUser(It.Is<User>(u => u.NameIdentifierClaim == NewUser.NameIdentifierClaim)), Times.Once());
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
