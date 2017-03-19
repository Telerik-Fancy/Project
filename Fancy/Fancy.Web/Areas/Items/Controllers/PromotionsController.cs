﻿using Fancy.Common.Constants;
using Fancy.Services.Common.Contracts;
using Fancy.Services.Data.Contracts;
using System.Web.Mvc;

namespace Fancy.Web.Areas.Items.Controllers
{
    public class PromotionsController : Controller
    {
        private const string SingleItemPageUrl = "~/Items/Items/SingleItem/";
        private IPromotionService promotionService;
        private IMappingService mappingService;

        public PromotionsController(IPromotionService promotionService, IMappingService mappingService)
        {
            this.promotionService = promotionService;
            this.mappingService = mappingService;
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = UserConstants.AdministratorRole)]
        public ActionResult AddPromotion(int itemId, decimal discount)
        {
            this.promotionService.AddPromotion(itemId, discount);

            return this.Redirect(SingleItemPageUrl + itemId);
        }

        [Authorize(Roles = UserConstants.AdministratorRole)]
        public ActionResult RemovePromotion(int itemId)
        {
            this.promotionService.RemovePromotion(itemId);

            return this.Redirect(SingleItemPageUrl + itemId);
        }
    }
}