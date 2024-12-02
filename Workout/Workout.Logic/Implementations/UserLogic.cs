using Microsoft.AspNetCore.Http;
using Workouts.Data.Interfaces;
using Workouts.Entities.CustomExceptions;
using Workouts.Entities.Database;
using Workouts.Logic.Interfaces;

namespace Workouts.Logic.Implementations
{
    public class UserLogic : IUserLogic
    {
        private IUserRepository Repository { get; set; }

        public UserLogic(IUserRepository repository) 
        {
            Repository = repository;
        }

        public User GetCurrentUser(HttpContext context)
        {
            var identifier = context.User.Claims
                .Where(c => c.Type.Equals(System.Security.Claims.ClaimTypes.NameIdentifier))
                .Select(c => c.Value)
                .FirstOrDefault();
            var user = Repository.GetUser(identifier);

            if (user == null) 
            {
                AddUser(context, identifier);
                user = Repository.GetUser(identifier);
            }
            else if (user.Active == false)
            {
                throw new DeactivatedUserException();
            }
            else if (user.LastLogin < DateTime.Today)
            {
                user.LastLogin = DateTime.Today;
                Repository.UpdateUser(user);
            }

            return user;
        }

        public void UpdateUsername(User user, string newUsername)
        {
            user.Username = newUsername;
            Repository.UpdateUser(user);
        }

        private void AddUser(HttpContext context, string identifier)
        {
            var username = context.User.Identity != null ? context.User.Identity.Name : string.Empty;
            var newUser = new User()
            {
                NameIdentifierClaim = identifier,
                Active = true,
                Username = username,
                AccountCreated = DateTime.Today,
                LastLogin = DateTime.Today,
            };
            Repository.AddUser(newUser);
        }
    }
}
