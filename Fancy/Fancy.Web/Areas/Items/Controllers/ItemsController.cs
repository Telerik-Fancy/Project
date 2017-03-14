using Fancy.Common.Enums;
using Fancy.Services.Common.Contracts;
using Fancy.Services.Data.Contracts;
using Fancy.Web.WebUtils.Contracts;
using System.Web.Mvc;
using System.Collections.Generic;
using Fancy.Web.Areas.Items.Models;
using Fancy.Data.Models.Models;

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

        public ActionResult Necklaces(int pageNumber)
        {
            var dbItemsList = this.itemService.GetAllItemsOfType(ItemType.Necklace, pageNumber);
            var viewItemsList = this.ConvertToViewItemList(dbItemsList);

            ViewBag.ItemsList = viewItemsList;

            return View();
        }

        public ActionResult Earings(int pageNumber)
        {
            var dbItemsList = this.itemService.GetAllItemsOfType(ItemType.Earings, pageNumber);
            var viewItemsList = this.ConvertToViewItemList(dbItemsList);

            ViewBag.ItemsList = viewItemsList;

            return View();
        }

        public ActionResult Bracelets(int pageNumber)
        {
            var dbItemsList = this.itemService.GetAllItemsOfType(ItemType.Bracelet, pageNumber);
            var viewItemsList = this.ConvertToViewItemList(dbItemsList);

            ViewBag.ItemsList = viewItemsList;

            return View();
        }

        public ActionResult Sets(int pageNumber)
        {
            var dbItemsList = this.itemService.GetAllItemsOfType(ItemType.Set, pageNumber);
            var viewItemsList = this.ConvertToViewItemList(dbItemsList);

            ViewBag.ItemsList = viewItemsList;

            return View();
        }

        public ActionResult NewItems(int pageNumber)
        {
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