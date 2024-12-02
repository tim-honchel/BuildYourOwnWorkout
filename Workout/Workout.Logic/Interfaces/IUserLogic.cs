using Microsoft.AspNetCore.Http;
using Workouts.Entities.Database;

namespace Workouts.Logic.Interfaces
{
    public interface IUserLogic
    {
        User GetCurrentUser(HttpContext context);
        void UpdateUsername(User user, string newUsername);
    }
}
