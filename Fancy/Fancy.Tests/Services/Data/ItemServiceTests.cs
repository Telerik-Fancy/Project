using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Fancy.Data.Repositories;
using Fancy.Data.Models.Models;
using Fancy.Data.Data;
using Fancy.Services.Data;
using Fancy.Common.Exceptions;
using System.Linq.Expressions;
using Fancy.Common.Enums;
using System.Collections.Generic;

namespace Fancy.Tests.Services.Data
{
    [TestClass]
    public class ItemServiceTests
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
        public void Constructor_ShowdThrowArgumenNullException_WhenDataParameterIsNull()
        {
            // Arrange, Act, Assert
            var itemService = new ItemService(null);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void AddItem_ShowdThrowArgumenNullException_WhenItemIsNull()
        {
            // Arrange
            var itemService = new ItemService(this.dataMock.Object);

            // Act & Assert
            itemService.AddItem(null);
        }

        [TestMethod]
        public void AddItem_ShowdCallItemRepoAdd_WhenValidItemIsPassed()
        {
            // Arrange
            var item = new Item();

            this.dataMock.SetupGet(d => d.Items).Returns(this.itemRepoMock.Object);

            var itemService = new ItemService(this.dataMock.Object);

            // Act
            itemService.AddItem(item);

            // Assert
            this.itemRepoMock.Verify(r => r.Add(item), Times.Once);
        }

        [TestMethod]
        public void AddItem_ShowdCallDataCommit_WhenValidItemIsPassed()
        {
            // Arrange
            var item = new Item();

            this.dataMock.SetupGet(d => d.Items).Returns(this.itemRepoMock.Object);

            var itemService = new ItemService(this.dataMock.Object);

            // Act
            itemService.AddItem(item);

            // Assert
            this.dataMock.Verify(d => d.Commit(), Times.Once);

        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void GetItemById_ShowdThrowArgumentOutOfRangeException_WhenItemIdIsInvalid()
        {
            // Arrange
            var invalidItemId = -5;
            var itemService = new ItemService(this.dataMock.Object);

            // Act & Assert
            itemService.GetItemById(invalidItemId);
        }

        [TestMethod]
        [ExpectedException(typeof(ObjectNotFoundInDatabaseException))]
        public void GetItemById_ShowdThrowObjectNotFoundInDatabaseException_WhenNoItemWithGivenIdIsFound()
        {
            // Arrange
            var itemId = 5;
            Item item = null;

            this.itemRepoMock.Setup(r => r.GetById(itemId)).Returns(item);
            this.dataMock.SetupGet(d => d.Items).Returns(this.itemRepoMock.Object);

            var itemService = new ItemService(this.dataMock.Object);

            // Act & Assert
            itemService.GetItemById(itemId);
        }

        [TestMethod]
        public void GetItemById_ShowdCallItemRepoGetById_WhenValidItemIdIsPassed()
        {
            // Arrange
            var itemId = 132;
            var item = new Item();

            this.itemRepoMock.Setup(r => r.GetById(itemId)).Returns(item);
            this.dataMock.SetupGet(d => d.Items).Returns(this.itemRepoMock.Object);

            var itemService = new ItemService(this.dataMock.Object);

            // Act
            itemService.GetItemById(itemId);

            // Assert
            this.itemRepoMock.Verify(r => r.GetById(itemId), Times.Once);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void CheckUniqueItemCode_ShouldThrowArgumentNullException_WhenInvalidItemCodeIsPassed()
        {
            // Arrange, Act & Assert 
            var itemService = new ItemService(this.dataMock.Object);
            itemService.CheckUniqueItemCode(null);
        }

        [TestMethod]
        public void CheckUniqueItemCode_ShouldReturnTrue_WhenItemCodeIsUnique()
        {
            // Arrange
            var itemCode = "someCode111";
            Item item = null;

            this.itemRepoMock.Setup(r => r.GetSingleOrDefault(It.IsAny<Expression<Func<Item, bool>>>())).Returns(item);

            this.dataMock.SetupGet(d => d.Items).Returns(this.itemRepoMock.Object);
            var itemService = new ItemService(this.dataMock.Object);

            // Act
            var result = itemService.CheckUniqueItemCode(itemCode);

            // Assert
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void CheckUniqueItemCode_ShouldReturnFalse_WhenItemCodeIsNotUnique()
        {
            // Arrange
            var itemCode = "someCode111";
            var item = new Item();

            this.itemRepoMock.Setup(r => r.GetSingleOrDefault(It.IsAny<Expression<Func<Item, bool>>>())).Returns(item);
            this.dataMock.SetupGet(d => d.Items).Returns(this.itemRepoMock.Object);

            var itemService = new ItemService(this.dataMock.Object);

            // Act
            var result = itemService.CheckUniqueItemCode(itemCode);

            // Assert
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void GetItemsOfTypeCount_ShouldCallItemRepoGetAllOnce_WhenValidArgumentsArePassed()
        {
            // Arrange
            var itemType = ItemType.Necklace;
            var itemCollection = new List<Item>();

            this.itemRepoMock.Setup(r => r.GetAll(It.IsAny<Expression<Func<Item, bool>>>())).Returns(itemCollection);
            this.dataMock.SetupGet(d => d.Items).Returns(this.itemRepoMock.Object);

            var itemService = new ItemService(this.dataMock.Object);

            // Act
            var count = itemService.GetItemsOfTypeCount(itemType);

            // Assert
            this.itemRepoMock.Verify(r => r.GetAll(It.IsAny<Expression<Func<Item, bool>>>()), Times.Once);
        }

        [TestMethod]
        public void GetAllItemsCount_ShouldCallItemRepoGetAllOnce_WhenValidArgumentsArePassed()
        {
            // Arrange
            var itemCollection = new List<Item>();

            this.itemRepoMock.Setup(r => r.GetAll()).Returns(itemCollection);
            this.dataMock.SetupGet(d => d.Items).Returns(this.itemRepoMock.Object);

            var itemService = new ItemService(this.dataMock.Object);

            // Act
            var count = itemService.GetAllItemsCount();

            // Assert
            this.itemRepoMock.Verify(r => r.GetAll(), Times.Once);
        }

        [TestMethod]
        public void GetAllItemsInPromotionCount_ShouldCallItemRepoGetAllOnce_WhenValidArgumentsArePassed()
        {
            // Arrange
            var itemCollection = new List<Item>();

            this.itemRepoMock.Setup(r => r.GetAll(It.IsAny<Expression<Func<Item, bool>>>())).Returns(itemCollection);
            this.dataMock.SetupGet(d => d.Items).Returns(this.itemRepoMock.Object);

            var itemService = new ItemService(this.dataMock.Object);

            // Act
            var count = itemService.GetAllItemsInPromotionCount();

            // Assert
            this.itemRepoMock.Verify(r => r.GetAll(It.IsAny<Expression<Func<Item, bool>>>()), Times.Once);
        }
    }
}
