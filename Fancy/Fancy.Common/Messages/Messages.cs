using Fancy.Common.Constants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fancy.Common.Messages
{
    public class Messages
    {
        public const string RepositoryInitializingContextError = "An instance of DbContext is required to use this repository.";
        public const string RequiredInputMessage = "{0} is required!";

        public const string ItemCodeRequired = "Item code is required.";
        public const string ItemCodeInvalidLength = "Item code length must be between 3 and 40.";
        public const string ItemTypeRequired = "Item type is required.";
        public const string MainColourTypeRequired = "Colour is required.";
        public const string MainMaterialTypeRequired = "Material is required.";
        public const string PriceRequired = "Price is required.";
        public const string QuantityRequired = "Quantity is required.";
        public const string ImageRequired = "Image is required.";

        public const string RequiredDiscount = "Discount is required.";

        public const string ArgumentNullMessage = "{0} can not be null.";
    }
}
