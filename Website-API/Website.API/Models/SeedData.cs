using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;
using Website.Dal;
using Website.Model;

namespace Website.API.Models
{
    public static class SeedData
    {
        public static void Initialize(IServiceProvider serviceProvider)
        {
            var context = serviceProvider.GetRequiredService<CoreDbContext>();
            context.Database.EnsureCreated();

            var userManager = serviceProvider.GetRequiredService<UserManager<Website.Model.User>>();
            var config = serviceProvider.GetRequiredService<IConfiguration>();
            if (!context.Roles.Any())
            {
                var admin = new Role()
                {
                    Name = Roles.Roles.Administrator
                };

                var user = new Role()
                {
                    Name = Roles.Roles.DefaultUser
                };

                context.Roles.Add(admin);
                context.Roles.Add(user);
                context.SaveChanges();
            }

            if(!context.Users.Any())
            {
                var password = config["Admin:Password"];
                var user = new Website.Model.User()
                {
                    UserName = config["Admin:UserName"],
                    FirstName = config["Admin:FirstName"],
                    LastName = config["Admin:LastName"]
                };

                userManager.CreateAsync(user, password);

                var roleId = context.Roles.SingleOrDefault(x => x.Name == Roles.Roles.Administrator).Id;
                var userId = context.Users.SingleOrDefault(x => x.UserName == user.UserName).Id;

                context.UserRoles.Add(new UserRole()
                {
                    RoleId = roleId,
                    UserId = userId
                });

                context.SaveChanges();
            }
        }
    }
}
