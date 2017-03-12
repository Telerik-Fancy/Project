using System.Collections.Generic;

namespace Fancy.Data.Models.Models
{
    public class OrderStatus
    {
        private ICollection<Order> orders;

        public OrderStatus()
        {
            this.orders = new HashSet<Order>();
        }

        public int Id { get; set; }

        public string Name { get; set; }

        public virtual ICollection<Order> Orders
        {
            get
            {
                return this.orders;
            }

            set
            {
                this.orders = value;
            }
        }
    }
}
