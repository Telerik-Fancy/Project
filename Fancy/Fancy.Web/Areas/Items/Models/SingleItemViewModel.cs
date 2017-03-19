using Fancy.Common.Constants;
using Fancy.Common.Enums;
using Fancy.Common.Messages;
using System.ComponentModel.DataAnnotations;
using System.Drawing;
using System.Web;

namespace Fancy.Web.Areas.Items.Models
{
    public class SingleItemViewModel
    {
        public int Id { get; set; }

        public string ItemCode { get; set; }

        public MainColourType MainColour { get; set; }

        public MainMaterialType MainMaterial { get; set; }

        public decimal Price { get; set; }

        [Required]
        [Range(ServerConstants.PromotionMinValue, ServerConstants.PromotionMaxValue, ErrorMessage = Messages.RequiredDiscount)]
        public decimal Discount { get; set; }

        public int Quantity { get; set; }

        public string ImageBase64String { get; set; }
    }
}