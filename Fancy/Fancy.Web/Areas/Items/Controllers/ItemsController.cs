﻿using System;
using System.Collections.Generic;
using System.Web.Mvc;
using Fancy.Data.Models.Models;
using Fancy.Services.Common.Contracts;
using Fancy.Services.Data.Contracts;
using Fancy.Web.Areas.Items.Models;
using Fancy.Web.WebUtils.Contracts;
using Fancy.Common.Constants;
using Fancy.Common.Validator;
using Fancy.Common.Enums;

namespace Fancy.Web.Areas.Items.Controllers
{
    public class ItemsController : Controller
    {
        private IItemService itemService;
        private IMappingService mappingService;
        private IImageProvider imageProvider;

        public ItemsController(IItemService itemService, IMappingService mappingService, IImageProvider imageProvider)
        {
            Validator.ValidateNullArgument(itemService, "itemService");
            Validator.ValidateNullArgument(mappingService, "mappingService");
            Validator.ValidateNullArgument(imageProvider, "imageProvider");

            this.itemService = itemService;
            this.mappingService = mappingService;
            this.imageProvider = imageProvider;
        }

        public ActionResult GalleryItems(GalleryItemsViewModel model, int pageNumber, string type)
        {
            Validator.ValidateNullArgument(model, "model");
            Validator.ValidateRange(pageNumber, 1, int.MaxValue, "pageNumber");
            Validator.ValidateNullArgument(type, "type");

            ItemType itemType = (ItemType)Enum.Parse(typeof(ItemType), type, true);

            var itemsOfTypeCount = this.itemService.GetItemsOfTypeCount(itemType, model.Colour, model.Material);
            var dbItemsList = this.itemService.GetItemsOfType(pageNumber, itemType, model.Colour, model.Material);
            var viewItemsList = this.ConvertToViewItemList(dbItemsList);

            model.GalleryTitle = this.SetViewTitleFormItemType(itemType);
            model.PageButtonsCount = this.CalculatePageNumberButtonsCount(itemsOfTypeCount);
            model.ItemsList = viewItemsList;
            model.ItemType = type;

            ViewData["controllerName"] = "GalleryItems";

            return View(model);
        }

        public ActionResult GalleryItemsNew(GalleryItemsViewModel model, int pageNumber)
        {
            Validator.ValidateNullArgument(model, "model");
            Validator.ValidateRange(pageNumber, 1, int.MaxValue, "pageNumber");

            var itemsCount = this.itemService.GetAllItemsCount(model.Colour, model.Material);
            var dbItemsList = this.itemService.GetNewestItems(pageNumber, model.Colour, model.Material);
            var viewItemsList = this.ConvertToViewItemList(dbItemsList);

            model.GalleryTitle = ServerConstants.GalleryNewItemsHeader;
            model.PageButtonsCount = this.CalculatePageNumberButtonsCount(itemsCount);
            model.ItemsList = viewItemsList;

            ViewData["controllerName"] = "GalleryItemsNew";
            
            return View("GalleryItems", model);
        }

        public ActionResult GalleryItemsPromotions(GalleryItemsViewModel model, int pageNumber)
        {
            Validator.ValidateNullArgument(model, "model");
            Validator.ValidateRange(pageNumber, 1, int.MaxValue, "pageNumber");

            var itemsCount = this.itemService.GetAllItemsInPromotionCount(model.Colour, model.Material);
            var dbItemsList = this.itemService.GetItemsInPromotion(pageNumber, model.Colour, model.Material);
            var viewItemsList = this.ConvertToViewItemList(dbItemsList);

            model.GalleryTitle = ServerConstants.GalleryPromotionItemsHeader;
            model.PageButtonsCount = this.CalculatePageNumberButtonsCount(itemsCount);
            model.ItemsList = viewItemsList;

            ViewData["controllerName"] = "GalleryItemsPromotions";

            return View("GalleryItems", model);
        }

        [Authorize(Roles = UserConstants.AdministratorOrRegular)]
        public ActionResult SingleItem(SingleItemViewModel model, int itemId)
        {
            Validator.ValidateNullArgument(model, "model");
            Validator.ValidateRange(itemId, ServerConstants.IdMinValue, ServerConstants.IdMaxValue, "itemId");

            var dbItem = this.itemService.GetItemById(itemId);

            model = this.mappingService.Map<Item, SingleItemViewModel>(dbItem);

            return View(model);
        }

        private IEnumerable<SingleItemViewModel> ConvertToViewItemList(IEnumerable<Item> dbItemsList)
        {
            var viewItemsList = new List<SingleItemViewModel>();

            foreach (var dbItem in dbItemsList)
            {
                var mvItem = this.mappingService.Map<Item, SingleItemViewModel>((Item)dbItem);

                viewItemsList.Add(mvItem);
            }

            return viewItemsList;
        }

        private string SetViewTitleFormItemType(ItemType itemType)
        {
            string viewTitle = null;

            if(itemType == ItemType.Necklace)
            {
                viewTitle = ServerConstants.GalleryNecklacesHeader;
            }
            else if(itemType == ItemType.Earings)
            {
                viewTitle = ServerConstants.GalleryEaringsHeader;
            }
            else if (itemType == ItemType.Bracelet)
            {
                viewTitle = ServerConstants.GalleryBraceletsHeader;
            }
            else if (itemType == ItemType.Set)
            {
                viewTitle = ServerConstants.GallerySetsHeader;
            }
            else
            {
                viewTitle = itemType.ToString();
            }

            return viewTitle;
        }

        private int CalculatePageNumberButtonsCount(int itemsCount)
        {
            var pageButtonsCount = itemsCount / ServerConstants.ItemsOnPage;
            if(pageButtonsCount != 0 && pageButtonsCount * ServerConstants.ItemsOnPage < itemsCount)
            {
                pageButtonsCount++;
            }

            return pageButtonsCount;
        }
    }
}