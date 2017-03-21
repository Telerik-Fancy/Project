using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Fancy.Data.Repositories;
using Moq;
using Fancy.Data.Data;
using Fancy.Data.Models.Models;
using Fancy.Services.Data;
using Fancy.Common.Exceptions;
using System.Linq.Expressions;

namespace Fancy.Tests.Services.Data
{
    [TestClass]
    public class OrderServiceTests
    {
        private Mock<IEfFancyData> dataMock;
        private Mock<IEfGenericRepository<Item>> itemRepoMock;
        private Mock<IEfGenericRepository<Order>> orderRepoMock;
        private Mock<IEfGenericRepository<User>> userRepoMock;

        [TestInitialize()]
        public void Initialize()
        {
            this.dataMock = new Mock<IEfFancyData>();
            this.itemRepoMock = new Mock<IEfGenericRepository<Item>>();
            this.orderRepoMock = new Mock<IEfGenericRepository<Order>>();
            this.userRepoMock = new Mock<IEfGenericRepository<User>>();
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Constructor_ShouldThrowArgumentNullException_WhenDataParameterIsNull()
        {
            // Arrange & Act & Assert
            var promotionService = new OrderService(null);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void GetOrderInBasket_ShouldThrowArgumentNullException_WhenUserIdParameterIsNull()
        {
            // Arrange
            var promotionService = new OrderService(this.dataMock.Object);

            // Act & Assert
            promotionService.GetOrderInBasket(null);
        }

        [TestMethod]
        [ExpectedException(typeof(ObjectNotFoundInDatabaseException))]
        public void GetOrderInBasket_ShouldThrowObjectNotFoundInDatabaseException_WhenNoUserWithTheGivenIdIsFound()
        {
            // Arrange
            string userId = "randomUserId";
            User user = null;

            this.userRepoMock.Setup(r => r.GetById(It.IsAny<string>())).Returns(user);
            this.dataMock.SetupGet(d => d.Users).Returns(this.userRepoMock.Object);

            var orderService = new OrderService(this.dataMock.Object);

            // Act & Assert
            orderService.GetOrderInBasket(userId);
        }

        [TestMethod]
        public void GetOrderInBasket_ShouldCreateNewOrderAndCallDataCommit_WhenParametersAreValidAndOrderIsNotCreated()
        {
            // Arrange
            string userId = "randomUserId";
            User user = new User();
            Order order = null;

            this.userRepoMock.Setup(r => r.GetById(It.IsAny<string>())).Returns(user);
            this.orderRepoMock.Setup(r => r.GetSingleOrDefault(It.IsAny<Expression<Func<Order, bool>>>())).Returns(order);
            this.dataMock.SetupGet(d => d.Users).Returns(this.userRepoMock.Object);
            this.dataMock.SetupGet(d => d.Orders).Returns(this.orderRepoMock.Object);

            var orderService = new OrderService(this.dataMock.Object);

            // Act
            orderService.GetOrderInBasket(userId);

            // Assert
            this.dataMock.Verify(d => d.Commit(), Times.Once);
        }

        [TestMethod]
        public void GetOrderInBasket_ShouldCreateNewOrderAndCallOrderRepoAdd_WhenParametersAreValidAndOrderIsNotCreated()
        {
            // Arrange
            string userId = "randomUserId";
            User user = new User();
            Order order = null;

            this.userRepoMock.Setup(r => r.GetById(It.IsAny<string>())).Returns(user);
            this.orderRepoMock.Setup(r => r.GetSingleOrDefault(It.IsAny<Expression<Func<Order, bool>>>())).Returns(order);
            this.dataMock.SetupGet(d => d.Users).Returns(this.userRepoMock.Object);
            this.dataMock.SetupGet(d => d.Orders).Returns(this.orderRepoMock.Object);

            var orderService = new OrderService(this.dataMock.Object);

            // Act
            orderService.GetOrderInBasket(userId);

            // Assert
            this.orderRepoMock.Verify(r => r.Add(It.IsNotNull<Order>()), Times.Once);
        }

        [TestMethod]
        public void GetOrderInBasket_ShouldCreateNewOrderAndCallOrderGetSingleOrDefault_WhenParametersAreValidAndOrderIsNotCreated()
        {
            // Arrange
            string userId = "randomUserId";
            User user = new User();
            Order order = null;

            this.userRepoMock.Setup(r => r.GetById(It.IsAny<string>())).Returns(user);
            this.orderRepoMock.Setup(r => r.GetSingleOrDefault(It.IsAny<Expression<Func<Order, bool>>>())).Returns(order);
            this.dataMock.SetupGet(d => d.Users).Returns(this.userRepoMock.Object);
            this.dataMock.SetupGet(d => d.Orders).Returns(this.orderRepoMock.Object);

            var orderService = new OrderService(this.dataMock.Object);

            // Act
            orderService.GetOrderInBasket(userId);

            // Assert
            this.orderRepoMock.Verify(r => r.GetSingleOrDefault(It.IsAny<Expression<Func<Order, bool>>>()), Times.Once);
        }

        [TestMethod]
        public void GetOrderInBasket_ShouldCreateNewOrderAndCallUserGetById_WhenParametersAreValidAndOrderIsNotCreated()
        {
            // Arrange
            string userId = "randomUserId";
            User user = new User();
            Order order = null;

            this.userRepoMock.Setup(r => r.GetById(It.IsAny<string>())).Returns(user);
            this.orderRepoMock.Setup(r => r.GetSingleOrDefault(It.IsAny<Expression<Func<Order, bool>>>())).Returns(order);
            this.dataMock.SetupGet(d => d.Users).Returns(this.userRepoMock.Object);
            this.dataMock.SetupGet(d => d.Orders).Returns(this.orderRepoMock.Object);

            var orderService = new OrderService(this.dataMock.Object);

            // Act
            orderService.GetOrderInBasket(userId);

            // Assert
            this.userRepoMock.Verify(r => r.GetById(It.IsAny<string>()), Times.Once);
        }
    }
}
