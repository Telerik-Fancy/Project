using Fancy.Web.Areas.Admin.Models;
using System;
using System.Web.Mvc;
using Fancy.Web.WebUtils.Contracts;
using Fancy.Services.Common.Contracts;
using Fancy.Data.Models.Models;
using Fancy.Services.Data.Contracts;
using Fancy.Common.Constants;
using Fancy.Common.Validator;

namespace Fancy.Web.Areas.Admin.Controllers
{
    public class AdminController : Controller
    {
        private readonly IImageProvider imageProvider;
        private readonly IMappingService mappingService;
        private readonly IItemService itemService;

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
            if(this.ModelState.IsValid)
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