using Fancy.Data.Contexts;
using Fancy.Data.Models.Models;
using Fancy.Data.Repositories;
using Fancy.Common.Validator;

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
            Validator.ValidateNullArgument(context, "context");
            Validator.ValidateNullArgument(items, "items");
            Validator.ValidateNullArgument(orders, "orders");
            Validator.ValidateNullArgument(users, "users");

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
