using Fancy.Common.Constants;
using Fancy.Common.Validator;
using Fancy.Services.Common.Contracts;
using Fancy.Services.Data.Contracts;
using Fancy.Web.Areas.Items.Models;
using System.Web.Mvc;

namespace Fancy.Web.Areas.Items.Controllers
{
    public class PromotionsController : Controller
    {
        private IPromotionService promotionService;
        private IMappingService mappingService;

        public PromotionsController(IPromotionService promotionService, IMappingService mappingService)
        {
            Validator.ValidateNullArgument(promotionService, "promotionService");
            Validator.ValidateNullArgument(mappingService, "mappingService");

            this.promotionService = promotionService;
            this.mappingService = mappingService;
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = UserConstants.AdministratorRole)]
        public ActionResult AddPromotion(int itemId, decimal discount)
        {
            Validator.ValidateRange(itemId, ServerConstants.IdMinValue, ServerConstants.IdMaxValue, "itemId");
            Validator.ValidateRange(discount, ServerConstants.DiscountMinValue, ServerConstants.DiscountMaxValue, "discount");

            this.promotionService.AddPromotion(itemId, discount);

            return this.Redirect(ServerConstants.SingleItemRedirectUrl + itemId);
        }

        [Authorize(Roles = UserConstants.AdministratorRole)]
        public ActionResult RemovePromotion(int itemId)
        {
            Validator.ValidateRange(itemId, ServerConstants.IdMinValue, ServerConstants.IdMaxValue, "itemId");

            this.promotionService.RemovePromotion(itemId);

            return this.Redirect(ServerConstants.SingleItemRedirectUrl + itemId);
        }
    }
}