using Fancy.Services.Data.Contracts;
using Fancy.Data.Models.Models;
using Fancy.Data.Data;
using Fancy.Common.Enums;
using System.Linq;
using System.Collections.Generic;
using System;
using Fancy.Common.Constants;

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

        public IEnumerable<Item> GetItemsOfType(int pageNumber, ItemType itemType, MainColour colourType, MainMaterial materialType, PriceFilterType priceFilter)
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

            if(priceFilter != 0)
            {
                if(priceFilter == PriceFilterType.Ascending)
                {
                    itemsList.OrderBy(i => i.Price);
                }
                else
                {
                    itemsList.OrderByDescending(i => i.Price);
                }
            }
            else
            {
                itemsList.OrderBy(i => i.Id);
            }

            return itemsList
                    .OrderBy(i => i.Id)
                    .Skip((pageNumber - 1) * UiConstants.ItemsOnPage)
                    .Take(UiConstants.ItemsOnPage)
                    .ToList();
        }

        public IEnumerable<Item> GetItemsInPromotion(int pageNumber, MainColour colourType, MainMaterial materialType, PriceFilterType priceFilter)
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

            if (priceFilter != 0)
            {
                if (priceFilter == PriceFilterType.Ascending)
                {
                    itemsList.OrderBy(i => i.Price);
                }
                else
                {
                    itemsList.OrderByDescending(i => i.Price);
                }
            }
            else
            {
                itemsList.OrderBy(i => i.Id);
            }

            return itemsList
                    .OrderBy(i => i.Id)
                    .Skip((pageNumber - 1) * UiConstants.ItemsOnPage)
                    .Take(UiConstants.ItemsOnPage)
                    .ToList();
        }

        public IEnumerable<Item> GetNewestItems(int pageNumber, MainColour colourType, MainMaterial materialType, PriceFilterType priceFilter)
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

            if (priceFilter != 0)
            {
                if (priceFilter == PriceFilterType.Ascending)
                {
                    itemsList.ThenBy(i => i.Price);
                }
                else
                {
                    itemsList.ThenByDescending(i => i.Price);
                }
            }

            return itemsList
                    .OrderByDescending(i => i.DateAdded)
                    .Skip((pageNumber - 1) * UiConstants.ItemsOnPage)
                    .Take(UiConstants.ItemsOnPage)
                    .ToList();
        }
            
        public int GetItemsOfTypeCount(ItemType itemType)
        {
            return this.data.Items.All.Where(i => i.ItemType == itemType).Count();
        }

        public int GetAllItemsCount()
        {
            return this.data.Items.All.Count();
        }

        public int GetAllItemsInPromotionCount()
        {
            return this.data.Items.All
                .Where(i => i.Discount != 0 && !i.IsDeleted && i.Quantity != 0).Count();
        }
    }
}
