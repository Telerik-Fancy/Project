using Fancy.Common.Enums;
using Fancy.Data.Models.Models;
using System.Collections.Generic;

namespace Fancy.Services.Data.Contracts
{
    public interface IItemService
    {
        void AddItem(Item item);

        IEnumerable<Item> GetItemsOfType(int pageNumber, ItemType itemType, MainColourType colour, MainMaterialType material);

        IEnumerable<Item> GetNewestItems(int pageNumber, MainColourType colourType, MainMaterialType materialType);

        IEnumerable<Item> GetItemsInPromotion(int pageNumber, MainColourType colourType, MainMaterialType materialType);

        Item GetItemById(int id);

        int GetItemsOfTypeCount(ItemType itemType);

        int GetAllItemsCount();

        int GetAllItemsInPromotionCount();

        bool CheckUniqueItemCode(string itemCode);
    }
}
