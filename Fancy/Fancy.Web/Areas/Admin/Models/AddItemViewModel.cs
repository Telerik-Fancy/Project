using System.Web;
using Fancy.Common.Enums;
using System;
using System.ComponentModel.DataAnnotations;
using Fancy.Common.Messages;
using Fancy.Common.Constants;

namespace Fancy.Web.Areas.Admin.Models
{
    public class AddItemViewModel
    {
        [Required(ErrorMessage = Messages.ItemCodeRequired)]
        [StringLength(ServerConstants.ItemCodeMaxLength, ErrorMessage = Messages.ItemCodeInvalidLength, MinimumLength = ServerConstants.ItemCodeMinLength)]
        public string ItemCode { get; set; }

        [Required]
        [Range(ServerConstants.EnumStartValue, ServerConstants.EnumEndValue, ErrorMessage = Messages.ItemTypeRequired)]
        public ItemType ItemType { get; set; }

        [Required]
        [Range(ServerConstants.EnumStartValue, ServerConstants.EnumEndValue, ErrorMessage = Messages.MainColourTypeRequired)]
        public MainColourType MainColour { get; set; }

        [Required]
        [Range(ServerConstants.EnumStartValue, ServerConstants.EnumEndValue, ErrorMessage = Messages.MainMaterialTypeRequired)]
        public MainMaterialType MainMaterial { get; set; }

        [Required(ErrorMessage = Messages.ImageRequired)]
        public HttpPostedFileBase Image { get; set; }

        [Required(ErrorMessage = Messages.PriceRequired)]
        public decimal Price { get; set; }

        public decimal Discount { get; set; }

        [Required(ErrorMessage = Messages.QuantityRequired)]
        public int Quantity { get; set; }

        public DateTime DateAdded { get; set; }

        public bool IsDeleted { get; set; }

        public string ImageBase64String { get; set; }
    }
}