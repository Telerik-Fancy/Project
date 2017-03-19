using Fancy.Common.Enums;
using System;
using System.Collections.Generic;

namespace Fancy.Data.Models.Models
{
    public class Item
    {
        private ICollection<Order> orders;

        public Item()
        {
            this.orders = new HashSet<Order>();
        }

        public int Id { get; set; }

        public string ItemCode { get; set; }

        public ItemType ItemType { get; set; }

        public MainColourType MainColour { get; set; }

        public MainMaterialType MainMaterial { get; set; }

        public string ImageBase64String { get; set; }

        public virtual ICollection<Order> Orders
        {
            get { return this.orders; }

            set { this.orders = value; }
        }

        public decimal Price { get; set; }

        public decimal Discount { get; set; }

        public int Quantity { get; set; }

        public DateTime DateAdded { get; set; }

        public bool IsDeleted { get; set; }
    }
}
