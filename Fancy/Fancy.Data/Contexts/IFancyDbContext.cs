using Fancy.Data.Models.Models;
using System.Data.Entity;

namespace Fancy.Data.Contexts
{
    public interface IFancyDbContext : IDbContext
    {
        IDbSet<User> Users { get; }

        IDbSet<Item> Items { get; }

        IDbSet<Order> Orders { get; }
    }
}
