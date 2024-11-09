using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Workouts.Data.Interfaces;
using Workouts.Data.Repositories;


namespace Workouts.Data
{
    public static class DataConfiguration
    {
        public static IServiceCollection AddScope(this IServiceCollection services, string dbConnectionString)
        {
            services.AddDbContextFactory<Context>(options => options.UseSqlServer(dbConnectionString));
            services.AddTransient<IUserRepository, UserRepository>();
            services.AddTransient<IWorkoutRepository, WorkoutRepository>();
            return services;
        }
    }
}
