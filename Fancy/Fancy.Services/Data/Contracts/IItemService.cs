using Fancy.Common.Enums;
using Fancy.Data.Models.Models;
using System.Collections.Generic;

namespace Fancy.Services.Data.Contracts
{
    public interface IItemService
    {
        void AddItem(Item item);

        IEnumerable<Item> GetItemsOfType(int pageNumber, ItemType itemType, MainColour colour, MainMaterial material, PriceFilterType priceFilter);

        IEnumerable<Item> GetNewestItems(int pageNumber, MainColour colourType, MainMaterial materialType, PriceFilterType priceFilter);

        IEnumerable<Item> GetItemsInPromotion(int pageNumber, MainColour colourType, MainMaterial materialType, PriceFilterType priceFilter);

        Item GetItemById(int id);

        int GetItemsOfTypeCount(ItemType itemType);

        int GetAllItemsCount();

        int GetAllItemsInPromotionCount();
    }
}
