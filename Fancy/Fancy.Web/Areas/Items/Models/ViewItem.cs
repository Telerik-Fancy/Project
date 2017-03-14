using Fancy.Common.Enums;
using System.Drawing;
using System.Web;

namespace Fancy.Web.Areas.Items.Models
{
    public class ViewItem
    {
        public int Id { get; set; }

        public string ItemCode { get; set; }

        public MainColour MainColour { get; set; }

        public MainMaterial MainMaterial { get; set; }

        public byte[] ImageBytes { get; set; }

        public decimal Price { get; set; }

        public decimal Discount { get; set; }

        public int Quantity { get; set; }

        public string ImageBase64String { get; set; }
    }
}