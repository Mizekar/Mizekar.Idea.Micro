using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Mizekar.Micro.Idea.Data.Entities.Functional;
using Mizekar.Micro.Idea.Resources;

namespace Mizekar.Micro.Idea.Data
{
    /// <summary>
    /// 
    /// </summary>
    public static class DataBaseManager
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        public static void Migrate(IdeaDbContext context)
        {
            if (!context.Database.IsInMemory())
            {
                context.Database.Migrate();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public static async Task CreatePermissions(IdeaDbContext context)
        {
            var profiles = context.Profiles.ToList();
            var Permissions = context.Permissions.ToList();
            if (context.Permissions.Any()) return;

            CreatePermission(context, PermissionConstant.ViewAllIdeaContactsInfo);
            CreatePermission(context, PermissionConstant.DeleteAllIdeas);
            CreatePermission(context, PermissionConstant.EditAllComments);
            CreatePermission(context, PermissionConstant.EditAllIdeas);
            await context.SaveChangesAsync();
        }

        private static void CreatePermission(IdeaDbContext context, KeyValuePair<string, string> permissionInfo)
        {
            var permission = new Permission()
            {
                Name = permissionInfo.Key,
                Title = permissionInfo.Value,
            };
            context.Add(permission);
        }

        //public static async Task CreateRoles(IServiceProvider serviceProvider, IConfiguration Configuration)
        //{
        //    //adding customs roles
        //    var RoleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
        //    var UserManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();
        //    string[] roleNames = { "Admin", "Manager", "Member" };
        //    IdentityResult roleResult;

        //    foreach (var roleName in roleNames)
        //    {
        //        // creating the roles and seeding them to the database
        //        var roleExist = await RoleManager.RoleExistsAsync(roleName);
        //        if (!roleExist)
        //        {
        //            roleResult = await RoleManager.CreateAsync(new IdentityRole(roleName));
        //        }
        //    }
        //    // creating a super user who could maintain the web app
        //    var poweruser = new ApplicationUser
        //    {
        //        UserName = Configuration.GetSection("AppSettings")["UserEmail"],
        //        Email = Configuration.GetSection("AppSettings")["UserEmail"]
        //    };

        //    string userPassword = Configuration.GetSection("AppSettings")["UserPassword"];
        //    var user = await UserManager.FindByEmailAsync(Configuration.GetSection("AppSettings")["UserEmail"]);

        //    if (user == null)
        //    {
        //        var createPowerUser = await UserManager.CreateAsync(poweruser, userPassword);
        //        if (createPowerUser.Succeeded)
        //        {
        //            // here we assign the new user the "Admin" role 
        //            await UserManager.AddToRoleAsync(poweruser, "Admin");
        //        }
        //    }
    }
}

