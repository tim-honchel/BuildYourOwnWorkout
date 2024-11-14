using Microsoft.AspNetCore.Http;
using Workouts.Entities.Database;

namespace Workouts.Logic.Interfaces
{
    public interface IUserLogic
    {
        User GetCurrentUser(HttpContext context);
        void UpdateUser(User user);
    }
}
