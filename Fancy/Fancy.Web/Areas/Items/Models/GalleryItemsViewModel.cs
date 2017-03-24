using Fancy.Common.Enums;
using System.Collections.Generic;

namespace Fancy.Web.Areas.Items.Models
{
    public class GalleryItemsViewModel
    {
        public string GalleryTitle { get; set; }

        public int PageButtonsCount { get; set; }

        public IEnumerable<SingleItemViewModel> ItemsList { get; set; }

        public string ItemType { get; set; }

        public MainColourType Colour { get; set; }

        public MainMaterialType Material { get; set; }

        public string SelectedColour { get; set; }

        public string SelectedMaterial { get; set; }
    }
}