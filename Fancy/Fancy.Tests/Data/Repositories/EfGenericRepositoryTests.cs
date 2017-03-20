using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Fancy.Data.Contexts;
using Moq;
using Fancy.Data.Repositories;
using Fancy.Data.Models.Models;

namespace Fancy.Tests.Data.Repositories
{
    [TestClass]
    public class EfGenericRepositoryTests
    {
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ConstructorForItemRepository_ShouldThrowNullArgumentException_WhenContextIsNull()
        {
            // Arange & Act & Assert
            var efGenericRepository = new EfGenericRepository<Item>(null);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ConstructorForUserRepository_ShouldThrowNullArgumentException_WhenContextIsNull()
        {
            // Arange & Act & Assert
            var efGenericRepository = new EfGenericRepository<User>(null);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ConstructorForOrderRepository_ShouldThrowNullArgumentException_WhenContextIsNull()
        {
            // Arange & Act & Assert
            var efGenericRepository = new EfGenericRepository<Order>(null);
        }

        [TestMethod]
        public void ConstructorForItemRepository_ShouldSetDbSet_WhenContextIsNotNull()
        {
            // Arange 
            var context = new Mock<IFancyDbContext>();
            
            // Act
            var efGenericRepository = new EfGenericRepository<Item>(context.Object);

            // Assert
            context.Verify(c => c.Set<Item>(), Times.Once);
        }

        [TestMethod]
        public void ConstructorForUserRepository_ShouldSetDbSet_WhenContextIsNotNull()
        {
            // Arange 
            var context = new Mock<IFancyDbContext>();

            // Act
            var efGenericRepository = new EfGenericRepository<User>(context.Object);

            // Assert
            context.Verify(c => c.Set<User>(), Times.Once);
        }

        [TestMethod]
        public void ConstructorForOrderRepository_ShouldSetDbSet_WhenContextIsNotNull()
        {
            // Arange 
            var context = new Mock<IFancyDbContext>();

            // Act
            var efGenericRepository = new EfGenericRepository<Order>(context.Object);

            // Assert
            context.Verify(c => c.Set<Order>(), Times.Once);
        }
    }
}
