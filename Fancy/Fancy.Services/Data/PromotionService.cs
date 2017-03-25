using Fancy.Data.Data;
using Fancy.Services.Data.Contracts;
using Fancy.Common.Validator;
using Fancy.Common.Constants;

namespace Fancy.Services.Data
{
    public class PromotionService : IPromotionService
    {
        private IEfFancyData data;

        public PromotionService(IEfFancyData data)
        {
            Validator.ValidateNullArgument(data, "data");

            this.data = data;
        }

        public void AddPromotion(int itemId, decimal discount)
        {
            Validator.ValidateRange(itemId, ServerConstants.IdMinValue, ServerConstants.IdMaxValue, "itemId");
            Validator.ValidateRange(discount, ServerConstants.DiscountMinValue, ServerConstants.DiscountMaxValue, "discount");

            var item = this.data.Items.GetById(itemId);

            Validator.ValidateNullDatabaseObject(item, "Item");

            item.Discount = discount;

            this.data.Commit();
        }

        public void RemovePromotion(int itemId)
        {
            Validator.ValidateRange(itemId, ServerConstants.IdMinValue, ServerConstants.IdMaxValue, "itemId");

            var item = this.data.Items.GetById(itemId);

            Validator.ValidateNullDatabaseObject(item, "Item");

            item.Discount = 0;

            this.data.Commit();
        }
    }
}
