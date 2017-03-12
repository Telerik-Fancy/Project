using System.Web;
using Fancy.Common.Enums;

namespace Fancy.Web.Areas.Admin.Models
{
    public class AddItemViewModel
    {
        public ItemTypeEnum ItemType { get; set; }

        public MainColourEnum MainColour { get; set; }

        public MainMaterialEnum MainMaterial { get; set; }

        public HttpPostedFileBase Image { get; set; }

        public decimal Price { get; set; }

        public int Quantity { get; set; }
    }
}