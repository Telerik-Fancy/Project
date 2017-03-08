using System;
using System.Data.Entity;
using Fancy.Data.Models;
using Fancy.Data.Contexts.Interfaces;
using Microsoft.AspNet.Identity.EntityFramework;
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
    }
}
