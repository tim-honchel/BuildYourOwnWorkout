using Workouts.Entities.Database;

namespace Workouts.Data.Interfaces
{
    public interface IUserRepository
    {
        void AddUser(User user);
        User GetUser(string nameIdentifierClaim);
        void UpdateUser(User user);
    }
}
