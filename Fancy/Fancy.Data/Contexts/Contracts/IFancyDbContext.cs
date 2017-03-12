using Fancy.Data.Models.Models;
using System.Data.Entity;

namespace Fancy.Data.Contexts.Contracts
{
    public interface IFancyDbContext : IDbContext
    {
        IDbSet<User> Users { get; }

        IDbSet<Item> Items { get; }

        IDbSet<ItemType> ItemTypes { get; }

        IDbSet<MainColour> MainColours { get; }

        IDbSet<MainMaterial> MainMaterial { get; }

        IDbSet<Order> Orders { get; }

        IDbSet<OrderStatus> OrderStatuses { get; }
    }
}
