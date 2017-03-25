using System.Linq;
using System.Data.Entity.Migrations;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Fancy.Common.Constants;
using Fancy.Data.Contexts;
using Fancy.Data.Models.Models;

namespace Fancy.Data.Migrations
{
    public class Configuration : DbMigrationsConfiguration<FancyDbContext>
    {
        public Configuration()
        {
            this.AutomaticMigrationsEnabled = true;
            this.AutomaticMigrationDataLossAllowed = false;
        }

        protected override void Seed(FancyDbContext context)
        {
            this.SeedRoles(context);
            this.SeedUsers(context);
        }

        private void SeedRoles(FancyDbContext context)
        {
            if (context.Roles.Count() == 0)
            {
                var roleStore = new RoleStore<IdentityRole>(context);
                var roleManager = new RoleManager<IdentityRole>(roleStore);
                var regularRole = new IdentityRole { Name = UserConstants.RegularRole };
                var administratorRole = new IdentityRole { Name = UserConstants.AdministratorRole };

                roleManager.Create(regularRole);
                roleManager.Create(administratorRole);
            }
        }

        private void SeedUsers(FancyDbContext context)
        {
            var userStore = new UserStore<User>(context);
            var userManager = new UserManager<User>(userStore);

            if (!context.Users.Any(u => u.UserName == UserConstants.AdminUsername))
            {
                var adminUser = new User { UserName = UserConstants.AdminUsername, Email = UserConstants.AdminEmail };

                userManager.Create(adminUser, UserConstants.AdminPassword);
                userManager.AddToRole(adminUser.Id, UserConstants.AdministratorRole);
            }
        }
    }
}
