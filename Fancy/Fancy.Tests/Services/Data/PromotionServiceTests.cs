﻿using System;
using System.Text;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Fancy.Services.Data;
using Fancy.Data.Data;
using Moq;
using Fancy.Data.Repositories;
using Fancy.Data.Models.Models;
using System.Linq.Expressions;
using System.Linq;
using Fancy.Common.Exceptions;

namespace Fancy.Tests.Services.Data
{
    [TestClass]
    public class PromotionServiceTests
    {
        private Mock<IEfFancyData> dataMock;
        private Mock<IEfGenericRepository<Item>> itemRepoMock;

        [TestInitialize()]
        public void Initialize()
        {
            this.dataMock = new Mock<IEfFancyData>();
            this.itemRepoMock = new Mock<IEfGenericRepository<Item>>();
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Constructor_ShouldThrowArgumentNullException_WhenDataParameterIsNull()
        {
            // Arrange & Act & Assert
            var promotionService = new PromotionService(null);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void AddPromotion_ShouldThrowArgumentOutOfRangeException_WhenItemIdParameterIsZero()
        {
            // Arrange
            var invalidItemId = 0;
            var validDiscount = 10;
            var promotionService = new PromotionService(this.dataMock.Object);
            
            // Act & Assert
            promotionService.AddPromotion(invalidItemId, validDiscount);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void AddPromotion_ShouldThrowArgumentOutOfRangeException_WhenItemIdParameterIsLessZero()
        {
            // Arrange
            var invalidItemId = -5;
            var validDiscount = 10;
            var promotionService = new PromotionService(this.dataMock.Object);
            
            // Act & Assert
            promotionService.AddPromotion(invalidItemId, validDiscount);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void AddPromotion_ShouldThrowArgumentOutOfRangeException_WhenDiscountParameterIsLessThanMinimumAllowed()
        {
            // Arrange
            var validItemId = 5;
            var invalidDiscount = 3;
            var promotionService = new PromotionService(this.dataMock.Object);
            
            // Act & Assert
            promotionService.AddPromotion(validItemId, invalidDiscount);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void AddPromotion_ShouldThrowArgumentOutOfRangeException_WhenDiscountParameterIsMoreThanMaximumAllowed()
        {
            // Arrange
            var validItemId = 1;
            var invalidDiscount = 93;
            var promotionService = new PromotionService(this.dataMock.Object);
            
            // Act & Assert
            promotionService.AddPromotion(validItemId, invalidDiscount);
        }

        [TestMethod]
        [ExpectedException(typeof(ObjectNotFoundInDatabaseException))]
        public void AddPromotion_ShouldThrowObjectNotFoundInDatabaseException_WhenNoObjectWithTheGivenIdIsFound()
        {
            // Arrange
            var itemId = 10;
            var discount = 50;
            Item item = null;

            this.itemRepoMock.Setup(r => r.GetById(It.IsAny<int>())).Returns(item);
            this.dataMock.SetupGet(d => d.Items).Returns(this.itemRepoMock.Object);

            var promotionService = new PromotionService(this.dataMock.Object);
            
            // Act & Assert
            promotionService.AddPromotion(itemId, discount);
        }

        [TestMethod]
        public void AddPromotion_ShouldCallDataGetById_WhenAllParametersAreValid()
        {
            // Arrange
            var itemId = 15;
            var discount = 15;
            var item = new Item();

            this.itemRepoMock.Setup(r => r.GetById(It.IsAny<int>())).Returns(item);
            this.dataMock.SetupGet(d => d.Items).Returns(this.itemRepoMock.Object);

            var promotionService = new PromotionService(this.dataMock.Object);

            // Act
            promotionService.AddPromotion(itemId, discount);

            // Assert
            itemRepoMock.Verify(r => r.GetById(itemId), Times.Once);
        }

        [TestMethod]
        public void AddPromotion_ShouldCallDataCommit_WhenAllParametersAreValid()
        {
            // Arrange
            var itemId = 15;
            var discount = 15;
            var item = new Item();

            this.itemRepoMock.Setup(r => r.GetById(It.IsAny<int>())).Returns(item);
            this.dataMock.SetupGet(d => d.Items).Returns(this.itemRepoMock.Object);

            var promotionService = new PromotionService(this.dataMock.Object);

            // Act
            promotionService.AddPromotion(itemId, discount);

            // Assert
            dataMock.Verify(d => d.Commit(), Times.Once);
        }

        [TestMethod]
        public void AddPromotion_ShouldIncreaseItemDiscount_WhenAllParametersAreValid()
        {
            // Arrange
            var itemId = 53;
            var discount = 15;
            var item = new Item();

            this.itemRepoMock.Setup(r => r.GetById(It.IsAny<int>())).Returns(item);
            this.dataMock.SetupGet(d => d.Items).Returns(this.itemRepoMock.Object);

            var promotionService = new PromotionService(this.dataMock.Object);

            // Act
            promotionService.AddPromotion(itemId, discount);

            // Assert
            Assert.AreEqual(discount, item.Discount);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void RemovePromotion_ShouldThrowArgumentOutOfRangeException_WhenItemIdParameterIsZero()
        {
            // Arrange
            var invalidItemId = 0;
            var promotionService = new PromotionService(this.dataMock.Object);

            // Act & Assert
            promotionService.RemovePromotion(invalidItemId);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void RemovePromotion_ShouldThrowArgumentOutOfRangeException_WhenItemIdParameterIsLessZero()
        {
            // Arrange
            var invalidItemId = -5;
            var promotionService = new PromotionService(this.dataMock.Object);

            // Act & Assert
            promotionService.RemovePromotion(invalidItemId);
        }

        [TestMethod]
        [ExpectedException(typeof(ObjectNotFoundInDatabaseException))]
        public void RemovePromotion_ShouldThrowObjectNotFoundInDatabaseException_WhenNoObjectWithTheGivenIdIsFound()
        {
            // Arrange
            var itemId = 1;
            var discount = 50;
            Item item = null;

            this.itemRepoMock.Setup(r => r.GetById(It.IsAny<int>())).Returns(item);
            this.dataMock.SetupGet(d => d.Items).Returns(this.itemRepoMock.Object);

            var promotionService = new PromotionService(this.dataMock.Object);

            // Act & Assert
            promotionService.AddPromotion(itemId, discount);
        }

        [TestMethod]
        public void RemovePromotion_ShouldCallDataCommit_WhenAllParametersAreValid()
        {
            // Arrange
            var itemId = 5;
            var item = new Item();

            this.itemRepoMock.Setup(r => r.GetById(It.IsAny<int>())).Returns(item);
            this.dataMock.SetupGet(d => d.Items).Returns(this.itemRepoMock.Object);

            var promotionService = new PromotionService(this.dataMock.Object);

            // Act
            promotionService.RemovePromotion(itemId);

            // Assert
            this.dataMock.Verify(d => d.Commit(), Times.Once);
        }

        [TestMethod]
        public void RemovePromotion_ShouldCallDataGetById_WhenAllParametersAreValid()
        {
            // Arrange
            var itemId = 15;
            var item = new Item();

            this.itemRepoMock.Setup(r => r.GetById(It.IsAny<int>())).Returns(item);
            this.dataMock.SetupGet(d => d.Items).Returns(this.itemRepoMock.Object);

            var promotionService = new PromotionService(this.dataMock.Object);

            // Act
            promotionService.RemovePromotion(itemId);

            // Assert
            this.itemRepoMock.Verify(r => r.GetById(itemId), Times.Once);
        }

        public void RemovePromotion_ShouldDecreaseItemDiscountToZero_WhenAllParametersAreValid()
        {
            // Arrange
            var itemId = 5;
            var item = new Item();

            this.itemRepoMock.Setup(r => r.GetById(It.IsAny<int>())).Returns(item);
            this.dataMock.SetupGet(d => d.Items).Returns(this.itemRepoMock.Object);

            var promotionService = new PromotionService(this.dataMock.Object);

            // Act
            promotionService.RemovePromotion(itemId);

            // Assert
            Assert.AreEqual(0, item.Discount);
        }
    }
}
