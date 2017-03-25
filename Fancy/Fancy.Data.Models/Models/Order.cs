using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Fancy.Common.Enums;

namespace Fancy.Data.Models.Models
{
    public class Order
    {
        private ICollection<Item> items;

        public Order()
        {
            this.items = new List<Item>();
        }

        [Key]
        public int Id { get; set; }

        public OrderStatusType OrderStatus { get; set; }

        [ForeignKey("User")]
        public string UserId { get; set; }

        public virtual User User { get; set; }

        public virtual ICollection<Item> Items
        {
            get { return this.items; }

            set { this.items = value; }
        }

        public decimal TotalPrice { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime PurchaseDate { get; set; }
    }
}
