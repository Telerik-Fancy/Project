using Fancy.Data.Contexts;
using System;
using Fancy.Data.Models.Models;
using Fancy.Data.Repositories;
using Bytes2you.Validation;

namespace Fancy.Data.Data
{
    public class EfFancyData : IEfFancyData
    {
        private IFancyDbContext context;
        private IEfGenericRepository<Item> items;
        private IEfGenericRepository<Order> orders;
        private IEfGenericRepository<User> users;

        public EfFancyData(IFancyDbContext context, IEfGenericRepository<Item> items, IEfGenericRepository<Order> orders, IEfGenericRepository<User> users)
        {
            Guard.WhenArgument(context, nameof(context)).IsNull().Throw();
            Guard.WhenArgument(items, nameof(items)).IsNull().Throw();
            Guard.WhenArgument(orders, nameof(orders)).IsNull().Throw();
            Guard.WhenArgument(users, nameof(users)).IsNull().Throw();

            this.context = context;
            this.items = items;
            this.orders = orders;
            this.users = users;
        }

        public IEfGenericRepository<Item> Items
        {
            get
            {
                return this.items;
            }
        }

        public IEfGenericRepository<Order> Orders
        {
            get
            {
                return this.orders;
            }
        }

        public IEfGenericRepository<User> Users
        {
            get
            {
                return this.users;
            }
        }

        public void Commit()
        {
            this.context.SaveChanges();
        }

        public void Dispose()
        {
            this.context.Dispose();
        }
    }
}
