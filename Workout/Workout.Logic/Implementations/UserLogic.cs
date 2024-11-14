using Microsoft.AspNetCore.Http;
using Workouts.Data.Interfaces;
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
            throw new NotImplementedException();
        }

        public void UpdateUser(User user)
        {
            throw new NotImplementedException();
        }
    }
}
