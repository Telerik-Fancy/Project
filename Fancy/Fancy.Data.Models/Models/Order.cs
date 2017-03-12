using System.Collections.Generic;

namespace Fancy.Data.Models.Models
{
    public class Order
    {
        private ICollection<Item> items;
        private ICollection<User> users;

        public Order()
        {
            this.items = new HashSet<Item>();
            this.users = new HashSet<User>();
        }

        public int Id { get; set; }

        public virtual OrderStatus OrderStatus { get; set; }

        public virtual ICollection<Item> Items
        {
            get
            {
                return this.items;
            }

            set
            {
                this.items = value;
            }
        }

        public virtual ICollection<User> Users
        {
            get
            {
                return this.users;
            }

            set
            {
                this.users = value;
            }
        }

        public bool IsDeleted { get; set; }
    }
}
