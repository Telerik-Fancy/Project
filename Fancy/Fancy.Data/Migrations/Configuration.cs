using Fancy.Data.Contexts;
using Fancy.Data.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Data.Entity.Migrations;
using System.Linq;

namespace Fancy.Data.Migrations
{
    public class Configuration : DbMigrationsConfiguration<FancyDbContext>
    {
        public Configuration()
        {
            this.AutomaticMigrationsEnabled = true;
            this.AutomaticMigrationDataLossAllowed = true;
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
                var regularRole = new IdentityRole { Name = "Regular" };
                var administratorRole = new IdentityRole { Name = "Administrator" };

                roleManager.Create(regularRole);
                roleManager.Create(administratorRole);
            }
        }

        private void SeedUsers(FancyDbContext context)
        {
            var userStore = new UserStore<User>(context);
            var userManager = new UserManager<User>(userStore);

            if (!context.Users.Any(u => u.UserName == "admin@gmail.com"))
            {
                var adminUser = new User { UserName = "admin@gmail.com", Email = "admin@gmail.com" };

                userManager.Create(adminUser, "123456");
                userManager.AddToRole(adminUser.Id, "Administrator");
            }
        }
    }
}
