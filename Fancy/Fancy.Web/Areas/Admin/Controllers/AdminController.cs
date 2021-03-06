﻿using System;
using System.Web.Mvc;
using Fancy.Data.Models.Models;
using Fancy.Services.Data.Contracts;
using Fancy.Services.Common.Contracts;
using Fancy.Web.Areas.Admin.Models;
using Fancy.Web.WebUtils.Contracts;
using Fancy.Common.Constants;
using Fancy.Common.Validator;

namespace Fancy.Web.Areas.Admin.Controllers
{
    public class AdminController : Controller
    {
        private readonly IItemService itemService;
        private readonly IMappingService mappingService;
        private readonly IImageProvider imageProvider;
        
        public AdminController(IItemService itemService, IMappingService mappingService, IImageProvider imageProvider)
        {
            Validator.ValidateNullArgument(itemService, "itemService");
            Validator.ValidateNullArgument(mappingService, "mappingService");
            Validator.ValidateNullArgument(imageProvider, "imageProvider");

            this.itemService = itemService;
            this.mappingService = mappingService; 
            this.imageProvider = imageProvider;
        }

        public ActionResult AdminPanel()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = UserConstants.AdministratorRole)]
        public ActionResult AddItem(AddItemViewModel model)
        {
            ViewBag.ItemCodeNotUnique = string.Empty;
            bool isCodeUnique = this.itemService.CheckUniqueItemCode(model.ItemCode);

            if(this.ModelState.IsValid && isCodeUnique)
            {
                model.DateAdded = DateTime.Now;
                model.IsDeleted = false;
                model.Discount = 0;

                var bytesArray = this.imageProvider.ConvertFileToByteArray(model.Image);
                model.ImageBase64String = this.imageProvider.ConvertByteArrayToImageString(bytesArray);

                var item = this.mappingService.Map<AddItemViewModel, Item>(model);
                this.itemService.AddItem(item);

                this.ModelState.Clear();
            }

            return this.Redirect(ServerConstants.AdminPanelRedirectUrl);
        }
    }
}