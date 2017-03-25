using System.Data.Entity;
using Fancy.Data.Models.Models;

namespace Fancy.Data.Contexts
{
    public interface IFancyDbContext : IDbContext
    {
        IDbSet<User> Users { get; }

        IDbSet<Item> Items { get; }

        IDbSet<Order> Orders { get; }
    }
}
