using System;
using System.Text;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Fancy.Services.Data.Contracts;
using Fancy.Web.WebUtils.Contracts;
using Fancy.Services.Common.Contracts;
using Fancy.Web.Areas.Profile.Controllers;
using Fancy.Data.Models.Models;
using Fancy.Web.Areas.Profile.Models;
using System.Web.Mvc;
using System.Linq;
using Fancy.Common.Constants;

namespace Fancy.Tests.Web.Areas.Profile.Controllers
{
    [TestClass]
    public class ProfileControllerTests
    {
        private Mock<IOrderService> orderServiceMock;
        private Mock<IMappingService> mappingServiceMock;
        private Mock<IImageProvider> imageProviderMock;
        private Mock<IIdentityProvider> identityProviderMock;

        [TestInitialize()]
        public void Initialize()
        {
            this.orderServiceMock = new Mock<IOrderService>();
            this.mappingServiceMock = new Mock<IMappingService>();
            this.imageProviderMock = new Mock<IImageProvider>();
            this.identityProviderMock = new Mock<IIdentityProvider>();
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Constructor_ShouldThrowArgumentNullException_WhenOrderServiceIsNull()
        {
            // Arrange, Act & Assert
            var profileController = new ProfileController(null, this.mappingServiceMock.Object, this.imageProviderMock.Object, this.identityProviderMock.Object);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Constructor_ShouldThrowArgumentNullException_WhenMappingServiceIsNull()
        {
            // Arrange, Act & Assert
            var profileController = new ProfileController(this.orderServiceMock.Object, null, this.imageProviderMock.Object, this.identityProviderMock.Object);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Constructor_ShouldThrowArgumentNullException_WhenImageProviderIsNull()
        {
            // Arrange, Act & Assert
            var profileController = new ProfileController(this.orderServiceMock.Object, this.mappingServiceMock.Object, null, this.identityProviderMock.Object);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Constructor_ShouldThrowArgumentNullException_WhenIdntityProviderIsNull()
        {
            // Arrange, Act & Assert
            var profileController = new ProfileController(this.orderServiceMock.Object, this.mappingServiceMock.Object, this.imageProviderMock.Object, null);
        }

        [TestMethod]
        public void ProfilePage_ShouldCallAllNeededServicesToLoadProfilePage()
        {
            // Arrange
            var userId = "12345";
            var order = new Order();
            var orderVm = new OrderViewModel();
            var previousOrders = new List<Order>();

            this.identityProviderMock.Setup(i => i.GetUserId()).Returns(userId);
            this.orderServiceMock.Setup(o => o.GetOrderInBasket(userId)).Returns(order);
            this.mappingServiceMock.Setup(m => m.Map<Order, OrderViewModel>(order));
            this.orderServiceMock.Setup(o => o.PreviousOrders(userId)).Returns(previousOrders);

            var model = new ProfilePageViewModel();

            var profileController = new ProfileController(this.orderServiceMock.Object, this.mappingServiceMock.Object, this.imageProviderMock.Object, this.identityProviderMock.Object);

            // Act 
            profileController.ProfilePage(model);

            // Assert
            this.identityProviderMock.Verify(i => i.GetUserId(), Times.Once);
            this.orderServiceMock.Verify(o => o.GetOrderInBasket(userId), Times.Once);
            this.mappingServiceMock.Verify(m => m.Map<Order, OrderViewModel>(order), Times.Once);
            this.orderServiceMock.Verify(o => o.PreviousOrders(userId), Times.Once);
        }

        [TestMethod]
        public void ProfilePage_ShouldReturnNotNullViewResult()
        {
            // Arrange
            var userId = "12345";
            var order = new Order();
            var orderVm = new OrderViewModel();
            var previousOrders = new List<Order>();

            this.identityProviderMock.Setup(i => i.GetUserId()).Returns(userId);
            this.orderServiceMock.Setup(o => o.GetOrderInBasket(userId)).Returns(order);
            this.mappingServiceMock.Setup(m => m.Map<Order, OrderViewModel>(order));
            this.orderServiceMock.Setup(o => o.PreviousOrders(userId)).Returns(previousOrders);

            var model = new ProfilePageViewModel();

            var profileController = new ProfileController(this.orderServiceMock.Object, this.mappingServiceMock.Object, this.imageProviderMock.Object, this.identityProviderMock.Object);

            // Act
            var viewResult = profileController.ProfilePage(model) as ViewResult;

            // Assert
            Assert.IsNotNull(viewResult);
        }

        [TestMethod]
        public void ProfilePage_ShouldHaveAuthorizeAttribute_WithAdministratorOrRegularRole()
        {
            // Arrange
            var promotionsController = new ProfileController(this.orderServiceMock.Object, this.mappingServiceMock.Object, this.imageProviderMock.Object, this.identityProviderMock.Object);
            var profilePageMethod = typeof(ProfileController).GetMethod("ProfilePage");

            // Act
            var attribute = profilePageMethod.GetCustomAttributes(typeof(AuthorizeAttribute), true).Single() as AuthorizeAttribute;

            // Assert
            Assert.IsNotNull(attribute);
            StringAssert.Contains(UserConstants.AdministratorOrRegular, attribute.Roles);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void AddItemToBasket_ShouldThrowArgumentOutOfRangeException_WhenItemIdIsNegative()
        {
            // Arrange
            var promotionsController = new ProfileController(this.orderServiceMock.Object, this.mappingServiceMock.Object, this.imageProviderMock.Object, this.identityProviderMock.Object);
            var invalidItemId = -5;

            // Act & Assert
            promotionsController.AddItemToBasket(invalidItemId);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void AddItemToBasket_ShouldThrowArgumentOutOfRangeException_WhenItemIdIsZero()
        {
            // Arrange
            var promotionsController = new ProfileController(this.orderServiceMock.Object, this.mappingServiceMock.Object, this.imageProviderMock.Object, this.identityProviderMock.Object);
            var invalidItemId = 0;

            // Act & Assert
            promotionsController.AddItemToBasket(invalidItemId);
        }

        [TestMethod]
        public void AddItemToBasket_ShouldRedirectToProfilePage()
        {
            // Arrange
            var userId = "12345";
            var itemId = 11;
            var order = new Order();
            var orderVm = new OrderViewModel();
            var previousOrders = new List<Order>();

            this.identityProviderMock.Setup(i => i.GetUserId()).Returns(userId);
            this.orderServiceMock.Setup(o => o.GetOrderInBasket(userId)).Returns(order);
            this.mappingServiceMock.Setup(m => m.Map<Order, OrderViewModel>(order));
            this.orderServiceMock.Setup(o => o.PreviousOrders(userId)).Returns(previousOrders);

            var model = new ProfilePageViewModel();

            var profileController = new ProfileController(this.orderServiceMock.Object, this.mappingServiceMock.Object, this.imageProviderMock.Object, this.identityProviderMock.Object);

            // Act
            var redirectResult = profileController.AddItemToBasket(itemId) as RedirectResult;

            // Assert
            Assert.IsNotNull(redirectResult);
            Assert.AreEqual(ServerConstants.SingleItemRedirectUrl + itemId, redirectResult.Url);
        }

        [TestMethod]
        public void AddItemToBasket_ShouldHaveAuthorizeAttribute_WithAdministratorOrRegularRole()
        {
            // Arrange
            var promotionsController = new ProfileController(this.orderServiceMock.Object, this.mappingServiceMock.Object, this.imageProviderMock.Object, this.identityProviderMock.Object);
            var addItemToBasketMethod = typeof(ProfileController).GetMethod("AddItemToBasket");

            // Act
            var attribute = addItemToBasketMethod.GetCustomAttributes(typeof(AuthorizeAttribute), true).Single() as AuthorizeAttribute;

            // Assert
            Assert.IsNotNull(attribute);
            StringAssert.Contains(UserConstants.AdministratorOrRegular, attribute.Roles);
        }

        [TestMethod]
        public void AddItemToBasket_ShouldCallOrderService_AddItemToBasket()
        {
            // Arrange
            var userId = "12345";
            var itemId = 11;
            var order = new Order();
            var orderVm = new OrderViewModel();
            var previousOrders = new List<Order>();

            this.identityProviderMock.Setup(i => i.GetUserId()).Returns(userId);
            this.orderServiceMock.Setup(o => o.AddItemToBasket(itemId, userId));

            var model = new ProfilePageViewModel();

            var profileController = new ProfileController(this.orderServiceMock.Object, this.mappingServiceMock.Object, this.imageProviderMock.Object, this.identityProviderMock.Object);

            // Act
            profileController.AddItemToBasket(itemId);

            // Assert
            this.orderServiceMock.Verify(o => o.AddItemToBasket(itemId, userId), Times.Once);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void RemoveItemFromBasket_ShouldThrowArgumentOutOfRangeException_WhenItemIdIsNegative()
        {
            // Arrange
            var promotionsController = new ProfileController(this.orderServiceMock.Object, this.mappingServiceMock.Object, this.imageProviderMock.Object, this.identityProviderMock.Object);
            var invalidItemId = -5;
            var orderVm = new OrderViewModel();

            // Act & Assert
            promotionsController.RemoveItemFromBasket(orderVm, invalidItemId);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void RemoveItemFromBasket_ShouldThrowArgumentOutOfRangeException_WhenItemIdIsZero()
        {
            // Arrange
            var promotionsController = new ProfileController(this.orderServiceMock.Object, this.mappingServiceMock.Object, this.imageProviderMock.Object, this.identityProviderMock.Object);
            var invalidItemId = 0;
            var orderVm = new OrderViewModel();

            // Act & Assert
            promotionsController.RemoveItemFromBasket(orderVm, invalidItemId);
        }

        [TestMethod]
        public void RemoveItemFromBasket_ShouldRedirectToProfilePage()
        {
            // Arrange
            var userId = "12345";
            var itemId = 11;
            var order = new Order();
            var orderVm = new OrderViewModel();
            var previousOrders = new List<Order>();

            this.identityProviderMock.Setup(i => i.GetUserId()).Returns(userId);
            this.orderServiceMock.Setup(o => o.GetOrderInBasket(userId)).Returns(order);
            this.mappingServiceMock.Setup(m => m.Map<Order, OrderViewModel>(order));
            this.orderServiceMock.Setup(o => o.PreviousOrders(userId)).Returns(previousOrders);

            var model = new ProfilePageViewModel();

            var profileController = new ProfileController(this.orderServiceMock.Object, this.mappingServiceMock.Object, this.imageProviderMock.Object, this.identityProviderMock.Object);

            // Act
            var redirectResult = profileController.RemoveItemFromBasket(orderVm, itemId) as RedirectResult;

            // Assert
            Assert.IsNotNull(redirectResult);
            Assert.AreEqual(ServerConstants.ProfilePageRedirectUrl, redirectResult.Url);
        }

        [TestMethod]
        public void RemoveItemFromBasket_ShouldHaveAuthorizeAttribute_WithAdministratorOrRegularRole()
        {
            // Arrange
            var promotionsController = new ProfileController(this.orderServiceMock.Object, this.mappingServiceMock.Object, this.imageProviderMock.Object, this.identityProviderMock.Object);
            var removeItemFromBasketMethod = typeof(ProfileController).GetMethod("RemoveItemFromBasket");
            var orderVm = new OrderViewModel();

            // Act
            var attribute = removeItemFromBasketMethod.GetCustomAttributes(typeof(AuthorizeAttribute), true).Single() as AuthorizeAttribute;

            // Assert
            Assert.IsNotNull(attribute);
            StringAssert.Contains(UserConstants.AdministratorOrRegular, attribute.Roles);
        }

        [TestMethod]
        public void RemoveItemFromBasket_ShouldCallOrderService_RemoveItemFromBasket()
        {
            // Arrange
            var userId = "12345";
            var itemId = 11;
            var order = new Order();
            var orderVm = new OrderViewModel();
            var previousOrders = new List<Order>();

            this.identityProviderMock.Setup(i => i.GetUserId()).Returns(userId);
            this.orderServiceMock.Setup(o => o.RemoveItemFromBasket(itemId, userId));

            var model = new ProfilePageViewModel();

            var profileController = new ProfileController(this.orderServiceMock.Object, this.mappingServiceMock.Object, this.imageProviderMock.Object, this.identityProviderMock.Object);

            // Act
            profileController.RemoveItemFromBasket(orderVm, itemId);

            // Assert
            this.orderServiceMock.Verify(o => o.RemoveItemFromBasket(itemId, userId), Times.Once);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void ExecuteOrder_ShouldThrowArgumentOutOfRangeException_WhenOrderIdIsNegative()
        {
            // Arrange
            var promotionsController = new ProfileController(this.orderServiceMock.Object, this.mappingServiceMock.Object, this.imageProviderMock.Object, this.identityProviderMock.Object);
            var invalidOrderId = -10;
            var validTotalPrice = 20;

            // Act & Assert
            promotionsController.ExecuteOrder(invalidOrderId, validTotalPrice);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void ExecuteOrder_ShouldThrowArgumentOutOfRangeException_WhenItemIdIsZero()
        {
            // Arrange
            var promotionsController = new ProfileController(this.orderServiceMock.Object, this.mappingServiceMock.Object, this.imageProviderMock.Object, this.identityProviderMock.Object);
            var invalidOrderId = 0;
            var validTotalPrice = 20;

            // Act & Assert
            promotionsController.ExecuteOrder(invalidOrderId, validTotalPrice);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void ExecuteOrder_ShouldThrowArgumentOutOfRangeException_WhenTotalPriceIsNegative()
        {
            // Arrange
            var promotionsController = new ProfileController(this.orderServiceMock.Object, this.mappingServiceMock.Object, this.imageProviderMock.Object, this.identityProviderMock.Object);
            var validOrderId = 5;
            var invalidTotalPrice = -10;

            // Act & Assert
            promotionsController.ExecuteOrder(validOrderId, invalidTotalPrice);
        }

        [TestMethod]
        public void ExecuteOrder_ShouldHaveAuthorizeAttribute_WithAdministratorOrRegularRole()
        {
            // Arrange
            var promotionsController = new ProfileController(this.orderServiceMock.Object, this.mappingServiceMock.Object, this.imageProviderMock.Object, this.identityProviderMock.Object);
            var executeOrderMethod = typeof(ProfileController).GetMethod("ExecuteOrder");

            // Act
            var attribute = executeOrderMethod.GetCustomAttributes(typeof(AuthorizeAttribute), true).Single() as AuthorizeAttribute;

            // Assert
            Assert.IsNotNull(attribute);
            StringAssert.Contains(UserConstants.AdministratorOrRegular, attribute.Roles);
        }

        [TestMethod]
        public void ExecuteOrder_ShouldCallOrderService_ExecuteOrder_TimesOnce()
        {
            // Arrange
            var orderId = 11;
            var totalPrice = 11;

            this.orderServiceMock.Setup(o => o.ExecuteOrder(orderId, totalPrice));

            var model = new ProfilePageViewModel();

            var profileController = new ProfileController(this.orderServiceMock.Object, this.mappingServiceMock.Object, this.imageProviderMock.Object, this.identityProviderMock.Object);

            // Act
            profileController.ExecuteOrder(orderId, totalPrice);

            // Assert
            this.orderServiceMock.Verify(o => o.ExecuteOrder(orderId, totalPrice), Times.Once);
        }

        [TestMethod]
        public void ExecuteOrder_ShouldRedirectToProfilePage()
        {
            // Arrange
            var orderId = 10;
            var totalPrice = 100;

            this.orderServiceMock.Setup(o => o.ExecuteOrder(orderId, totalPrice));

            var model = new ProfilePageViewModel();

            var profileController = new ProfileController(this.orderServiceMock.Object, this.mappingServiceMock.Object, this.imageProviderMock.Object, this.identityProviderMock.Object);

            // Act
            var redirectResult = profileController.ExecuteOrder(orderId, totalPrice) as RedirectResult;

            // Assert
            Assert.IsNotNull(redirectResult);
            Assert.AreEqual(ServerConstants.ProfilePageRedirectUrl, redirectResult.Url);
        }
    }
}
