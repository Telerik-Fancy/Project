using Fancy.Data.Models.Models;
using Fancy.Services.Common.Contracts;
using Fancy.Services.Data.Contracts;
using Fancy.Web.Areas.Items.Models;
using Fancy.Web.WebUtils.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Fancy.Web.Areas.Items.Controllers
{
    public class PromotionsController : Controller
    {
        private IItemService itemService;
        private IMappingService mappingService;
        private IImageConverter imageConverter;

        public PromotionsController(IItemService itemService, IMappingService mappingService, IImageConverter imageConverter)
        {
            this.itemService = itemService;
            this.mappingService = mappingService;
            this.imageConverter = imageConverter;
        }

        public ActionResult Promotions(int pageNumber)
        {
            var dbItemsList = this.itemService.GetNewestItems(pageNumber);
            var viewItemsList = this.ConvertToViewItemList(dbItemsList);

            ViewBag.ItemsList = viewItemsList;

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
    }
}