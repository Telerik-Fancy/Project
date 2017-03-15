using Fancy.Data.Models.Models;
using System.Collections.Generic;

namespace Fancy.Services.Data.Contracts
{
    public interface IPromotionService
    {
        IEnumerable<Item> GetItemsInPromotion(int pageNumber);

        void AddPromotion(int itemId, decimal discount);

        void RemovePromotion(int itemId);
    }
}
