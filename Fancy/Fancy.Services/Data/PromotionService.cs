using Fancy.Data.Data;
using Fancy.Services.Data.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using Fancy.Data.Models.Models;

namespace Fancy.Services.Data
{
    public class PromotionService : IPromotionService
    {
        private IEfFancyData data;

        public PromotionService(IEfFancyData data)
        {
            this.data = data;
        }

        public void AddPromotion(int itemId, decimal discount)
        {
            var item = this.data.Items.All.Single(i => i.Id == itemId);

            item.Discount = discount;

            this.data.Commit();
        }

        public void RemovePromotion(int itemId)
        {
            var item = this.data.Items.All.Single(i => i.Id == itemId);

            item.Discount = 0;

            this.data.Commit();
        }
    }
}
