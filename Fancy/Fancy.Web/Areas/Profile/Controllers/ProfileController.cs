﻿using Bytes2you.Validation;
using Fancy.Common.Constants;
using Fancy.Data.Models.Models;
using Fancy.Services.Common.Contracts;
using Fancy.Services.Data.Contracts;
using Fancy.Web.Areas.Profile.Models;
using Fancy.Web.WebUtils.Contracts;
using System.Collections.Generic;
using System.Web.Mvc;

namespace Fancy.Web.Areas.Profile.Controllers
{
    public class ProfileController : Controller
    {
        private const string SingleItemPageUrl = "~/Items/Items/SingleItem/";
        private const string ProfilePageUrl = "~/Profile/Profile/ProfilePage";

        private IIdentityProvider identityProvider;
        private IOrderService orderService;
        private IMappingService mappingService;
        private IImageProvider imageProvider;

        public ProfileController(IOrderService orderService, IMappingService mappingService, IImageProvider imageProvider, IIdentityProvider identityProvider)
        {
            Guard.WhenArgument(orderService, nameof(orderService)).IsNull().Throw();
            Guard.WhenArgument(mappingService, nameof(mappingService)).IsNull().Throw();
            Guard.WhenArgument(imageProvider, nameof(imageProvider)).IsNull().Throw();
            Guard.WhenArgument(identityProvider, nameof(identityProvider)).IsNull().Throw();

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
            var mvOrderInBasket = this.mappingService.Map<Order, OrderViewModel>((Order)dbOrderInBasket);

            model.OrderInBasket = mvOrderInBasket;

            var dbPreviousOrders = this.orderService.PreviousOrders(userId);
            model.PreviousOrders = this.ConvertToOrderViewModelList(dbPreviousOrders);

            return View(model);
        }

        [Authorize(Roles = UserConstants.AdministratorOrRegular)]
        public ActionResult AddItemToBasket(int itemId)
        {
            string userId = this.identityProvider.GetUserId();

            this.orderService.AddItemToBasket(itemId, userId);

            return this.Redirect(SingleItemPageUrl + itemId);
        }

        [Authorize(Roles = UserConstants.AdministratorOrRegular)]
        public ActionResult RemoveItemFromBasket(OrderViewModel model, int itemId)
        {
            string userId = this.identityProvider.GetUserId();

            this.orderService.RemoveItemFromBasket(itemId, userId);

            return this.Redirect(ProfilePageUrl);
        }

        [Authorize(Roles = UserConstants.AdministratorOrRegular)]
        public ActionResult ExecuteOrder(int orderId, decimal totalPrice)
        {
            this.orderService.ExecuteOrder(orderId, totalPrice);

            return this.Redirect(ProfilePageUrl);
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