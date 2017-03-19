﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fancy.Common.Constants
{
    public class ServerConstants
    {
        public const int ItemsOnPage = 8;

        public const string GalleryNecklacesHeader = "Necklaces";
        public const string GalleryEaringsHeader = "Earings";
        public const string GalleryBraceletsHeader = "Bracelets";
        public const string GallerySetsHeader = "Sets";
        public const string GalleryNewItemsHeader = "New items";
        public const string GalleryPromotionItemsHeader = "Items in promotion";

        public const int EnumStartValue = 1;
        public const int EnumEndValue = 10000;

        public const int PromotionMinValue = 5;
        public const int PromotionMaxValue = 90;

        public const int ItemCodeMinLength = 3;
        public const int ItemCodeMaxLength = 40;
    }
}