using AutoMapper;
using Fancy.Common.Enums;
using Fancy.Data.Models.Models;
using Fancy.Web.Areas.Admin.Models;
using System.Web;
using System.Web.Mvc;

namespace Fancy.Web.Areas.Admin.Controllers
{
    public class AdminController : Controller
    {

        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddItem(AddItemViewModel model)
        {
            ItemTypeEnum itemTypeName = model.ItemType;
            MainMaterialEnum materialName = model.MainMaterial;
            MainColourEnum colourName = model.MainColour;
            decimal price = model.Price;
            int quantity = model.Quantity;
            HttpPostedFileBase image = model.Image;

            var mapped = Mapper.Map<AddItemViewModel, Item>();
            return View();
        }
    }
}