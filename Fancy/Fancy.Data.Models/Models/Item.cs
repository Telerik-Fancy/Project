using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Fancy.Common.Constants;
using Fancy.Common.Enums;
using Fancy.Common.Messages;

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

        [Index(IsUnique = true)]
        [Required]
        [MaxLength(ServerConstants.ItemCodeMaxLength, ErrorMessage = Messages.ItemCodeInvalidLength), MinLength(ServerConstants.ItemCodeMinLength, ErrorMessage = Messages.ItemCodeInvalidLength)]
        public string ItemCode { get; set; }

        [Required]
        public ItemType ItemType { get; set; }

        [Required]
        public MainColourType MainColour { get; set; }

        [Required]
        public MainMaterialType MainMaterial { get; set; }

        [Required]
        public string ImageBase64String { get; set; }

        public virtual ICollection<Order> Orders
        {
            get { return this.orders; }

            set { this.orders = value; }
        }

        [Required]
        public decimal Price { get; set; }

        public decimal Discount { get; set; }

        [Required]
        public int Quantity { get; set; }

        public DateTime DateAdded { get; set; }

        public bool IsDeleted { get; set; }
    }
}
