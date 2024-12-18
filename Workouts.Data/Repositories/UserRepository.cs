﻿using Microsoft.EntityFrameworkCore;
using Workouts.Data.Interfaces;
using Workouts.Entities.Database;

namespace Workouts.Data.Repositories
{
    public class UserRepository : IUserRepository
    {
        private IDbContextFactory<Context> _contextFactory;

        public UserRepository(IDbContextFactory<Context> contextFactory)
        {
            _contextFactory = contextFactory;
        }

        public void AddUser(User user)
        {
            using var context = _contextFactory.CreateDbContext();
            context.User.Add(user);
            context.SaveChanges();
            context.Dispose();
        }

        public User GetUser(string nameIdentifierClaim)
        {
            using var context = _contextFactory.CreateDbContext();
            var user = context.User
                .FirstOrDefault(u => u.NameIdentifierClaim == nameIdentifierClaim);
            context.Dispose();
            return user;
        }

        public void UpdateUser(User user)
        {
            using var context = _contextFactory.CreateDbContext();
            context.User .Update(user);
            context.SaveChanges();
            context.Dispose();
        }
    }
}
