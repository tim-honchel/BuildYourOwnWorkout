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
            Context.User.Add(user);
            Context.SaveChanges();
        }

        public User GetUser(string nameIdentifierClaim)
        {
            return Context.User
                .FirstOrDefault(u => u.NameIdentifierClaim == nameIdentifierClaim);
        }

        public void UpdateUser(User user)
        {
            Context.User .Update(user);
            Context.SaveChanges();
        }
    }
}
