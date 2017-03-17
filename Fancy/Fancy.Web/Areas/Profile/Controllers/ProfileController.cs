using Fancy.Data.Models.Models;
using Fancy.Services.Common.Contracts;
using Fancy.Services.Data.Contracts;
using Fancy.Web.Areas.Items.Models;
using Fancy.Web.Areas.Profile.Models;
using Fancy.Web.WebUtils.Contracts;
using System.Collections.Generic;
using System.Web.Mvc;

namespace Fancy.Web.Areas.Profile.Controllers
{
    public class ProfileController : Controller
    {
        private IIdentityProvider identityProvider;
        private IOrderService orderService;
        private IMappingService mappingService;

        public ProfileController(IIdentityProvider identityProvider, IOrderService orderService, IMappingService mappingService)
        {
            this.identityProvider = identityProvider;
            this.orderService = orderService;
            this.mappingService = mappingService;
        }

        public ActionResult ProfilePage(ProfilePageViewModel model)
        {
            var userId = this.identityProvider.GetUserId();

            var dbOrderInBasket = this.orderService.GetOrderInBasket(userId);
            var mvOrderInBasket = this.mappingService.Map<Order, OrderViewModel>((Order)dbOrderInBasket);

            model.OrderInBasket = mvOrderInBasket;

            var dbPreviousOrders = this.orderService.PreviousOrders(userId);
            model.PreviousOrders = this.ConvertToOrderViewModelList(dbPreviousOrders);

            return View(model);
        }

        public ActionResult AddItemToBasket(int itemId)
        {
            string userId = this.identityProvider.GetUserId();

            this.orderService.AddItemToBasket(itemId, userId);

            return this.Redirect("~/Items/Items/SingleItem/" + itemId);
        }

        public ActionResult RemoveItemFromBasket(OrderViewModel model, int itemId)
        {
            string userId = this.identityProvider.GetUserId();

            this.orderService.RemoveItemFromBasket(itemId, userId);

            return this.Redirect("~/Profile/Profile/ProfilePage");
        }

        public ActionResult ExecuteOrder(int orderId)
        {
            this.orderService.ExecuteOrder(orderId);

            return this.Redirect("~/Profile/Profile/ProfilePage");
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