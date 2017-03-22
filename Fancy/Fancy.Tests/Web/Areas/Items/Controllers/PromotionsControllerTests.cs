using System;
using System.Text;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Fancy.Services.Data.Contracts;
using Fancy.Services.Common.Contracts;
using Fancy.Web.Areas.Items.Controllers;
using System.Web.Mvc;
using Fancy.Common.Constants;
using System.Linq;
using Fancy.Web.Areas.Items.Models;

namespace Fancy.Tests.Web.Areas.Items.Controllers
{

    [TestClass]
    public class PromotionsControllerTests
    {
        private Mock<IPromotionService> promotionServiceMock;
        private Mock<IMappingService> mappingServiceMock;

        [TestInitialize()]
        public void Initialize()
        {
            this.promotionServiceMock = new Mock<IPromotionService>();
            this.mappingServiceMock = new Mock<IMappingService>();
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Constructor_ShouldThrowArgumentNullException_WhenPromotionServiceIsNull()
        {
            // Arrange, Act & Assert
            var promotionsController = new PromotionsController(null, this.mappingServiceMock.Object);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Constructor_ShouldThrowArgumentNullException_WhenMappingServiceIsNull()
        {
            // Arrange, Act & Assert
            var promotionsController = new PromotionsController(this.promotionServiceMock.Object, null);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void AddPromotion_ShouldThrowArgumentOutOfRangeException_WhenItemIdIsNegative()
        {
            // Arrange
            var promotionsController = new PromotionsController(this.promotionServiceMock.Object, this.mappingServiceMock.Object);
            var invalidItemId = -5;
            var validDiscount = 15;

            // Act & Assert
            promotionsController.AddPromotion(invalidItemId, validDiscount);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void AddPromotion_ShouldThrowArgumentOutOfRangeException_WhenItemIdIsZero()
        {
            // Arrange
            var promotionsController = new PromotionsController(this.promotionServiceMock.Object, this.mappingServiceMock.Object);
            var invalidItemId = 0;
            var validDiscount = 15;

            // Act & Assert
            promotionsController.AddPromotion(invalidItemId, validDiscount);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void AddPromotion_ShouldThrowArgumentOutOfRangeException_WhenDiscountIsLessThanMinValue()
        {
            // Arrange
            var promotionsController = new PromotionsController(this.promotionServiceMock.Object, this.mappingServiceMock.Object);
            var validItemId = 5;
            var invalidDiscount = 3;

            // Act & Assert
            promotionsController.AddPromotion(validItemId, invalidDiscount);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void AddPromotion_ShouldThrowArgumentOutOfRangeException_WhenDiscountIsLessThanMinValueNegative()
        {
            // Arrange
            var promotionsController = new PromotionsController(this.promotionServiceMock.Object, this.mappingServiceMock.Object);
            var validItemId = 5;
            var invalidDiscount = -3;

            // Act & Assert
            promotionsController.AddPromotion(validItemId, invalidDiscount);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void AddPromotion_ShouldThrowArgumentOutOfRangeException_WhenDiscountIsMoreThanMaxValue()
        {
            // Arrange
            var promotionsController = new PromotionsController(this.promotionServiceMock.Object, this.mappingServiceMock.Object);
            var validItemId = 5;
            var invalidDiscount = 96;

            // Act & Assert
            promotionsController.AddPromotion(validItemId, invalidDiscount);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void AddPromotion_ShouldThrowArgumentOutOfRangeException_WhenDiscountIsMoreThanMaxValueAndMoreThanHundredPercent()
        {
            // Arrange
            var promotionsController = new PromotionsController(this.promotionServiceMock.Object, this.mappingServiceMock.Object);
            var validItemId = 5;
            var invalidDiscount = 196;

            // Act & Assert
            promotionsController.AddPromotion(validItemId, invalidDiscount);
        }

        [TestMethod]
        public void AddPromotion_ShouldHaveAuthorizeAttribute_WithAdministratorOrRegularRole()
        {
            // Arrange
            var promotionsController = new PromotionsController(this.promotionServiceMock.Object, this.mappingServiceMock.Object);
            var addPromotionMethod = typeof(PromotionsController).GetMethod("AddPromotion");

            // Act
            var attribute = addPromotionMethod.GetCustomAttributes(typeof(AuthorizeAttribute), true).Single() as AuthorizeAttribute;

            // Assert
            Assert.IsNotNull(attribute);
            StringAssert.Contains(UserConstants.AdministratorRole, attribute.Roles);
        }

        [TestMethod]
        public void AddPromotion_ShouldHavePostAttribute()
        {
            // Arrange
            var promotionsController = new PromotionsController(this.promotionServiceMock.Object, this.mappingServiceMock.Object);
            var addPromotionMethod = typeof(PromotionsController).GetMethod("AddPromotion");

            // Act
            var attribute = addPromotionMethod.GetCustomAttributes(typeof(HttpPostAttribute), true).Single() as HttpPostAttribute;

            // Assert
            Assert.IsNotNull(attribute);
        }

        [TestMethod]
        public void AddPromotion_ShouldHaveValidateAntiForgeryTokenAttribute()
        {
            // Arrange
            var promotionsController = new PromotionsController(this.promotionServiceMock.Object, this.mappingServiceMock.Object);
            var addPromotionMethod = typeof(PromotionsController).GetMethod("AddPromotion");

            // Act
            var attribute = addPromotionMethod.GetCustomAttributes(typeof(ValidateAntiForgeryTokenAttribute), true).Single() as ValidateAntiForgeryTokenAttribute;

            // Assert
            Assert.IsNotNull(attribute);
        }

        [TestMethod]
        public void AddPromotion_ShouldCallPromotionServiceAddPromotion_WhenValidArgumentsArePassed()
        {
            // Arrange
            this.promotionServiceMock.Setup(p => p.AddPromotion(
                It.Is<int>(i => i > ServerConstants.IdMinValue && i < ServerConstants.IdMaxValue),
                It.Is<int>(d => d > ServerConstants.DiscountMinValue && d < ServerConstants.DiscountMaxValue)));

            var promotionsController = new PromotionsController(this.promotionServiceMock.Object, this.mappingServiceMock.Object);
            var itemId = 10;
            var discount = 15;

            // Act
            promotionsController.AddPromotion(itemId, discount);

            // Assert
            this.promotionServiceMock.Verify(p => p.AddPromotion(itemId, discount), Times.Once);
        }

        [TestMethod]
        public void AddPromotion_ShouldRedirectToSingleItemPage()
        {
            //Arange
            var promotionsController = new PromotionsController(this.promotionServiceMock.Object, this.mappingServiceMock.Object);
            var itemId = 10;
            var discount = 15;

            //Act
            var redirectResult = promotionsController.AddPromotion(itemId, discount) as RedirectResult;

            //Assert
            Assert.IsNotNull(redirectResult);
            Assert.AreEqual(ServerConstants.SingleItemRedirectUrl + itemId, redirectResult.Url);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void RemovePromotion_ShouldThrowArgumentOutOfRangeException_WhenItemIdIsNegative()
        {
            // Arrange
            var promotionsController = new PromotionsController(this.promotionServiceMock.Object, this.mappingServiceMock.Object);
            var invalidItemId = -5;

            // Act & Assert
            promotionsController.RemovePromotion(invalidItemId);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void RemovePromotion_ShouldThrowArgumentOutOfRangeException_WhenItemIdIsZero()
        {
            // Arrange
            var promotionsController = new PromotionsController(this.promotionServiceMock.Object, this.mappingServiceMock.Object);
            var invalidItemId = 0;

            // Act & Assert
            promotionsController.RemovePromotion(invalidItemId);
        }
       
        [TestMethod]
        public void RemovePromotion_ShouldHaveAuthorizeAttribute_WithAdministratorOrRegularRole()
        {
            // Arrange
            var promotionsController = new PromotionsController(this.promotionServiceMock.Object, this.mappingServiceMock.Object);
            var addPromotionMethod = typeof(PromotionsController).GetMethod("AddPromotion");

            // Act
            var attribute = addPromotionMethod.GetCustomAttributes(typeof(AuthorizeAttribute), true).Single() as AuthorizeAttribute;

            // Assert
            Assert.IsNotNull(attribute);
            StringAssert.Contains(UserConstants.AdministratorRole, attribute.Roles);
        }
        
        [TestMethod]
        public void RemovePromotion_ShouldCallPromotionServiceAddPromotion_WhenValidArgumentsArePassed()
        {
            // Arrange
            this.promotionServiceMock.Setup(p => p.AddPromotion(
                It.Is<int>(i => i > ServerConstants.IdMinValue && i < ServerConstants.IdMaxValue),
                It.Is<int>(d => d > ServerConstants.DiscountMinValue && d < ServerConstants.DiscountMaxValue)));

            var promotionsController = new PromotionsController(this.promotionServiceMock.Object, this.mappingServiceMock.Object);
            var itemId = 10;

            // Act
            promotionsController.RemovePromotion(itemId);

            // Assert
            this.promotionServiceMock.Verify(p => p.RemovePromotion(itemId), Times.Once);
        }

        [TestMethod]
        public void RemovePromotion_ShouldRedirectToSingleItemPage()
        {
            // Arange
            var promotionsController = new PromotionsController(this.promotionServiceMock.Object, this.mappingServiceMock.Object);
            var itemId = 10;

            // Act
            var redirectResult = promotionsController.RemovePromotion(itemId) as RedirectResult;

            // Assert
            Assert.IsNotNull(redirectResult);
            Assert.AreEqual(ServerConstants.SingleItemRedirectUrl + itemId, redirectResult.Url);
        }
    }
}
