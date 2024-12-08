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

        public User GetUser(string identifier, string username)
        {
            var user = Repository.GetUser(identifier);

            if (user == null) 
            {
                AddUser(username, identifier);
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

        private void AddUser(string username, string identifier)
        {
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
