using Fancy.Common.Enums;
using Fancy.Data.Models.Models;
using System.Collections.Generic;

namespace Fancy.Services.Data.Contracts
{
    public interface IItemService
    {
        void AddItem(Item item);

        IEnumerable<Item> GetItemsOfType(ItemType itemType, int pageNumber);

        IEnumerable<Item> GetNewestItems(int pageNumber);

        IEnumerable<Item> GetItemsInPromotion(int pageNumber);

        Item GetItemById(int id);

        int GetItemsOfTypeCount(ItemType itemType);

        int GetAllItemsCount();
    }
}
