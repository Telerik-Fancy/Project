using System.Collections.Generic;
using Fancy.Common.Enums;
using Fancy.Data.Models.Models;

namespace Fancy.Services.Data.Contracts
{
    public interface IItemService
    {
        void AddItem(Item item);

        IEnumerable<Item> GetItemsOfType(int pageNumber, ItemType itemType, MainColourType colour, MainMaterialType material);

        IEnumerable<Item> GetNewestItems(int pageNumber, MainColourType colourType, MainMaterialType materialType);

        IEnumerable<Item> GetItemsInPromotion(int pageNumber, MainColourType colourType, MainMaterialType materialType);

        Item GetItemById(int id);

        int GetItemsOfTypeCount(ItemType itemType, MainColourType colour, MainMaterialType material);

        int GetAllItemsCount(MainColourType colour, MainMaterialType material);

        int GetAllItemsInPromotionCount(MainColourType colour, MainMaterialType material);

        bool CheckUniqueItemCode(string itemCode);
    }
}
