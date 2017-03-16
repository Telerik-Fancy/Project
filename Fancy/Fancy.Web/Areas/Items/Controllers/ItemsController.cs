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
        private IImageConverter imageConverter;

        public ItemsController(IItemService itemService, IMappingService mappingService, IImageConverter imageConverter)
        {
            this.itemService = itemService;
            this.mappingService = mappingService;
            this.imageConverter = imageConverter;
        }

        public ActionResult GalleryItems(int pageNumber, string itemType)
        {
            ItemType type = (ItemType) Enum.Parse(typeof(ItemType), itemType, true);

            var itemsOfTypeCount = this.itemService.GetItemsOfTypeCount(type);
            var dbItemsList = this.itemService.GetItemsOfType(type, pageNumber);
            var viewItemsList = this.ConvertToViewItemList(dbItemsList);

            ViewBag.GalleryTitle = this.SetViewTitleFormItemType(itemType);
            ViewBag.PageButtonsCount = this.CalculatePageNumberButtonsCount(itemsOfTypeCount);
            ViewBag.ItemsList = viewItemsList;
            ViewBag.ItemType = itemType;

            return View();
        }

        public ActionResult NewItems(int pageNumber)
        {
            var dbItemsList = this.itemService.GetNewestItems(pageNumber);
            var viewItemsList = this.ConvertToViewItemList(dbItemsList);

            ViewBag.ItemsList = viewItemsList;
            ViewBag.GalleryTitle = "New Items";

            return View("GalleryItems");
        }

        public ActionResult Promotions(int pageNumber)
        {
            var dbItemsList = this.itemService.GetItemsInPromotion(pageNumber);
            var viewItemsList = this.ConvertToViewItemList(dbItemsList);

            ViewBag.ItemsList = viewItemsList;
            ViewBag.GalleryTitle = "Items in promotion";

            return View("GalleryItems");
        }

        public ActionResult SingleItem(int itemId)
        {
            var dbItem = this.itemService.GetItemById(itemId);

            var mvItem = this.mappingService.Map<Item, ViewItem>((Item)dbItem);
            string base64 = this.imageConverter.ConvertByteArrayToImageString(mvItem.ImageBytes);
            mvItem.ImageBase64String = string.Format("data:image/gif;base64,{0}", base64);

            ViewBag.Item = mvItem;

            return View();
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