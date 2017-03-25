using System;
using System.Collections.Generic;
using Fancy.Data.Models.Models;
using Fancy.Web.Areas.Items.Models;
using Fancy.Common.Enums;

namespace Fancy.Web.Areas.Profile.Models
{
    public class OrderViewModel
    {
        public int Id { get; set; }

        public OrderStatusType OrderStatus { get; set; }

        public string UserId { get; set; }

        public User User { get; set; }

        public ICollection<SingleItemViewModel> Items { get; set; }

        public decimal TotalPrice { get; set; }

        public DateTime PurchaseDate { get; set; }
    }
}