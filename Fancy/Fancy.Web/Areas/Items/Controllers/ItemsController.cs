using Fancy.Common.Enums;
using Fancy.Services.Common.Contracts;
using Fancy.Services.Data.Contracts;
using Fancy.Web.WebUtils.Contracts;
using System.Web.Mvc;
using System.Collections.Generic;
using Fancy.Web.Areas.Items.Models;
using Fancy.Data.Models.Models;
using System;
using Fancy.Common.Constants;

namespace Fancy.Web.Areas.Items.Controllers
{
    public class ItemsController : Controller
    {
        private IItemService itemService;
        private IMappingService mappingService;
        private IImageProvider imageConverter;

        public ItemsController(IItemService itemService, IMappingService mappingService, IImageProvider imageConverter)
        {
            this.itemService = itemService;
            this.mappingService = mappingService;
            this.imageConverter = imageConverter;
        }

        public ActionResult GalleryItems(ViewGalleryItems model, int pageNumber, string type)
        {
            ItemType itemType = (ItemType)Enum.Parse(typeof(ItemType), type, true);

            var itemsOfTypeCount = this.itemService.GetItemsOfTypeCount(itemType);
            var dbItemsList = this.itemService.GetItemsOfType(pageNumber, itemType, model.Colour, model.Material);
            var viewItemsList = this.ConvertToViewItemList(dbItemsList);

            model.GalleryTitle = this.SetViewTitleFormItemType(type);
            model.PageButtonsCount = this.CalculatePageNumberButtonsCount(itemsOfTypeCount);
            model.ItemsList = viewItemsList;
            model.ItemType = type;

            return View(model);
        }

        public ActionResult GalleryItemsNew(ViewGalleryItems model, int pageNumber)
        {
            var itemsCount = this.itemService.GetAllItemsCount();
            var dbItemsList = this.itemService.GetNewestItems(pageNumber, model.Colour, model.Material);
            var viewItemsList = this.ConvertToViewItemList(dbItemsList);

            model.GalleryTitle = "New items";
            model.PageButtonsCount = this.CalculatePageNumberButtonsCount(itemsCount);
            model.ItemsList = viewItemsList;

            return View(model);
        }

        public ActionResult GalleryItemsPromotions(ViewGalleryItems model, int pageNumber)
        {
            var itemsCount = this.itemService.GetAllItemsInPromotionCount();
            var dbItemsList = this.itemService.GetItemsInPromotion(pageNumber, model.Colour, model.Material);
            var viewItemsList = this.ConvertToViewItemList(dbItemsList);

            model.GalleryTitle = "Items in promotion";
            model.PageButtonsCount = this.CalculatePageNumberButtonsCount(itemsCount);
            model.ItemsList = viewItemsList;

            return View(model);
        }

        public ActionResult SingleItem(ViewItem model, int itemId)
        {
            var dbItem = this.itemService.GetItemById(itemId);

            model = this.mappingService.Map<Item, ViewItem>((Item)dbItem);
            string base64 = this.imageConverter.ConvertByteArrayToImageString(model.ImageBytes);
            model.ImageBase64String = string.Format("data:image/gif;base64,{0}", base64);

            return View(model);
        }

        private IEnumerable<ViewItem> ConvertToViewItemList(IEnumerable<Item> dbItemsList)
        {
            var viewItemsList = new List<ViewItem>();

            foreach (var dbItem in dbItemsList)
            {
                var mvItem = this.mappingService.Map<Item, ViewItem>((Item)dbItem);
                string base64 = this.imageConverter.ConvertByteArrayToImageString(mvItem.ImageBytes);
                mvItem.ImageBase64String = string.Format("data:image/gif;base64,{0}", base64);

                viewItemsList.Add(mvItem);
            }

            return viewItemsList;
        }

        private string SetViewTitleFormItemType(string itemType)
        {
            string viewTitle = null;

            if(itemType == "Necklace")
            {
                viewTitle = "Necklaces";
            }
            else if(itemType == "Earings")
            {
                viewTitle = "Earings";
            }
            else if (itemType == "Bracelet")
            {
                viewTitle = "Bracelets";
            }
            else if (itemType == "Set")
            {
                viewTitle = "Sets";
            }
            else
            {
                viewTitle = itemType;
            }

            return viewTitle;
        }

        private int CalculatePageNumberButtonsCount(int itemsCount)
        {
            var pageButtonsCount = itemsCount / UiConstants.ItemsOnPage;
            if(pageButtonsCount != 0 && pageButtonsCount * UiConstants.ItemsOnPage < itemsCount)
            {
                pageButtonsCount++;
            }

            return pageButtonsCount;
        }
    }
}