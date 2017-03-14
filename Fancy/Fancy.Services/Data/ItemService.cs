using Fancy.Services.Data.Contracts;
using Fancy.Data.Models.Models;
using Fancy.Data.Data;
using Fancy.Common.Enums;
using System.Linq;
using System.Collections.Generic;

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

        public IEnumerable<Item> GetAllItemsOfType(ItemType itemType, int pageNumber)
        {
            var itemsList = this.data.Items.All
                .Where(i => i.ItemType == itemType && !i.IsDeleted && i.Quantity != 0)
                .ToList();
            return itemsList;
        }
    }
}
