using BLL.Services;
using Core.Entities;
using DAL.Contexts;
using DAL.Repositories.Abstractions;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace UsersAPI
{
    public class SeedManager
    {
        public static async Task Seed(IServiceProvider services)
        {
            await SeedAdminUser(services);
        }

        private static async Task SeedAdminUser(IServiceProvider services)
        {
            var repos = services.GetRequiredService<IUserRepository>();
            var userService = services.GetRequiredService<UserService>();

            var adminUser = await repos.GetByLoginAsync("admin");

            if (adminUser is null)
            {
                adminUser = new User
                {
                    Login = "admin",
                    Password = "admin",
                    Name = "MainAdmin",
                    Gender = 1,
                    Admin = true
                };
                await userService.Create(adminUser, "SeedManager");
            }
        }
    }
}
