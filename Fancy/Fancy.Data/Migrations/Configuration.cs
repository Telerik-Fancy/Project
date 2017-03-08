using Fancy.Data.Contexts;
using System.Data.Entity.Migrations;

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
            //if (context.Roles.Count() == 0)
            //{
            //    var roleStore = new RoleStore<IdentityRole>(context);
            //    var roleManager = new RoleManager<IdentityRole>(roleStore);
            //    var normalRole = new IdentityRole { Name = UserRole.Normal };
            //    var moderatorRole = new IdentityRole { Name = UserRole.Moderator };
            //    var adminRole = new IdentityRole { Name = UserRole.Administrator };

            //    roleManager.Create(normalRole);
            //    roleManager.Create(moderatorRole);
            //    roleManager.Create(adminRole);
            //}
        }

        private void SeedUsers(FancyDbContext context)
        {
            //var userStore = new UserStore<User>(context);
            //var userManager = new UserManager<User>(userStore);

            //if (!context.Users.Any(u => u.UserName == "admin"))
            //{
            //    var adminUser = new User { UserName = "admin", Email = "admin@admin.com" };

            //    userManager.Create(adminUser, WebConfigurationManager.AppSettings["AdminPassword"]);
            //    userManager.AddToRole(adminUser.Id, UserRole.Administrator);
            //}
        }
    }
}
