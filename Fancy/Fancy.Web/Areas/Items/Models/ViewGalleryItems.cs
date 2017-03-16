using Fancy.Common.Enums;
using System.Collections.Generic;

namespace Fancy.Web.Areas.Items.Models
{
    public class ViewGalleryItems
    {
        public string GalleryTitle { get; set; }

        public int PageButtonsCount { get; set; }

        public IEnumerable<ViewItem> ItemsList { get; set; }

        public string ItemType { get; set; }

        public MainColour Colour { get; set; }

        public MainMaterial Material { get; set; }

        public PriceFilterType PriceFilter { get; set; }

        public string SelectedColour { get; set; }
    }
}