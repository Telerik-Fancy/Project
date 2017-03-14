using Fancy.Data.Models.Models;
using Fancy.Data.Repositories;
using System;

namespace Fancy.Data.Data
{
    public interface IEfFancyData : IDisposable
    {
        IEfGenericRepository<Item> Items { get; }

        IEfGenericRepository<User> Users { get; }

        IEfGenericRepository<Order> Orders { get; }

        void Commit();
    }
}
