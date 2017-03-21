using System;
using System.Text;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Fancy.Data.Repositories;
using Fancy.Data.Models.Models;
using Fancy.Data.Data;
using Fancy.Data.Contexts;

namespace Fancy.Tests.Data.Data
{
    [TestClass]
    public class EfFancyDataTests
    { 
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Constructor_ShouldThrowArgumentNullException_WhenContextParameterIsNull()
        {
            // Arrange
            var itemRepoMock = new Mock<IEfGenericRepository<Item>>();
            var orderRepoMock = new Mock<IEfGenericRepository<Order>>();
            var userRepoMock = new Mock<IEfGenericRepository<User>>();

            // Act & Assert
            var data = new EfFancyData(null, itemRepoMock.Object, orderRepoMock.Object, userRepoMock.Object);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Constructor_ShouldThrowArgumentNullException_WhenItemRepoParameterIsNull()
        {
            // Arrange
            var dbContextMock = new Mock<IFancyDbContext>();
            var orderRepoMock = new Mock<IEfGenericRepository<Order>>();
            var userRepoMock = new Mock<IEfGenericRepository<User>>();

            // Act & Assert
            var data = new EfFancyData(dbContextMock.Object, null, orderRepoMock.Object, userRepoMock.Object);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Constructor_ShouldThrowArgumentNullException_WhenOrderRepoParameterIsNull()
        {
            // Arrange
            var dbContextMock = new Mock<IFancyDbContext>();
            var itemRepoMock = new Mock<IEfGenericRepository<Item>>();
            var userRepoMock = new Mock<IEfGenericRepository<User>>();

            // Act & Assert
            var data = new EfFancyData(dbContextMock.Object, itemRepoMock.Object, null, userRepoMock.Object);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Constructor_ShouldThrowArgumentNullException_WhenUserRepoParameterIsNull()
        {
            // Arrange
            var dbContextMock = new Mock<IFancyDbContext>();
            var itemRepoMock = new Mock<IEfGenericRepository<Item>>();
            var orderRepoMock = new Mock<IEfGenericRepository<Order>>();

            // Act & Assert
            var data = new EfFancyData(dbContextMock.Object, itemRepoMock.Object, orderRepoMock.Object, null);
        }

        [TestMethod]
        public void Commit_ShouldCallDbContextSaveChanges()
        {
            // Arrange
            var dbContextMock = new Mock<IFancyDbContext>();
            var itemRepoMock = new Mock<IEfGenericRepository<Item>>();
            var orderRepoMock = new Mock<IEfGenericRepository<Order>>();
            var userRepoMock = new Mock<IEfGenericRepository<User>>();

            // Act
            var data = new EfFancyData(dbContextMock.Object, itemRepoMock.Object, orderRepoMock.Object, userRepoMock.Object);
            data.Commit();
            
            // Assert
            dbContextMock.Verify(c => c.SaveChanges(), Times.Once);
        }

        [TestMethod]
        public void Dispose_ShouldCallDbContextDispose()
        {
            // Arrange
            var dbContextMock = new Mock<IFancyDbContext>();
            var itemRepoMock = new Mock<IEfGenericRepository<Item>>();
            var orderRepoMock = new Mock<IEfGenericRepository<Order>>();
            var userRepoMock = new Mock<IEfGenericRepository<User>>();

            // Act
            var data = new EfFancyData(dbContextMock.Object, itemRepoMock.Object, orderRepoMock.Object, userRepoMock.Object);
            data.Dispose();

            // Assert
            dbContextMock.Verify(c => c.Dispose(), Times.Once);
        }

        [TestMethod]
        public void GetItems_ShouldReturnCorrectInstanceOfItemRepository()
        {
            // Arrange
            var dbContextMock = new Mock<IFancyDbContext>();
            var itemRepoMock = new Mock<IEfGenericRepository<Item>>();
            var orderRepoMock = new Mock<IEfGenericRepository<Order>>();
            var userRepoMock = new Mock<IEfGenericRepository<User>>();

            // Act
            var data = new EfFancyData(dbContextMock.Object, itemRepoMock.Object, orderRepoMock.Object, userRepoMock.Object);
            var itemRepo = data.Items;

            // Assert
            Assert.AreEqual(itemRepo, itemRepoMock.Object);
        }

        [TestMethod]
        public void GetOrders_ShouldReturnCorrectInstanceOfItemRepository()
        {
            // Arrange
            var dbContextMock = new Mock<IFancyDbContext>();
            var itemRepoMock = new Mock<IEfGenericRepository<Item>>();
            var orderRepoMock = new Mock<IEfGenericRepository<Order>>();
            var userRepoMock = new Mock<IEfGenericRepository<User>>();

            // Act
            var data = new EfFancyData(dbContextMock.Object, itemRepoMock.Object, orderRepoMock.Object, userRepoMock.Object);
            var orderRepo = data.Orders;

            // Assert
            Assert.AreEqual(orderRepo, orderRepoMock.Object);
        }

        [TestMethod]
        public void GetUsers_ShouldReturnCorrectInstanceOfItemRepository()
        {
            // Arrange
            var dbContextMock = new Mock<IFancyDbContext>();
            var itemRepoMock = new Mock<IEfGenericRepository<Item>>();
            var orderRepoMock = new Mock<IEfGenericRepository<Order>>();
            var userRepoMock = new Mock<IEfGenericRepository<User>>();

            // Act
            var data = new EfFancyData(dbContextMock.Object, itemRepoMock.Object, orderRepoMock.Object, userRepoMock.Object);
            var usersRepo = data.Users;

            // Assert
            Assert.AreEqual(usersRepo, userRepoMock.Object);
        }
    }
}
