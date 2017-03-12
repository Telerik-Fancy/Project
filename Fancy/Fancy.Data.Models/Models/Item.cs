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

        public int ItemTypeId { get; set; }

        public virtual ItemType ItemType { get; set; }

        public int MainMaterialId { get; set; }

        public virtual MainMaterial MainMaterial { get; set; }

        public int MainColourId { get; set; }

        public virtual MainColour MainColour { get; set; }

        public byte[] ImageBytes { get; set; }

        public virtual ICollection<Order> Orders { get; set; }

        public decimal Price { get; set; }

        public decimal Discount { get; set; }

        public int Quantity { get; set; }

        public DateTime DateAdded { get; set; }

        public bool IsDeleted { get; set; }
    }
}
