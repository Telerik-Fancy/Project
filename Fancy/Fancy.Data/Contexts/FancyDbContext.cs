using System.Data.Entity;
using Fancy.Data.Models.Models;
using Fancy.Data.Contexts.Contracts;
using Microsoft.AspNet.Identity.EntityFramework;
using Fancy.Data.Migrations;
using System.Data.Entity.ModelConfiguration.Conventions;
using System;

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

        public IDbSet<Item> Items { get; set; }

        public IDbSet<ItemType> ItemTypes { get; set; }

        public IDbSet<MainColour> MainColours { get; set; }

        public IDbSet<MainMaterial> MainMaterial { get; set; }

        public IDbSet<Order> Orders { get; set; }

        public IDbSet<OrderStatus> OrderStatuses { get; set; }

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
