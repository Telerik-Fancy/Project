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
            throw new NotImplementedException();
        }

        public IEnumerable<Item> GetItemsInPromotion(int pageNumber)
        {
            return this.data.Items.All.Where(i => i.Discount != 0);
        }

        public void RemovePromotion(int itemId)
        {
            throw new NotImplementedException();
        }
    }
}
