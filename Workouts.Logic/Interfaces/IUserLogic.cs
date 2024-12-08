using Microsoft.AspNetCore.Http;
using Workouts.Entities.Database;

namespace Workouts.Logic.Interfaces
{
    public interface IUserLogic
    {
        User GetUser(string identifier, string username);
        void UpdateUsername(User user, string newUsername);
    }
}
