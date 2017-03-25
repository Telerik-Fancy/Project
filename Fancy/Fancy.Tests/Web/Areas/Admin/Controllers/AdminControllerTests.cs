using System;
using System.Text;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Fancy.Services.Common.Contracts;
using Fancy.Services.Data.Contracts;
using Fancy.Web.WebUtils.Contracts;
using Moq;
using Fancy.Data.Repositories;
using Fancy.Web.Areas.Admin.Controllers;
using System.Web.Mvc;
using Fancy.Web.Areas.Admin.Models;
using Fancy.Common.Constants;
using System.Linq;
using System.Web;
using Fancy.Data.Models.Models;

namespace Fancy.Tests.Web.Areas.Admin.Controllers
{
    [TestClass]
    public class AdminControllerTests
    {
        private Mock<IItemService> itemServiceMock;
        private Mock<IMappingService> mappingServiceMock;
        private Mock<IImageProvider> imageProviderMock;

        public object UserRole { get; private set; }

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
            var adminController = new AdminController(null, this.mappingServiceMock.Object, this.imageProviderMock.Object);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Constructor_ShouldThrowArgumentNullException_WhenMappingServiceIsNull()
        {
            // Arrange, Act & Assert
            var adminController = new AdminController(this.itemServiceMock.Object, null, this.imageProviderMock.Object);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Constructor_ShouldThrowArgumentNullException_WhenImageProviderIsNull()
        {
            // Arrange, Act & Assert
            var adminController = new AdminController(this.itemServiceMock.Object, this.mappingServiceMock.Object, null);
        }

        [TestMethod]
        public void AdminPanel_ShouldReturnViewNotNull()
        {
            //Arange
            var adminController = new AdminController(this.itemServiceMock.Object, this.mappingServiceMock.Object, this.imageProviderMock.Object);

            //Act
            var viewResult = adminController.AdminPanel() as ViewResult;

            //Assert
            Assert.IsNotNull(viewResult);
        }

        [TestMethod]
        public void AddItem_ShouldRedirectToAdminPanel()
        {
            //Arange
            var adminController = new AdminController(this.itemServiceMock.Object, this.mappingServiceMock.Object, this.imageProviderMock.Object);
            var addItemViewModel = new AddItemViewModel();

            //Act
            var redirectResult = adminController.AddItem(addItemViewModel) as RedirectResult;

            //Assert
            Assert.IsNotNull(redirectResult);
            Assert.AreEqual(ServerConstants.AdminPanelRedirectUrl, redirectResult.Url);
        }

        [TestMethod]
        public void AddItem_ShouldHaveAuthorizeAttribute_WithOnlyAdministratorRole()
        {
            // Arrange
            var adminController = new AdminController(this.itemServiceMock.Object, this.mappingServiceMock.Object, this.imageProviderMock.Object);
            var addItemMethod = typeof(AdminController).GetMethod("AddItem");
            
            // Act
            var attribute = addItemMethod.GetCustomAttributes(typeof(AuthorizeAttribute), true).Single() as AuthorizeAttribute;

            // Assert
            Assert.IsNotNull(attribute);
            StringAssert.Contains(UserConstants.AdministratorRole, attribute.Roles);
        }

        [TestMethod]
        public void AddItem_ShouldHavePostAttribute()
        {
            // Arrange
            var adminController = new AdminController(this.itemServiceMock.Object, this.mappingServiceMock.Object, this.imageProviderMock.Object);
            var addItemMethod = typeof(AdminController).GetMethod("AddItem");

            // Act
            var attribute = addItemMethod.GetCustomAttributes(typeof(HttpPostAttribute), true).Single() as HttpPostAttribute;

            // Assert
            Assert.IsNotNull(attribute);
        }

        [TestMethod]
        public void AddItem_ShouldHaveValidateAntiForgeryTokenAttribute()
        {
            // Arrange
            var adminController = new AdminController(this.itemServiceMock.Object, this.mappingServiceMock.Object, this.imageProviderMock.Object);
            var addItemMethod = typeof(AdminController).GetMethod("AddItem");

            // Act
            var attribute = addItemMethod.GetCustomAttributes(typeof(ValidateAntiForgeryTokenAttribute), true).Single() as ValidateAntiForgeryTokenAttribute;

            // Assert
            Assert.IsNotNull(attribute);
        }

        [TestMethod]
        public void AddItem_ShouldSetViewItemProperties_IfModelIsValid()
        {
            // Arrange 
            var adminController = new AdminController(this.itemServiceMock.Object, this.mappingServiceMock.Object, this.imageProviderMock.Object);
            var viewModel = new AddItemViewModel();

            // Act 
            adminController.AddItem(viewModel);

            // Assert
            Assert.AreEqual(viewModel.IsDeleted, false);
            Assert.AreEqual(viewModel.Discount, 0);
        }

        [TestMethod]
        public void AddItem_ShouldCallImageProviderToParseItemImage_IfModelIsValid()
        {
            // Arrange
            string imageStringBase64 = "asdfasdf";
            byte[] array = new byte[5];

            this.itemServiceMock.Setup(i => i.CheckUniqueItemCode(It.IsAny<string>())).Returns(true);
            this.imageProviderMock.Setup(i => i.ConvertByteArrayToImageString(It.IsAny<byte[]>())).Returns(imageStringBase64);
            this.imageProviderMock.Setup(i => i.ConvertFileToByteArray(It.IsAny<HttpPostedFileBase>())).Returns(array);

            var adminController = new AdminController(this.itemServiceMock.Object, this.mappingServiceMock.Object, this.imageProviderMock.Object);
            var viewModel = new AddItemViewModel();

            // Act 
            adminController.AddItem(viewModel);

            // Assert
            this.imageProviderMock.Verify(i => i.ConvertFileToByteArray(It.IsAny<HttpPostedFileBase>()), Times.Once);
            this.imageProviderMock.Verify(i => i.ConvertByteArrayToImageString(It.IsAny<byte[]>()), Times.Once);
        }

        [TestMethod]
        public void AddItem_ShouldCallMappingServiceMap_TimesOnce_IfModelIsValid()
        {
            // Arrange
            string imageStringBase64 = "asdfasdf";
            byte[] array = new byte[5];
            var viewModel = new AddItemViewModel();
            var dbModel = new Item();

            this.itemServiceMock.Setup(i => i.CheckUniqueItemCode(It.IsAny<string>())).Returns(true);
            this.imageProviderMock.Setup(i => i.ConvertByteArrayToImageString(It.IsAny<byte[]>())).Returns(imageStringBase64);
            this.imageProviderMock.Setup(i => i.ConvertFileToByteArray(It.IsAny<HttpPostedFileBase>())).Returns(array);
            this.mappingServiceMock.Setup(m => m.Map<AddItemViewModel, Item>(viewModel)).Returns(dbModel);
            this.itemServiceMock.Setup(i => i.AddItem(dbModel));

            var adminController = new AdminController(this.itemServiceMock.Object, this.mappingServiceMock.Object, this.imageProviderMock.Object);

            // Act 
            adminController.AddItem(viewModel);

            // Assert
            this.mappingServiceMock.Verify(m => m.Map<AddItemViewModel, Item>(viewModel), Times.Once);
        }

        [TestMethod]
        public void AddItem_ShouldCallItemService_AddItem_TimesOnce_IfModelIsValid()
        {
            // Arrange
            string imageStringBase64 = "asdfasdf";
            byte[] array = new byte[5];
            var viewModel = new AddItemViewModel();
            var dbModel = new Item();

            this.itemServiceMock.Setup(i => i.CheckUniqueItemCode(It.IsAny<string>())).Returns(true);
            this.imageProviderMock.Setup(i => i.ConvertByteArrayToImageString(It.IsAny<byte[]>())).Returns(imageStringBase64);
            this.imageProviderMock.Setup(i => i.ConvertFileToByteArray(It.IsAny<HttpPostedFileBase>())).Returns(array);
            this.mappingServiceMock.Setup(m => m.Map<AddItemViewModel, Item>(viewModel)).Returns(dbModel);
            this.itemServiceMock.Setup(i => i.AddItem(dbModel));

            var adminController = new AdminController(this.itemServiceMock.Object, this.mappingServiceMock.Object, this.imageProviderMock.Object);

            // Act 
            adminController.AddItem(viewModel);

            // Assert
            this.itemServiceMock.Verify(i => i.AddItem(dbModel), Times.Once);
        }

        [TestMethod]
        public void AddItem_ShouldCallItemService_CheckUniqueItemCode_TimesOnce()
        {
            // Arrange
            string imageStringBase64 = "asdfasdf";
            byte[] array = new byte[5];
            var viewModel = new AddItemViewModel();
            var dbModel = new Item();

            this.itemServiceMock.Setup(i => i.CheckUniqueItemCode(It.IsAny<string>())).Returns(true);
            this.imageProviderMock.Setup(i => i.ConvertByteArrayToImageString(It.IsAny<byte[]>())).Returns(imageStringBase64);
            this.imageProviderMock.Setup(i => i.ConvertFileToByteArray(It.IsAny<HttpPostedFileBase>())).Returns(array);
            this.mappingServiceMock.Setup(m => m.Map<AddItemViewModel, Item>(viewModel)).Returns(dbModel);
            this.itemServiceMock.Setup(i => i.AddItem(dbModel));

            var adminController = new AdminController(this.itemServiceMock.Object, this.mappingServiceMock.Object, this.imageProviderMock.Object);

            // Act 
            adminController.AddItem(viewModel);

            // Assert
            this.itemServiceMock.Verify(i => i.CheckUniqueItemCode(It.IsAny<string>()), Times.Once);
        }
    }
}
