using Fancy.Services.Data.Contracts;
using Fancy.Data.Models.Models;
using Fancy.Data.Data;
using Fancy.Common.Enums;
using System.Linq;
using System.Collections.Generic;
using System;
using Fancy.Common.Constants;
using Fancy.Common.Validator;

namespace Fancy.Services.Data
{
    public class ItemService : IItemService
    {
        private readonly IEfFancyData data;

        public ItemService(IEfFancyData data)
        {
            Validator.ValidateNullArgument(data, "data");

            this.data = data;   
        }

        public void AddItem(Item item)
        {
            this.data.Items.Add(item);

            this.data.Commit();
        }

        public Item GetItemById(int id)
        {
            var item = this.data.Items.GetById(id);

            return item;
        }

        public IEnumerable<Item> GetItemsOfType(int pageNumber, ItemType itemType, MainColourType colourType, MainMaterialType materialType)
        {
            var itemsList = this.data.Items.All
                .Where(i => i.ItemType == itemType && !i.IsDeleted && i.Quantity != 0);

            if(colourType != 0)
            {
                itemsList.Where(i => i.MainColour == colourType);
            }

            if (materialType != 0)
            {
                itemsList.Where(i => i.MainMaterial == materialType);
            }

            return itemsList
                    .OrderBy(i => i.Id)
                    .Skip((pageNumber - 1) * ServerConstants.ItemsOnPage)
                    .Take(ServerConstants.ItemsOnPage)
                    .ToList();
        }

        public IEnumerable<Item> GetItemsInPromotion(int pageNumber, MainColourType colourType, MainMaterialType materialType)
        {
            var itemsList = this.data.Items.All
                .Where(i => i.Discount != 0 && !i.IsDeleted && i.Quantity != 0);

            if (colourType != 0)
            {
                itemsList.Where(i => i.MainColour == colourType);
            }

            if (materialType != 0)
            {
                itemsList.Where(i => i.MainMaterial == materialType);
            }

            return itemsList
                    .OrderBy(i => i.Id)
                    .Skip((pageNumber - 1) * ServerConstants.ItemsOnPage)
                    .Take(ServerConstants.ItemsOnPage)
                    .ToList();
        }

        public IEnumerable<Item> GetNewestItems(int pageNumber, MainColourType colourType, MainMaterialType materialType)
        {
            var itemsList = this.data.Items.All
                .Where(i => !i.IsDeleted && i.Quantity != 0)
                .OrderByDescending(i => i.DateAdded);

            if (colourType != 0)
            {
                itemsList.Where(i => i.MainColour == colourType);
            }

            if (materialType != 0)
            {
                itemsList.Where(i => i.MainMaterial == materialType);
            }

            return itemsList
                    .OrderByDescending(i => i.DateAdded)
                    .Skip((pageNumber - 1) * ServerConstants.ItemsOnPage)
                    .Take(ServerConstants.ItemsOnPage)
                    .ToList();
        }
            
        public int GetItemsOfTypeCount(ItemType itemType)
        {
            var itemsCount = this.data.Items.All.Where(i => i.ItemType == itemType).Count();

            return itemsCount;
        }

        public int GetAllItemsCount()
        {
            var itemsCount = this.data.Items.All.Count();

            return itemsCount;
        }

        public int GetAllItemsInPromotionCount()
        {
            var itemsCount = this.data.Items.All
                .Where(i => i.Discount != 0 && !i.IsDeleted && i.Quantity != 0).Count();

            return itemsCount;
        }
    }
}
