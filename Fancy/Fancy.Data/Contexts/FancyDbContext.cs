using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using Microsoft.AspNet.Identity.EntityFramework;
using Fancy.Data.Models.Models;
using Fancy.Data.Migrations;

namespace Fancy.Data.Contexts
{
    public class FancyDbContext : IdentityDbContext<User>, IFancyDbContext
    {
        public FancyDbContext()
            :base("Fancy", throwIfV1Schema: false)
        {
            
            Database.SetInitializer(new MigrateDatabaseToLatestVersion<FancyDbContext, Configuration>());
        }

        public static FancyDbContext Create()
        {
            var context = new FancyDbContext();

            return context;
        }

        public new IDbSet<T> Set<T>() where T : class
        {
            return base.Set<T>();
        }

        public virtual IDbSet<Item> Items { get; set; }

        public virtual IDbSet<Order> Orders { get; set; }

        public new int SaveChanges()
        {
            return base.SaveChanges();
        }

        public new void Dispose()
        {
            base.Dispose();
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<OneToManyCascadeDeleteConvention>();
            modelBuilder.Conventions.Remove<ManyToManyCascadeDeleteConvention>();

            base.OnModelCreating(modelBuilder);
        }
    }
}
