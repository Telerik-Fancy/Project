using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Fancy.Data.Repositories;
using Moq;
using Fancy.Data.Data;
using Fancy.Data.Models.Models;
using Fancy.Services.Data;
using Fancy.Common.Exceptions;
using System.Linq.Expressions;
using Fancy.Common.Enums;
using System.Collections.Generic;

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
            var orderService = new OrderService(this.dataMock.Object);

            // Act & Assert
            orderService.GetOrderInBasket(null);
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

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void AddItemToBasket_ShouldThrowArgumentOutOfRangeException_WhenItemIdIsInvalid()
        {
            // Arange 
            var invalidItemId = -4;
            var validUserId = "1231231";

            var orderService = new OrderService(this.dataMock.Object);

            // Act & Assert
            orderService.AddItemToBasket(invalidItemId, validUserId);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void AddItemToBasket_ShouldThrowArgumentNullException_WhenUserIdIsInvalid()
        {
            // Arange 
            var validItemId = 10;
            string invalidUserId = null;

            var orderService = new OrderService(this.dataMock.Object);

            // Act & Assert
            orderService.AddItemToBasket(validItemId, invalidUserId);
        }

        [TestMethod]
        [ExpectedException(typeof(ObjectNotFoundInDatabaseException))]
        public void AddItemToBasket_ShouldThrowObjectNotFoundInDatabaseException_WhenNoUserWithGivenIdIsFound()
        {
            // Arange 
            var validItemId = 10;
            var validUserId = "1234";
            
            User user = null;
            Item item = new Item();

            this.userRepoMock.Setup(r => r.GetById(validUserId)).Returns(user);
            this.itemRepoMock.Setup(r => r.GetById(validItemId)).Returns(item);

            this.dataMock.SetupGet(d => d.Users).Returns(this.userRepoMock.Object);
            this.dataMock.SetupGet(d => d.Items).Returns(this.itemRepoMock.Object);

            var orderService = new OrderService(this.dataMock.Object);

            // Act & Assert
            orderService.AddItemToBasket(validItemId, validUserId);
        }

        [TestMethod]
        [ExpectedException(typeof(ObjectNotFoundInDatabaseException))]
        public void AddItemToBasket_ShouldThrowObjectNotFoundInDatabaseException_WhenNoItemWithGivenIdIsFound()
        {
            // Arange 
            var validItemId = 10;
            var validUserId = "1234";

            User user = new User();
            Item item = null;
            Order order = new Order();

            this.userRepoMock.Setup(r => r.GetById(validUserId)).Returns(user);
            this.itemRepoMock.Setup(r => r.GetById(validItemId)).Returns(item);
            this.orderRepoMock.Setup(r => r.GetSingleOrDefault(It.IsAny<Expression<Func<Order, bool>>>())).Returns(order);

            this.dataMock.SetupGet(d => d.Users).Returns(this.userRepoMock.Object);
            this.dataMock.SetupGet(d => d.Items).Returns(this.itemRepoMock.Object);
            this.dataMock.SetupGet(d => d.Orders).Returns(this.orderRepoMock.Object);

            var orderService = new OrderService(this.dataMock.Object);

            // Act & Assert
            orderService.AddItemToBasket(validItemId, validUserId);
        }

        [TestMethod]
        public void AddItemToBasket_ShouldAddItemToBasket_WhenValidArgumentsArePassed()
        {
            // Arange 
            var validItemId = 10;
            var validUserId = "1234";

            User user = new User();
            Item item = new Item();
            Order order = new Order();

            this.userRepoMock.Setup(r => r.GetById(validUserId)).Returns(user);
            this.itemRepoMock.Setup(r => r.GetById(validItemId)).Returns(item);
            this.orderRepoMock.Setup(r => r.GetSingleOrDefault(It.IsAny<Expression<Func<Order, bool>>>())).Returns(order);

            this.dataMock.SetupGet(d => d.Users).Returns(this.userRepoMock.Object);
            this.dataMock.SetupGet(d => d.Items).Returns(this.itemRepoMock.Object);
            this.dataMock.SetupGet(d => d.Orders).Returns(this.orderRepoMock.Object);

            var orderService = new OrderService(this.dataMock.Object);
            var initialCount = order.Items.Count;

            // Act
            orderService.AddItemToBasket(validItemId, validUserId);
            var currentCount = order.Items.Count;

            // Assert
            Assert.AreEqual(initialCount, currentCount - 1);
        }

        [TestMethod]
        public void AddItemToBasket_ShouldReduceItemQuantity_WhenValidArgumentsArePassed()
        {
            // Arange 
            var validItemId = 10;
            var validUserId = "1234";

            User user = new User();
            Item item = new Item();
            Order order = new Order();

            this.userRepoMock.Setup(r => r.GetById(validUserId)).Returns(user);
            this.itemRepoMock.Setup(r => r.GetById(validItemId)).Returns(item);
            this.orderRepoMock.Setup(r => r.GetSingleOrDefault(It.IsAny<Expression<Func<Order, bool>>>())).Returns(order);

            this.dataMock.SetupGet(d => d.Users).Returns(this.userRepoMock.Object);
            this.dataMock.SetupGet(d => d.Items).Returns(this.itemRepoMock.Object);
            this.dataMock.SetupGet(d => d.Orders).Returns(this.orderRepoMock.Object);

            var orderService = new OrderService(this.dataMock.Object);
            var initialItemQuantity = 10;
            item.Quantity = initialItemQuantity;

            // Act
            orderService.AddItemToBasket(validItemId, validUserId);
            var currentItemQuantity = item.Quantity;

            // Assert
            Assert.AreEqual(initialItemQuantity, currentItemQuantity + 1);
        }

        [TestMethod]
        public void AddItemToBasket_ShouldCallDataCommit_WhenValidArgumentsArePassed()
        {
            // Arange 
            var validItemId = 10;
            var validUserId = "1234";

            User user = new User();
            Item item = new Item();
            Order order = new Order();

            this.userRepoMock.Setup(r => r.GetById(validUserId)).Returns(user);
            this.itemRepoMock.Setup(r => r.GetById(validItemId)).Returns(item);
            this.orderRepoMock.Setup(r => r.GetSingleOrDefault(It.IsAny<Expression<Func<Order, bool>>>())).Returns(order);

            this.dataMock.SetupGet(d => d.Users).Returns(this.userRepoMock.Object);
            this.dataMock.SetupGet(d => d.Items).Returns(this.itemRepoMock.Object);
            this.dataMock.SetupGet(d => d.Orders).Returns(this.orderRepoMock.Object);

            var orderService = new OrderService(this.dataMock.Object);

            // Act
            orderService.AddItemToBasket(validItemId, validUserId);

            // Assert
            dataMock.Verify(d => d.Commit(), Times.Once);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void RemoveItemFromBasket_ShouldThrowArgumentOutOfRangeException_WhenItemIdIsInvalid()
        {
            // Arange 
            var invalidItemId = -4;
            var validUserId = "1231231";

            var orderService = new OrderService(this.dataMock.Object);

            // Act & Assert
            orderService.RemoveItemFromBasket(invalidItemId, validUserId);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void RemoveItemFromBasket_ShouldThrowArgumentNullException_WhenUserIdIsInvalid()
        {
            // Arange 
            var validItemId = 10;
            string invalidUserId = null;

            var orderService = new OrderService(this.dataMock.Object);

            // Act & Assert
            orderService.RemoveItemFromBasket(validItemId, invalidUserId);
        }

        [TestMethod]
        [ExpectedException(typeof(ObjectNotFoundInDatabaseException))]
        public void RemoveItemFromBasket_ShouldThrowObjectNotFoundInDatabaseException_WhenNotUserWithGivenIdIsFound()
        {
            // Arange 
            var validItemId = 10;
            var validUserId = "1234";

            User user = null;
            Item item = new Item();
            Order order = new Order();

            this.userRepoMock.Setup(r => r.GetById(validUserId)).Returns(user);
            this.itemRepoMock.Setup(r => r.GetById(validItemId)).Returns(item);
            this.orderRepoMock.Setup(r => r.GetSingleOrDefault(It.IsAny<Expression<Func<Order, bool>>>())).Returns(order);

            this.dataMock.SetupGet(d => d.Users).Returns(this.userRepoMock.Object);
            this.dataMock.SetupGet(d => d.Items).Returns(this.itemRepoMock.Object);
            this.dataMock.SetupGet(d => d.Orders).Returns(this.orderRepoMock.Object);

            var orderService = new OrderService(this.dataMock.Object);

            // Act & Assert
            orderService.RemoveItemFromBasket(validItemId, validUserId);
        }

        [TestMethod]
        [ExpectedException(typeof(ObjectNotFoundInDatabaseException))]
        public void RemoveItemFromBasket_ShouldThrowObjectNotFoundInDatabaseException_WhenNotItemWithGivenIdIsFound()
        {
            // Arange 
            var validItemId = 10;
            var validUserId = "1234";

            User user = new User();
            Item item = null;
            Order order = new Order();

            this.userRepoMock.Setup(r => r.GetById(validUserId)).Returns(user);
            this.itemRepoMock.Setup(r => r.GetById(validItemId)).Returns(item);
            this.orderRepoMock.Setup(r => r.GetSingleOrDefault(It.IsAny<Expression<Func<Order, bool>>>())).Returns(order);

            this.dataMock.SetupGet(d => d.Users).Returns(this.userRepoMock.Object);
            this.dataMock.SetupGet(d => d.Items).Returns(this.itemRepoMock.Object);
            this.dataMock.SetupGet(d => d.Orders).Returns(this.orderRepoMock.Object);

            var orderService = new OrderService(this.dataMock.Object);

            // Act & Assert
            orderService.RemoveItemFromBasket(validItemId, validUserId);
        }

        [TestMethod]
        public void RemoveItemFromBasket_ShouldRemoveItemFromBasket_WhenValidArgumentsArePassed()
        {
            // Arange 
            var validItemId = 10;
            var validUserId = "1234";

            User user = new User();
            Item item1 = new Item();
            Item item2 = new Item();
            Item item3 = new Item();
            Order order = new Order();
            order.Items.Add(item1);
            order.Items.Add(item2);
            order.Items.Add(item3);

            this.userRepoMock.Setup(r => r.GetById(validUserId)).Returns(user);
            this.itemRepoMock.Setup(r => r.GetById(validItemId)).Returns(item3);
            this.orderRepoMock.Setup(r => r.GetSingleOrDefault(It.IsAny<Expression<Func<Order, bool>>>())).Returns(order);

            this.dataMock.SetupGet(d => d.Users).Returns(this.userRepoMock.Object);
            this.dataMock.SetupGet(d => d.Items).Returns(this.itemRepoMock.Object);
            this.dataMock.SetupGet(d => d.Orders).Returns(this.orderRepoMock.Object);

            var orderService = new OrderService(this.dataMock.Object);
            var initialCount = order.Items.Count;

            // Act
            orderService.RemoveItemFromBasket(validItemId, validUserId);
            var currentCount = order.Items.Count;

            // Assert
            Assert.AreEqual(initialCount, currentCount + 1);
        }

        [TestMethod]
        public void RemoveItemFromBasket_ShouldIncreaseItemQuantity_WhenValidArgumentsArePassed()
        {
            // Arange 
            var validItemId = 10;
            var validUserId = "1234";

            User user = new User();
            Item item = new Item();
            Order order = new Order();

            this.userRepoMock.Setup(r => r.GetById(validUserId)).Returns(user);
            this.itemRepoMock.Setup(r => r.GetById(validItemId)).Returns(item);
            this.orderRepoMock.Setup(r => r.GetSingleOrDefault(It.IsAny<Expression<Func<Order, bool>>>())).Returns(order);

            this.dataMock.SetupGet(d => d.Users).Returns(this.userRepoMock.Object);
            this.dataMock.SetupGet(d => d.Items).Returns(this.itemRepoMock.Object);
            this.dataMock.SetupGet(d => d.Orders).Returns(this.orderRepoMock.Object);

            var orderService = new OrderService(this.dataMock.Object);
            var initialItemQuantity = 10;
            item.Quantity = initialItemQuantity;

            // Act
            orderService.AddItemToBasket(validItemId, validUserId);
            var currentItemQuantity = item.Quantity;

            // Assert
            Assert.AreEqual(initialItemQuantity, currentItemQuantity + 1);
        }

        [TestMethod]
        public void RemoveItemFromBasket_ShouldChangeOrderStatusToDiscarded_WhenValidArgumentsArePassedAndNoItemAreLeftInThenBasket()
        {
            // Arange 
            var validItemId = 10;
            var validUserId = "1234";

            User user = new User();
            Item item = new Item();
            Order order = new Order();
            order.Items.Add(item);

            this.userRepoMock.Setup(r => r.GetById(validUserId)).Returns(user);
            this.itemRepoMock.Setup(r => r.GetById(validItemId)).Returns(item);
            this.orderRepoMock.Setup(r => r.GetSingleOrDefault(It.IsAny<Expression<Func<Order, bool>>>())).Returns(order);

            this.dataMock.SetupGet(d => d.Users).Returns(this.userRepoMock.Object);
            this.dataMock.SetupGet(d => d.Items).Returns(this.itemRepoMock.Object);
            this.dataMock.SetupGet(d => d.Orders).Returns(this.orderRepoMock.Object);

            var orderService = new OrderService(this.dataMock.Object);

            // Act
            orderService.RemoveItemFromBasket(validItemId, validUserId);

            // Assert
            Assert.AreEqual(order.OrderStatus, OrderStatusType.Discarded);
        }

        [TestMethod]
        public void RemoveItemFromBasket_ShouldCallDataCommit_WhenValidArgumentsArePassed()
        {
            // Arange 
            var validItemId = 10;
            var validUserId = "1234";

            User user = new User();
            Item item = new Item();
            Order order = new Order();

            this.userRepoMock.Setup(r => r.GetById(validUserId)).Returns(user);
            this.itemRepoMock.Setup(r => r.GetById(validItemId)).Returns(item);
            this.orderRepoMock.Setup(r => r.GetSingleOrDefault(It.IsAny<Expression<Func<Order, bool>>>())).Returns(order);

            this.dataMock.SetupGet(d => d.Users).Returns(this.userRepoMock.Object);
            this.dataMock.SetupGet(d => d.Items).Returns(this.itemRepoMock.Object);
            this.dataMock.SetupGet(d => d.Orders).Returns(this.orderRepoMock.Object);

            var orderService = new OrderService(this.dataMock.Object);

            // Act
            orderService.AddItemToBasket(validItemId, validUserId);

            // Assert
            dataMock.Verify(d => d.Commit(), Times.Once);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void ExecuteOrder_ShouldThrowArgumentOutOfRangeException_WhenOrderIdIsInvalid()
        {
            // Arange 
            var invalidOrderId = -4;
            var validTotalPrice = 1000;

            var orderService = new OrderService(this.dataMock.Object);

            // Act & Assert
            orderService.ExecuteOrder(invalidOrderId, validTotalPrice);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void ExecuteOrder_ShouldThrowArgumentOutOfRangeException_WhenTotalPriceIsNegative()
        {
            // Arange 
            var validOrderId = 11;
            var invalidTotalPrice = -1000;

            var orderService = new OrderService(this.dataMock.Object);

            // Act & Assert
            orderService.ExecuteOrder(validOrderId, invalidTotalPrice);
        }

        [TestMethod]
        [ExpectedException(typeof(ObjectNotFoundInDatabaseException))]
        public void ExecuteOrder_ShouldThrowObjectNotFoundInDatabaseException_WhenNoOrderWithGivenIdIsFound()
        {
            // Arange 
            var orderId = 11;
            var totalPrice = 1000;
            Order order = null;

            this.orderRepoMock.Setup(r => r.GetSingleOrDefault(It.IsAny<Expression<Func<Order, bool>>>())).Returns(order);
            this.dataMock.SetupGet(d => d.Orders).Returns(this.orderRepoMock.Object);

            var orderService = new OrderService(this.dataMock.Object);
            
            // Act & Assert
            orderService.ExecuteOrder(orderId, totalPrice);
        }

        [TestMethod]
        public void ExecuteOrder_ShouldSetOrderStatusShipped_WhenValidArgumentArePassed()
        {
            // Arange 
            var orderId = 11;
            var totalPrice = 1000;
            Order order = new Order();
            
            this.orderRepoMock.Setup(r => r.GetById(orderId)).Returns(order);
            this.dataMock.SetupGet(d => d.Orders).Returns(this.orderRepoMock.Object);

            var orderService = new OrderService(this.dataMock.Object);

            // Act
            orderService.ExecuteOrder(orderId, totalPrice);

            // Assert
            Assert.AreEqual(order.OrderStatus, OrderStatusType.Shipped);
        }

        [TestMethod]
        public void ExecuteOrder_ShouldSetOrderTotalPriceCorrect_WhenValidArgumentArePassed()
        {
            // Arange 
            var orderId = 11;
            var totalPrice = 1000;
            Order order = new Order();

            this.orderRepoMock.Setup(r => r.GetById(orderId)).Returns(order);
            this.dataMock.SetupGet(d => d.Orders).Returns(this.orderRepoMock.Object);

            var orderService = new OrderService(this.dataMock.Object);

            // Act
            orderService.ExecuteOrder(orderId, totalPrice);

            // Assert
            Assert.AreEqual(order.TotalPrice, totalPrice);
        }

        [TestMethod]
        public void ExecuteOrder_ShouldCallDataCommit_WhenValidArgumentArePassed()
        {
            // Arange 
            var orderId = 11;
            var totalPrice = 1000;
            Order order = new Order();

            this.orderRepoMock.Setup(r => r.GetById(orderId)).Returns(order);
            this.dataMock.SetupGet(d => d.Orders).Returns(this.orderRepoMock.Object);

            var orderService = new OrderService(this.dataMock.Object);

            // Act
            orderService.ExecuteOrder(orderId, totalPrice);

            // Assert
            this.dataMock.Verify(d => d.Commit(), Times.Once);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void PreviosOrders_ShouldThrowArgumentNullException_WhenUserIdIsInvalid()
        {
            // Arange 
            string invalidUserId = null;

            var orderService = new OrderService(this.dataMock.Object);

            // Act & Assert
            orderService.PreviousOrders(invalidUserId);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void PreviosOrders_ShouldCallOrdersRepoGetAll_WhenUserIdIsValid()
        {
            // Arange 
            string userId = null;
            var orders = new List<Order>();

            this.orderRepoMock.Setup(r => r.GetAll(It.IsAny<Expression<Func<Order, bool>>>())).Returns(orders);
            this.dataMock.SetupGet(d => d.Orders).Returns(this.orderRepoMock.Object);
            var orderService = new OrderService(this.dataMock.Object);

            // Act
            orderService.PreviousOrders(userId);

            // Assert
            this.orderRepoMock.Verify(d => d.GetAll(), Times.Once);

        }
    }
}
