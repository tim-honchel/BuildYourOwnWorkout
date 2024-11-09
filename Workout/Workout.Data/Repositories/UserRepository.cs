using Microsoft.EntityFrameworkCore;
using Workouts.Data.Interfaces;
using Workouts.Entities.Database;

namespace Workouts.Data.Repositories
{
    public class UserRepository : IUserRepository
    {
        public Context Context { get; set; }

        public UserRepository(Context context)
        {
            Context = context;
        }

        public void AddUser(User user)
        {
            throw new NotImplementedException();
        }

        public User GetUser(string nameIdentifierClaim)
        {
            throw new NotImplementedException();
        }

        public void UpdateUser(User user)
        {
            throw new NotImplementedException();
        }
    }
}
