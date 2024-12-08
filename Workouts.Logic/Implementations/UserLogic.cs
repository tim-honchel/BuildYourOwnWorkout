using Workouts.Data.Interfaces;
using Workouts.Entities.CustomExceptions;
using Workouts.Entities.Database;
using Workouts.Logic.Interfaces;

namespace Workouts.Logic.Implementations
{
    public class UserLogic : IUserLogic
    {
        private IUserRepository _repository;

        public UserLogic(IUserRepository repository) 
        {
            _repository = repository;
        }

        public User GetUser(string identifier, string username)
        {
            var user = _repository.GetUser(identifier);

            if (user == null) 
            {
                AddUser(username, identifier);
                user = _repository.GetUser(identifier);
            }
            else if (user.Active == false)
            {
                throw new DeactivatedUserException();
            }
            else if (user.LastLogin < DateTime.Today)
            {
                user.LastLogin = DateTime.Today;
                _repository.UpdateUser(user);
            }

            return user;
        }

        public void UpdateUsername(User user, string newUsername)
        {
            user.Username = newUsername;
            _repository.UpdateUser(user);
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
            _repository.AddUser(newUser);
        }
    }
}
