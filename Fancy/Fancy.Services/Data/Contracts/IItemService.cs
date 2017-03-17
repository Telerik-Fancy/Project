using Fancy.Common.Enums;
using Fancy.Data.Models.Models;
using System.Collections.Generic;

namespace Fancy.Services.Data.Contracts
{
    public interface IItemService
    {
        void AddItem(Item item);

        IEnumerable<Item> GetItemsOfType(int pageNumber, ItemType itemType, MainColour colour, MainMaterial material);

        IEnumerable<Item> GetNewestItems(int pageNumber, MainColour colourType, MainMaterial materialType);

        IEnumerable<Item> GetItemsInPromotion(int pageNumber, MainColour colourType, MainMaterial materialType);

        Item GetItemById(int id);

        int GetItemsOfTypeCount(ItemType itemType);

        int GetAllItemsCount();

        int GetAllItemsInPromotionCount();
    }
}
