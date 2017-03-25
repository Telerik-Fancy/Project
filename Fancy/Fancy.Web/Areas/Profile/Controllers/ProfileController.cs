using System.Collections.Generic;
using System.Web.Mvc;
using Fancy.Services.Common.Contracts;
using Fancy.Services.Data.Contracts;
using Fancy.Web.Areas.Profile.Models;
using Fancy.Web.WebUtils.Contracts;
using Fancy.Common.Constants;
using Fancy.Common.Validator;
using Fancy.Data.Models.Models;

namespace Fancy.Web.Areas.Profile.Controllers
{
    public class ProfileController : Controller
    {
        private IOrderService orderService;
        private IMappingService mappingService;
        private IImageProvider imageProvider;
        private IIdentityProvider identityProvider;

        public ProfileController(IOrderService orderService, IMappingService mappingService, IImageProvider imageProvider, IIdentityProvider identityProvider)
        {
            Validator.ValidateNullArgument(orderService, "orderService");
            Validator.ValidateNullArgument(mappingService, "mappingService");
            Validator.ValidateNullArgument(imageProvider, "imageProvider");
            Validator.ValidateNullArgument(identityProvider, "identityProvider");

            this.identityProvider = identityProvider;
            this.orderService = orderService;
            this.mappingService = mappingService;
            this.imageProvider = imageProvider;
        }

        [Authorize(Roles = UserConstants.AdministratorOrRegular)]
        public ActionResult ProfilePage(ProfilePageViewModel model)
        {
            var userId = this.identityProvider.GetUserId();

            var dbOrderInBasket = this.orderService.GetOrderInBasket(userId);
            var mvOrderInBasket = this.mappingService.Map<Order, OrderViewModel>(dbOrderInBasket);

            model.OrderInBasket = mvOrderInBasket;

            var dbPreviousOrders = this.orderService.PreviousOrders(userId);
            model.PreviousOrders = this.ConvertToOrderViewModelList(dbPreviousOrders);

            return View(model);
        }

        [Authorize(Roles = UserConstants.AdministratorOrRegular)]
        public ActionResult AddItemToBasket(int itemId)
        {
            Validator.ValidateRange(itemId, ServerConstants.IdMinValue, ServerConstants.IdMaxValue, "itemId");

            string userId = this.identityProvider.GetUserId();

            this.orderService.AddItemToBasket(itemId, userId);

            return this.Redirect(ServerConstants.SingleItemRedirectUrl + itemId);
        }

        [Authorize(Roles = UserConstants.AdministratorOrRegular)]
        public ActionResult RemoveItemFromBasket(OrderViewModel model, int itemId)
        {
            Validator.ValidateRange(itemId, ServerConstants.IdMinValue, ServerConstants.IdMaxValue, "itemId");

            string userId = this.identityProvider.GetUserId();

            this.orderService.RemoveItemFromBasket(itemId, userId);

            return this.Redirect(ServerConstants.ProfilePageRedirectUrl);
        }

        [Authorize(Roles = UserConstants.AdministratorOrRegular)]
        public ActionResult ExecuteOrder(int orderId, decimal totalPrice)
        {
            Validator.ValidateRange(orderId, ServerConstants.IdMinValue, ServerConstants.IdMaxValue, "orderId");
            Validator.ValidateRange(totalPrice, 0, int.MaxValue, "totalPrice");

            this.orderService.ExecuteOrder(orderId, totalPrice);

            return this.Redirect(ServerConstants.ProfilePageRedirectUrl);
        }

        private IEnumerable<OrderViewModel> ConvertToOrderViewModelList(IEnumerable<Order> dbPreviousOrders)
        {
            var orderViewModelList = new List<OrderViewModel>();

            foreach (var dbOrder in dbPreviousOrders)
            {
                var mvOrder = this.mappingService.Map<Order, OrderViewModel>((Order) dbOrder);

                orderViewModelList.Add(mvOrder);
            }

            return orderViewModelList;
        }
    }
}