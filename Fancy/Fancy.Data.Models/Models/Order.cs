using Fancy.Common.Enums;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Fancy.Data.Models.Models
{
    public class Order
    {
        private ICollection<Item> items;

        public Order()
        {
            this.items = new HashSet<Item>();
        }

        [Key]
        public int Id { get; set; }

        public OrderStatusType OrderStatus { get; set; }

        public string UserId { get; set; }

        public virtual User User { get; set; }

        public virtual ICollection<Item> Items
        {
            get { return this.items; }

            set { this.items = value; }
        }

        public decimal TotalPrice { get; set; }
    }
}
