using System.Web;
using Fancy.Common.Enums;
using System;

namespace Fancy.Web.Areas.Admin.Models
{
    public class AddItemViewModel
    {
        public string ItemCode { get; set; }

        public ItemType ItemType { get; set; }

        public MainColour MainColour { get; set; }

        public MainMaterial MainMaterial { get; set; }

        public HttpPostedFileBase Image { get; set; }

        public byte[] ImageBytes { get; set; }

        public decimal Price { get; set; }

        public decimal Discount { get; set; }

        public int Quantity { get; set; }

        public DateTime DateAdded { get; set; }

        public bool IsDeleted { get; set; }
    }
}