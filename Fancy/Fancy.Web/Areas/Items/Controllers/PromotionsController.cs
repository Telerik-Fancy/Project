using Fancy.Data.Models.Models;
using Fancy.Services.Common.Contracts;
using Fancy.Services.Data.Contracts;
using System.Web.Mvc;

namespace Fancy.Web.Areas.Items.Controllers
{
    public class PromotionsController : Controller
    {
        private IPromotionService promotionService;
        private IMappingService mappingService;

        public PromotionsController(IPromotionService promotionService, IMappingService mappingService)
        {
            this.promotionService = promotionService;
            this.mappingService = mappingService;        }

        public ActionResult AddPromotionToItem()
        {
            return View();
        }

        public ActionResult RemovePromotionFromItem()
        {
            return View();
        }
    }
}