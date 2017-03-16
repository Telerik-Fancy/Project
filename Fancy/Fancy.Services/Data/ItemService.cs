using Fancy.Services.Data.Contracts;
using Fancy.Data.Models.Models;
using Fancy.Data.Data;
using Fancy.Common.Enums;
using System.Linq;
using System.Collections.Generic;
using System;

namespace Fancy.Services.Data
{
    public class ItemService : IItemService
    {
        private readonly IEfFancyData data;

        public ItemService(IEfFancyData data)
        {
            this.data = data;   
        }

        public void AddItem(Item item)
        {
            this.data.Items.Add(item);
            this.data.Commit();
        }

        public Item GetItemById(int id)
        {
            return this.data.Items.GetById(id);
        }

        public IEnumerable<Item> GetItemsOfType(ItemType itemType, int pageNumber)
        {
            var itemsList = this.data.Items.All
                .Where(i => i.ItemType == itemType && !i.IsDeleted && i.Quantity != 0)
                .OrderBy(i => i.Id)
                .Skip((pageNumber - 1) * 6)
                .Take(6)
                .ToList();
            return itemsList;
        }

        public IEnumerable<Item> GetItemsInPromotion(int pageNumber)
        {
            var itemsList = this.data.Items.All
                .Where(i => i.Discount != 0 && !i.IsDeleted && i.Quantity != 0)
                .ToList();
            return itemsList;
        }

        public IEnumerable<Item> GetNewestItems(int pageNumber)
        {
            var itemsList = this.data.Items.All
                .Where(i => !i.IsDeleted && i.Quantity != 0)
                .OrderByDescending(i => i.DateAdded)
                .ToList();
            return itemsList;
        }

        public int GetItemsOfTypeCount(ItemType itemType)
        {
            return this.data.Items.All.Where(i => i.ItemType == itemType).Count();
        }

        public int GetAllItemsCount()
        {
            return this.data.Items.All.Count();
        }
    }
}
