using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Fancy.Services.Data.Contracts;
using Fancy.Services.Common.Contracts;
using Fancy.Web.WebUtils.Contracts;
using Fancy.Web.Areas.Items.Controllers;
using System.Web.Mvc;
using Fancy.Web.Areas.Items.Models;
using Fancy.Common.Enums;
using System.Collections.Generic;
using Fancy.Data.Models.Models;
using System.Linq;
using Fancy.Common.Constants;

namespace Fancy.Tests.Web.Areas.Items.Controllers
{
    [TestClass]
    public class ItemsControllerTests
    {
        private Mock<IItemService> itemServiceMock;
        private Mock<IMappingService> mappingServiceMock;
        private Mock<IImageProvider> imageProviderMock;

        [TestInitialize()]
        public void Initialize()
        {
            this.itemServiceMock = new Mock<IItemService>();
            this.mappingServiceMock = new Mock<IMappingService>();
            this.imageProviderMock = new Mock<IImageProvider>();
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Constructor_ShouldThrowArgumentNullException_WhenItemServiceIsNull()
        {
            // Arrange, Act & Assert
            var itemsController = new ItemsController(null, this.mappingServiceMock.Object, this.imageProviderMock.Object);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Constructor_ShouldThrowArgumentNullException_WhenMappingServiceIsNull()
        {
            // Arrange, Act & Assert
            var itemsController = new ItemsController(this.itemServiceMock.Object, null, this.imageProviderMock.Object);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Constructor_ShouldThrowArgumentNullException_WhenImageProviderIsNull()
        {
            // Arrange, Act & Assert
            var itemsController = new ItemsController(this.itemServiceMock.Object, this.mappingServiceMock.Object, null);
        }

        [TestMethod]
        public void GalleryItems_ShouldReturnViewNotNull()
        {
            //Arange
            var itemsController = new ItemsController(this.itemServiceMock.Object, this.mappingServiceMock.Object, this.imageProviderMock.Object);
            GalleryItemsViewModel model = new GalleryItemsViewModel();
            int pageNumber = 1;
            string type = "Necklace";

            //Act
            var viewResult = itemsController.GalleryItems (model, pageNumber, type) as ViewResult;

            //Assert
            Assert.IsNotNull(viewResult);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void GalleryItems_ShouldThrowArgumentOutOfRangeException_WhenNegativePageNumberIsPassed()
        {
            //Arange
            var itemsController = new ItemsController(this.itemServiceMock.Object, this.mappingServiceMock.Object, this.imageProviderMock.Object);
            GalleryItemsViewModel model = new GalleryItemsViewModel();
            int invalidPageNumber = -91;
            string type = "Necklace";

            //Act & Assert
            var viewResult = itemsController.GalleryItems(model, invalidPageNumber, type) as ViewResult;
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void GalleryItems_ShouldThrowArgumentOutOfRangeException_WhenZeroPageNumberIsPassed()
        {
            //Arange
            var itemsController = new ItemsController(this.itemServiceMock.Object, this.mappingServiceMock.Object, this.imageProviderMock.Object);
            GalleryItemsViewModel model = new GalleryItemsViewModel();
            int invalidPageNumber = 0;
            string type = "Necklace";

            //Act & Assert
            var viewResult = itemsController.GalleryItems(model, invalidPageNumber, type) as ViewResult;
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void GalleryItems_ShouldThrowArgumentNullException_WhenViewModelIsNull()
        {
            //Arange
            var itemsController = new ItemsController(this.itemServiceMock.Object, this.mappingServiceMock.Object, this.imageProviderMock.Object);
            GalleryItemsViewModel model = null;
            int pageNumber = 10;
            string type = "Necklace";

            //Act & Assert
            var viewResult = itemsController.GalleryItems(model, pageNumber, type) as ViewResult;
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void GalleryItems_ShouldThrowArgumentNullException_WhenTypeIsNull()
        {
            //Arange
            var itemsController = new ItemsController(this.itemServiceMock.Object, this.mappingServiceMock.Object, this.imageProviderMock.Object);
            GalleryItemsViewModel model = new GalleryItemsViewModel();
            int pageNumber = 10;
            string type = null;

            //Act & Assert
            var viewResult = itemsController.GalleryItems(model, pageNumber, type) as ViewResult;
        }

        [TestMethod]
        public void GalleryItems_ShouldCallIteServiceGetItemsOfTypeCount_TimesOnce_WhenAllParametersAreValid()
        {
            //Arange
            GalleryItemsViewModel model = new GalleryItemsViewModel();
            int pageNumber = 10;
            string type = "Necklace";
            var items = new List<Item>();
            var colour = MainColourType.Red;
            var material = MainMaterialType.Swarovski;
            model.Colour = colour;
            model.Material = material;

            this.itemServiceMock.Setup(i => i.GetItemsOfTypeCount(It.IsAny<ItemType>(), colour, material)).Returns(It.Is<int>(c => c >= 0));
            this.itemServiceMock.Setup(i => i.GetItemsOfType(
                pageNumber,
                It.Is<ItemType>(t => t.ToString() == type),
                It.IsAny<MainColourType>(),
                It.IsAny<MainMaterialType>())).Returns(items);

            var itemsController = new ItemsController(this.itemServiceMock.Object, this.mappingServiceMock.Object, this.imageProviderMock.Object);
            
            // Act
            var viewResult = itemsController.GalleryItems(model, pageNumber, type) as ViewResult;

            // Assert
            this.itemServiceMock.Verify(i => i.GetItemsOfTypeCount(It.IsAny<ItemType>(), colour, material), Times.Once);
        }

        [TestMethod]
        public void GalleryItems_ShouldCallIteServiceGetItemsOfType_TimesOnce_WhenTypeIsNull()
        {
            //Arange
            GalleryItemsViewModel model = new GalleryItemsViewModel();
            int pageNumber = 10;
            string type = "Necklace";
            var items = new List<Item>();
            var colour = MainColourType.Red;
            var material = MainMaterialType.Swarovski;

            this.itemServiceMock.Setup(i => i.GetItemsOfTypeCount(It.IsAny<ItemType>(), colour, material)).Returns(It.Is<int>(c => c >= 0));
            this.itemServiceMock.Setup(i => i.GetItemsOfType(
                pageNumber,
                It.Is<ItemType>(t => t.ToString() == type),
                It.IsAny<MainColourType>(),
                It.IsAny<MainMaterialType>())).Returns(items);

            var itemsController = new ItemsController(this.itemServiceMock.Object, this.mappingServiceMock.Object, this.imageProviderMock.Object);

            // Act
            var viewResult = itemsController.GalleryItems(model, pageNumber, type) as ViewResult;

            // Assert
            this.itemServiceMock.Verify(i => i.GetItemsOfType(
                pageNumber,
                It.Is<ItemType>(t => t.ToString() == type),
                It.IsAny<MainColourType>(),
                It.IsAny<MainMaterialType>()), Times.Once);
        }

        [TestMethod]
        public void GalleryItemsNew_ShouldReturnViewNotNull()
        {
            //Arange
            var itemsController = new ItemsController(this.itemServiceMock.Object, this.mappingServiceMock.Object, this.imageProviderMock.Object);
            GalleryItemsViewModel model = new GalleryItemsViewModel();
            int pageNumber = 1;

            //Act
            var viewResult = itemsController.GalleryItemsNew(model, pageNumber) as ViewResult;

            //Assert
            Assert.IsNotNull(viewResult);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void GalleryItemsNew_ShouldThrowArgumentOutOfRangeException_WhenNegativePageNumberIsPassed()
        {
            //Arange
            var itemsController = new ItemsController(this.itemServiceMock.Object, this.mappingServiceMock.Object, this.imageProviderMock.Object);
            GalleryItemsViewModel model = new GalleryItemsViewModel();
            int invalidPageNumber = -91;

            //Act & Assert
            var viewResult = itemsController.GalleryItemsNew(model, invalidPageNumber) as ViewResult;
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void GalleryItemsNew_ShouldThrowArgumentOutOfRangeException_WhenZeroPageNumberIsPassed()
        {
            //Arange
            var itemsController = new ItemsController(this.itemServiceMock.Object, this.mappingServiceMock.Object, this.imageProviderMock.Object);
            GalleryItemsViewModel model = new GalleryItemsViewModel();
            int invalidPageNumber = 0;

            //Act & Assert
            var viewResult = itemsController.GalleryItemsNew(model, invalidPageNumber) as ViewResult;
        }

        [TestMethod]
        public void GalleryItemsNew_ShouldCallIteServiceGetAllItemsCount_WhenAllParametersAreValid()
        {
            //Arange
            GalleryItemsViewModel model = new GalleryItemsViewModel();
            int pageNumber = 10;
            var items = new List<Item>();
            var colour = MainColourType.Red;
            var material = MainMaterialType.Swarovski;
            model.Colour = colour;
            model.Material = material;

            this.itemServiceMock.Setup(i => i.GetAllItemsCount(colour, material)).Returns(It.Is<int>(c => c >= 0));
            this.itemServiceMock.Setup(i => i.GetNewestItems(
                pageNumber,
                It.IsAny<MainColourType>(),
                It.IsAny<MainMaterialType>())).Returns(items);

            var itemsController = new ItemsController(this.itemServiceMock.Object, this.mappingServiceMock.Object, this.imageProviderMock.Object);

            // Act
            var viewResult = itemsController.GalleryItemsNew(model, pageNumber) as ViewResult;

            // Assert
            this.itemServiceMock.Verify(i => i.GetAllItemsCount(colour, material), Times.Once);
        }

        [TestMethod]
        public void GalleryItemsNew_ShouldCallIteServiceGetNewestItems_WhenAllParametersAreValid()
        {
            //Arange
            GalleryItemsViewModel model = new GalleryItemsViewModel();
            int pageNumber = 10;
            var items = new List<Item>();
            var colour = MainColourType.Red;
            var material = MainMaterialType.Swarovski;

            this.itemServiceMock.Setup(i => i.GetAllItemsCount(colour, material)).Returns(It.Is<int>(c => c >= 0));
            this.itemServiceMock.Setup(i => i.GetNewestItems(
                pageNumber,
                It.IsAny<MainColourType>(),
                It.IsAny<MainMaterialType>())).Returns(items);

            var itemsController = new ItemsController(this.itemServiceMock.Object, this.mappingServiceMock.Object, this.imageProviderMock.Object);

            // Act
            var viewResult = itemsController.GalleryItemsNew(model, pageNumber) as ViewResult;

            // Assert
            this.itemServiceMock.Verify(i => i.GetNewestItems(
                pageNumber,
                It.IsAny<MainColourType>(),
                It.IsAny<MainMaterialType>()), Times.Once);
        }

        [TestMethod]
        public void GalleryItemsPromotions_ShouldReturnViewNotNull()
        {
            //Arange
            var itemsController = new ItemsController(this.itemServiceMock.Object, this.mappingServiceMock.Object, this.imageProviderMock.Object);
            GalleryItemsViewModel model = new GalleryItemsViewModel();
            int pageNumber = 1;

            //Act
            var viewResult = itemsController.GalleryItemsPromotions(model, pageNumber) as ViewResult;

            //Assert
            Assert.IsNotNull(viewResult);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void GalleryItemsPromotions_ShouldThrowArgumentOutOfRangeException_WhenNegativePageNumberIsPassed()
        {
            //Arange
            var itemsController = new ItemsController(this.itemServiceMock.Object, this.mappingServiceMock.Object, this.imageProviderMock.Object);
            GalleryItemsViewModel model = new GalleryItemsViewModel();
            int invalidPageNumber = -91;

            //Act & Assert
            var viewResult = itemsController.GalleryItemsPromotions(model, invalidPageNumber) as ViewResult;
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void GalleryItemsPromotions_ShouldThrowArgumentOutOfRangeException_WhenZeroPageNumberIsPassed()
        {
            //Arange
            var itemsController = new ItemsController(this.itemServiceMock.Object, this.mappingServiceMock.Object, this.imageProviderMock.Object);
            GalleryItemsViewModel model = new GalleryItemsViewModel();
            int invalidPageNumber = 0;

            //Act & Assert
            var viewResult = itemsController.GalleryItemsPromotions(model, invalidPageNumber) as ViewResult;
        }

        [TestMethod]
        public void GalleryItemsPromotions_ShouldCallGetAllItemsInPromotionCount_WhenAllParametersAreValid()
        {
            //Arange
            GalleryItemsViewModel model = new GalleryItemsViewModel();
            int pageNumber = 10;
            var items = new List<Item>();
            var colour = MainColourType.Red;
            var material = MainMaterialType.Swarovski;
            model.Colour = colour;
            model.Material = material;

            this.itemServiceMock.Setup(i => i.GetAllItemsInPromotionCount(colour, material)).Returns(It.Is<int>(c => c >= 0));
            this.itemServiceMock.Setup(i => i.GetItemsInPromotion(
                pageNumber,
                It.IsAny<MainColourType>(),
                It.IsAny<MainMaterialType>())).Returns(items);

            var itemsController = new ItemsController(this.itemServiceMock.Object, this.mappingServiceMock.Object, this.imageProviderMock.Object);

            // Act
            var viewResult = itemsController.GalleryItemsPromotions(model, pageNumber) as ViewResult;

            // Assert
            this.itemServiceMock.Verify(i => i.GetAllItemsInPromotionCount(colour, material), Times.Once);
        }

        [TestMethod]
        public void GalleryItemsPromotions_ShouldCallIteServiceGetItemsInPromotion_WhenAllParametersAreValid()
        {
            //Arange
            GalleryItemsViewModel model = new GalleryItemsViewModel();
            int pageNumber = 10;
            var items = new List<Item>();
            var colour = MainColourType.Red;
            var material = MainMaterialType.Swarovski;

            this.itemServiceMock.Setup(i => i.GetAllItemsInPromotionCount(colour, material)).Returns(It.Is<int>(c => c >= 0));
            this.itemServiceMock.Setup(i => i.GetItemsInPromotion(
                pageNumber,
                It.IsAny<MainColourType>(),
                It.IsAny<MainMaterialType>())).Returns(items);

            var itemsController = new ItemsController(this.itemServiceMock.Object, this.mappingServiceMock.Object, this.imageProviderMock.Object);

            // Act
            var viewResult = itemsController.GalleryItemsPromotions(model, pageNumber) as ViewResult;

            // Assert
            this.itemServiceMock.Verify(i => i.GetItemsInPromotion(
                pageNumber,
                It.IsAny<MainColourType>(),
                It.IsAny<MainMaterialType>()), Times.Once);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void SingleItem_ShouldThrowArgumentOutOfRangeException_WhenZeroItemIdIsPassed()
        {
            //Arange
            var itemsController = new ItemsController(this.itemServiceMock.Object, this.mappingServiceMock.Object, this.imageProviderMock.Object);

            GalleryItemsViewModel model = new GalleryItemsViewModel();
            int invalidItemId = 0;

            //Act & Assert
            var viewResult = itemsController.GalleryItemsNew(model, invalidItemId) as ViewResult;
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void SingleItem_ShouldThrowArgumentOutOfRangeException_WhenNegativeItemIdIsPassed()
        {
            //Arange
            var itemsController = new ItemsController(this.itemServiceMock.Object, this.mappingServiceMock.Object, this.imageProviderMock.Object);

            GalleryItemsViewModel model = new GalleryItemsViewModel();
            int invalidItemId = -10;

            //Act & Assert
            var viewResult = itemsController.GalleryItemsNew(model, invalidItemId) as ViewResult;
        }

        [TestMethod]
        public void SingleItem_ShouldHaveAuthorizeAttribute_WithAdministratorOrRegularRole()
        {
            // Arrange
            var adminController = new ItemsController(this.itemServiceMock.Object, this.mappingServiceMock.Object, this.imageProviderMock.Object);
            var singleItemMethod = typeof(ItemsController).GetMethod("SingleItem");

            // Act
            var attribute = singleItemMethod.GetCustomAttributes(typeof(AuthorizeAttribute), true).Single() as AuthorizeAttribute;

            // Assert
            Assert.IsNotNull(attribute);
            StringAssert.Contains(UserConstants.AdministratorOrRegular, attribute.Roles);
        }

        [TestMethod]
        public void SingleItem_ShouldReturnViewNotNull()
        {
            //Arange
            var itemsController = new ItemsController(this.itemServiceMock.Object, this.mappingServiceMock.Object, this.imageProviderMock.Object);
            int itemId = 1;
            SingleItemViewModel model = new SingleItemViewModel();

            //Act
            var viewResult = itemsController.SingleItem(model, itemId) as ViewResult;

            //Assert
            Assert.IsNotNull(viewResult);
        }
    }
}
