using Fancy.Web.Areas.Admin.Models;
using System;
using System.Web.Mvc;
using Fancy.Web.WebUtils;
using Fancy.Web.WebUtils.Contracts;
using Fancy.Services.Common.Contracts;
using Fancy.Data.Models.Models;
using Fancy.Services.Data.Contracts;

namespace Fancy.Web.Areas.Admin.Controllers
{
    public class AdminController : Controller
    {
        private readonly IImageConverter imageConverter;
        private readonly IMappingService mappingService;
        private readonly IItemService itemService;

        public AdminController(IImageConverter imageConverter, IMappingService mappingService, IItemService itemService)
        {
            this.imageConverter = imageConverter;
            this.mappingService = mappingService;
            this.itemService = itemService;
        }

        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddItem(AddItemViewModel model)
        {
            model.DateAdded = DateTime.Now;
            model.IsDeleted = false;
            model.Discount = 0;

            model.ImageBytes = this.imageConverter.ConvertFileToByteArray(model.Image);

            var item = this.mappingService.Map<AddItemViewModel, Item>(model);
            this.itemService.AddItem(item);

            ModelState.Clear();
            return View("Index");
        }
    }
}