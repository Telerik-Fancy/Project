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
        public void AddItem_ShouldCallItemRepoAdd_WhenValidItemIsPassed()
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
        public void AddItem_ShouldCallDataCommit_WhenValidItemIsPassed()
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
        public void GetItemById_ShowdThrowArgumentOutOfRangeException_WhenItemIdIsNegative()
        {
            // Arrange
            var invalidItemId = -5;
            var itemService = new ItemService(this.dataMock.Object);

            // Act & Assert
            itemService.GetItemById(invalidItemId);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void GetItemById_ShowdThrowArgumentOutOfRangeException_WhenItemIdIsZero()
        {
            // Arrange
            var invalidItemId = 0;
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
        public void GetItemById_ShowdCallItemRepoGetById_WhenValidItemIdIsPassed_CaseSmallNumberId()
        {
            // Arrange
            var itemId = 1;
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
        public void GetItemById_ShowdCallItemRepoGetById_WhenValidItemIdIsPassed_CaseAverageNumberId()
        {
            // Arrange
            var itemId = 100;
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
        public void GetItemById_ShowdCallItemRepoGetById_WhenValidItemIdIsPassed_CaseLargeNumberId()
        {
            // Arrange
            var itemId = 1000000;
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
        public void CheckUniqueItemCode_ShouldThrowArgumentNullException_WhenNoItemCodeIsPassed()
        {
            // Arrange, Act & Assert 
            var itemService = new ItemService(this.dataMock.Object);
            itemService.CheckUniqueItemCode(null);
        }

        [TestMethod]
        public void CheckUniqueItemCode_ShouldReturnTrue_WhenItemCodeIsUnique_CaseLettersOnlyId()
        {
            // Arrange
            var validItemCode = "asdfhsadfsdf";
            Item item = null;

            this.itemRepoMock.Setup(r => r.GetSingleOrDefault(It.IsAny<Expression<Func<Item, bool>>>())).Returns(item);

            this.dataMock.SetupGet(d => d.Items).Returns(this.itemRepoMock.Object);
            var itemService = new ItemService(this.dataMock.Object);

            // Act
            var result = itemService.CheckUniqueItemCode(validItemCode);

            // Assert
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void CheckUniqueItemCode_ShouldReturnTrue_WhenItemCodeIsUnique_CaseNumbersOnlyId()
        {
            // Arrange
            var validItemCode = "412949234";
            Item item = null;

            this.itemRepoMock.Setup(r => r.GetSingleOrDefault(It.IsAny<Expression<Func<Item, bool>>>())).Returns(item);

            this.dataMock.SetupGet(d => d.Items).Returns(this.itemRepoMock.Object);
            var itemService = new ItemService(this.dataMock.Object);

            // Act
            var result = itemService.CheckUniqueItemCode(validItemCode);

            // Assert
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void CheckUniqueItemCode_ShouldReturnTrue_WhenItemCodeIsUnique_CaseLettersAndNumbersId()
        {
            // Arrange
            var validItemCode = "F8SD7F98SDF";
            Item item = null;

            this.itemRepoMock.Setup(r => r.GetSingleOrDefault(It.IsAny<Expression<Func<Item, bool>>>())).Returns(item);

            this.dataMock.SetupGet(d => d.Items).Returns(this.itemRepoMock.Object);
            var itemService = new ItemService(this.dataMock.Object);

            // Act
            var result = itemService.CheckUniqueItemCode(validItemCode);

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
        public void GetItemsOfTypeCount_ShouldCallItemRepoGetAll_TimesOnce()
        {
            // Arrange
            ItemType itemType = ItemType.Necklace;
            MainColourType colour = MainColourType.Red;
            MainMaterialType material = MainMaterialType.Swarovski;

            var itemCollection = new List<Item>();
            Expression<Func<Item, bool>> ex = i => i.ItemType == itemType;

            this.itemRepoMock.Setup(r => r.GetAll(It.IsAny<Expression<Func<Item, bool>>>())).Returns(itemCollection);
            this.dataMock.SetupGet(d => d.Items).Returns(this.itemRepoMock.Object);

            var itemService = new ItemService(this.dataMock.Object);

            // Act
            var count = itemService.GetItemsOfTypeCount(itemType, colour, material);

            // Assert
            this.itemRepoMock.Verify(r => r.GetAll(It.IsAny<Expression<Func<Item, bool>>>()), Times.Once);
        }

        [TestMethod]
        public void GetAllItemsCount_ShouldCallItemRepoGetAll_TimesOnce()
        {
            // Arrange
            var itemCollection = new List<Item>();
            MainColourType colour = MainColourType.Red;
            MainMaterialType material = MainMaterialType.Swarovski;

            this.itemRepoMock.Setup(r => r.GetAll()).Returns(itemCollection);
            this.dataMock.SetupGet(d => d.Items).Returns(this.itemRepoMock.Object);

            var itemService = new ItemService(this.dataMock.Object);

            // Act
            var count = itemService.GetAllItemsCount(colour, material);

            // Assert
            this.itemRepoMock.Verify(r => r.GetAll(), Times.Once);
        }

        [TestMethod]
        public void GetAllItemsInPromotionCount_ShouldCallItemRepoGetAll_TimesOnce()
        {
            // Arrange
            var itemCollection = new List<Item>();
            MainColourType colour = MainColourType.Red;
            MainMaterialType material = MainMaterialType.Swarovski;

            this.itemRepoMock.Setup(r => r.GetAll(It.IsAny<Expression<Func<Item, bool>>>())).Returns(itemCollection);
            this.dataMock.SetupGet(d => d.Items).Returns(this.itemRepoMock.Object);

            var itemService = new ItemService(this.dataMock.Object);

            // Act
            var count = itemService.GetAllItemsInPromotionCount(colour, material);

            // Assert
            this.itemRepoMock.Verify(r => r.GetAll(It.IsAny<Expression<Func<Item, bool>>>()), Times.Once);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void GetItemsOfType_ShouldThrowArgumentOutOfRangeException_WhenZeroPageNumberIsPassed()
        {
            // Arrange, Act & Assert
            var itemCollection = new List<Item>();
            var invalidPageNumber = 0;
            this.itemRepoMock.Setup(r => r.GetAll(It.IsAny<Expression<Func<Item, bool>>>())).Returns(itemCollection);
            this.dataMock.SetupGet(d => d.Items).Returns(this.itemRepoMock.Object);

            var itemService = new ItemService(this.dataMock.Object);
            itemService.GetItemsOfType(invalidPageNumber, ItemType.Necklace, MainColourType.Black, MainMaterialType.Alloy);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void GetItemsOfType_ShouldThrowArgumentOutOfRangeException_WhenNegativePageNumberIsPassed()
        {
            // Arrange, Act & Assert
            var itemCollection = new List<Item>();
            var invalidPageNumber = -100;
            this.itemRepoMock.Setup(r => r.GetAll(It.IsAny<Expression<Func<Item, bool>>>())).Returns(itemCollection);
            this.dataMock.SetupGet(d => d.Items).Returns(this.itemRepoMock.Object);

            var itemService = new ItemService(this.dataMock.Object);
            itemService.GetItemsOfType(invalidPageNumber, ItemType.Necklace, MainColourType.Black, MainMaterialType.Alloy);
        }

        [TestMethod]
        public void GetItemsOfType_ShouldCallItemRepoGetAll_TimesOnce_WhenPageNumberIsValidSmallNumber()
        {
            // Arrange
            var itemCollection = new List<Item>();
            var validPageNumber = 4;
            this.itemRepoMock.Setup(r => r.GetAll(It.IsAny<Expression<Func<Item, bool>>>())).Returns(itemCollection);
            this.dataMock.SetupGet(d => d.Items).Returns(this.itemRepoMock.Object);

            var itemService = new ItemService(this.dataMock.Object);

            // Act
            var count = itemService.GetItemsOfType(validPageNumber, ItemType.Necklace, MainColourType.Black, MainMaterialType.Alloy);

            // Assert
            this.itemRepoMock.Verify(r => r.GetAll(It.IsAny<Expression<Func<Item, bool>>>()), Times.Once);
        }

        [TestMethod]
        public void GetItemsOfType_ShouldCallItemRepoGetAll_TimesOnce_WhenPageNumberIsValidAverageNumber()
        {
            // Arrange
            var itemCollection = new List<Item>();
            var validPageNumber = 1114;
            this.itemRepoMock.Setup(r => r.GetAll(It.IsAny<Expression<Func<Item, bool>>>())).Returns(itemCollection);
            this.dataMock.SetupGet(d => d.Items).Returns(this.itemRepoMock.Object);

            var itemService = new ItemService(this.dataMock.Object);

            // Act
            var count = itemService.GetItemsOfType(validPageNumber, ItemType.Necklace, MainColourType.Black, MainMaterialType.Alloy);

            // Assert
            this.itemRepoMock.Verify(r => r.GetAll(It.IsAny<Expression<Func<Item, bool>>>()), Times.Once);
        }

        [TestMethod]
        public void GetItemsOfType_ShouldCallItemRepoGetAll_TimesOnce_WhenPageNumberIsValidLargeNumber()
        {
            // Arrange
            var itemCollection = new List<Item>();
            var validPageNumber = 111000;
            this.itemRepoMock.Setup(r => r.GetAll(It.IsAny<Expression<Func<Item, bool>>>())).Returns(itemCollection);
            this.dataMock.SetupGet(d => d.Items).Returns(this.itemRepoMock.Object);

            var itemService = new ItemService(this.dataMock.Object);

            // Act
            var count = itemService.GetItemsOfType(validPageNumber, ItemType.Necklace, MainColourType.Black, MainMaterialType.Alloy);

            // Assert
            this.itemRepoMock.Verify(r => r.GetAll(It.IsAny<Expression<Func<Item, bool>>>()), Times.Once);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void GetItemsInPromotion_ShouldThrowArgumentOutOfRangeException_WhenZeroPageNumberIsPassed()
        {
            // Arrange
            var itemCollection = new List<Item>();
            var invalidPageNumber = 0;
            this.itemRepoMock.Setup(r => r.GetAll(It.IsAny<Expression<Func<Item, bool>>>())).Returns(itemCollection);
            this.dataMock.SetupGet(d => d.Items).Returns(this.itemRepoMock.Object);

            var itemService = new ItemService(this.dataMock.Object);

            // Act & Assert
            itemService.GetItemsInPromotion(invalidPageNumber, MainColourType.Black, MainMaterialType.Alloy);
        }
        
        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void GetItemsInPromotion_ShouldThrowArgumentOutOfRangeException_WhenNegativePageNumberIsPassed()
        {
            // Arrange
            var itemCollection = new List<Item>();
            var invalidPageNumber = -20;
            this.itemRepoMock.Setup(r => r.GetAll(It.IsAny<Expression<Func<Item, bool>>>())).Returns(itemCollection);
            this.dataMock.SetupGet(d => d.Items).Returns(this.itemRepoMock.Object);

            var itemService = new ItemService(this.dataMock.Object);

            // Act & Assert
            itemService.GetItemsInPromotion(invalidPageNumber, MainColourType.Black, MainMaterialType.Alloy);
        }

        [TestMethod]
        public void GetItemsInPromotion_ShouldCallItemRepoGetAllOnce_WhenValidArgumentsArePassed_()
        {
            // Arrange
            var itemCollection = new List<Item>();
            var validPageNumber = 4;

            this.itemRepoMock.Setup(r => r.GetAll(It.IsAny<Expression<Func<Item, bool>>>())).Returns(itemCollection);
            this.dataMock.SetupGet(d => d.Items).Returns(this.itemRepoMock.Object);

            var itemService = new ItemService(this.dataMock.Object);

            // Act
            var count = itemService.GetItemsInPromotion(validPageNumber, MainColourType.Black, MainMaterialType.Alloy);

            // Assert
            this.itemRepoMock.Verify(r => r.GetAll(It.IsAny<Expression<Func<Item, bool>>>()), Times.Once);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void GetNewestItems_ShouldThrowArgumentOutOfRangeException_WhenZeroPageNumberIsPassed()
        {
            // Arrange
            var itemCollection = new List<Item>();
            var invalidPageNumber = 0;
            this.itemRepoMock.Setup(r => r.GetAll(It.IsAny<Expression<Func<Item, bool>>>(),
                                                    It.IsAny<Expression<Func<Item, Item>>>())).Returns(itemCollection);
            this.dataMock.SetupGet(d => d.Items).Returns(this.itemRepoMock.Object);

            var itemService = new ItemService(this.dataMock.Object);

            // Act & Assert
            itemService.GetNewestItems(invalidPageNumber, MainColourType.Black, MainMaterialType.Alloy);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void GetNewestItems_ShouldThrowArgumentOutOfRangeException_WhenNegativePageNumberIsPassed()
        {
            // Arrange
            var itemCollection = new List<Item>();
            var invalidPageNumber = -4;
            this.itemRepoMock.Setup(r => r.GetAll(It.IsAny<Expression<Func<Item, bool>>>(),
                                                    It.IsAny<Expression<Func<Item, Item>>>())).Returns(itemCollection);
            this.dataMock.SetupGet(d => d.Items).Returns(this.itemRepoMock.Object);

            var itemService = new ItemService(this.dataMock.Object);

            // Act & Assert
            itemService.GetNewestItems(invalidPageNumber, MainColourType.Black, MainMaterialType.Alloy);
        }
    }
}
