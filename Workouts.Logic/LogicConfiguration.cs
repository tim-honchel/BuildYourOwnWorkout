using Microsoft.Extensions.DependencyInjection;
using Workouts.Logic.Implementations;
using Workouts.Logic.Interfaces;

namespace Workouts.Logic
{
    public static class LogicConfiguration
    {
        public static IServiceCollection AddScope(this IServiceCollection services)
        {
            services.AddScoped<IUserLogic, UserLogic>();
            services.AddScoped<IWorkoutLogic, WorkoutLogic>();
            return services;
        }
    }
}
