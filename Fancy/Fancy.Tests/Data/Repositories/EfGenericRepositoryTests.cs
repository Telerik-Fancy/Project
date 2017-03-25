using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Fancy.Data.Models.Models;
using Fancy.Data.Repositories;
using Fancy.Data.Contexts;
using Fancy.Common.Enums;

namespace Fancy.Tests.Data.Repositories
{
    [TestClass]
    public class EfGenericRepositoryTests
    {
        private EfGenericRepository<Item> repo;
        private Mock<IFancyDbContext> context;
        private Mock<IDbSet<Item>> itemsDbSet;


        [TestInitialize]
        public void Initialize()
        {
            this.context = new Mock<IFancyDbContext>();
            this.itemsDbSet = new Mock<IDbSet<Item>>();
        }

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
            var efGenericRepository = new EfGenericRepository<Item>(context.Object);

            // Assert
            context.Verify(c => c.Set<Item>(), Times.Once);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Add_ShouldThrowArgumentNullException_WhenAddedEntityIsNull()
        {
            // Arrange
            Item item1 = null;
            var itemsCollection = new List<Item>();

            this.itemsDbSet.Setup(i => i.Add(It.IsAny<Item>()))
                      .Callback<Item>(i => itemsCollection.Add(i));

            this.context.Setup(c => c.Set<Item>()).Returns(this.itemsDbSet.Object);

            this.repo = new EfGenericRepository<Item>(this.context.Object);

            this.repo.Add(item1);
        }

        [TestMethod]
        public void Add_ShouldAddItemToCollection_CaseOneObject()
        {
            // Arrange
            var item1 = new Item();
            var itemsCollection = new List<Item>();

            this.itemsDbSet.Setup(i => i.Add(It.IsAny<Item>()))
                      .Callback<Item>(i => itemsCollection.Add(i));

            this.context.Setup(c => c.Set<Item>()).Returns(this.itemsDbSet.Object);

            this.repo = new EfGenericRepository<Item>(this.context.Object);

            // Act
            this.repo.Add(item1);

            // Assert
            Assert.AreEqual(item1, itemsCollection[0]);
        }

        [TestMethod]
        public void Add_ShouldAddItemToCollection_CaseManyObjects()
        {
            // Arrange
            var item1 = new Item();
            var item2 = new Item();
            var item3 = new Item();
            var itemsCollection = new List<Item>();

            this.itemsDbSet.Setup(i => i.Add(It.IsAny<Item>()))
                      .Callback<Item>(i => itemsCollection.Add(i));

            this.context.Setup(c => c.Set<Item>()).Returns(this.itemsDbSet.Object);

            this.repo = new EfGenericRepository<Item>(this.context.Object);

            // Act
            this.repo.Add(item1);
            this.repo.Add(item2);
            this.repo.Add(item3);

            // Assert
            Assert.AreEqual(item1, itemsCollection[0]);
            Assert.AreEqual(item2, itemsCollection[1]);
            Assert.AreEqual(item3, itemsCollection[2]);
        }

        [TestMethod]
        public void Delete_ShouldDeleteItemFromCollection()
        {
            // Arrange
            var item1 = new Item();
            var item2 = new Item();
            var item3 = new Item();
            var itemsCollection = new List<Item>();

            this.itemsDbSet.Setup(i => i.Add(It.IsAny<Item>()))
                      .Callback<Item>(i => itemsCollection.Add(i));

            this.itemsDbSet.Setup(i => i.Remove(It.IsAny<Item>()))
                      .Callback<Item>(i => itemsCollection.Remove(i));

            this.context.Setup(c => c.Set<Item>()).Returns(this.itemsDbSet.Object);

            this.repo = new EfGenericRepository<Item>(this.context.Object);

            // Act
            this.repo.Add(item1);
            this.repo.Add(item2);
            this.repo.Add(item3);

            this.repo.Delete(item2);

            // Assert
            Assert.AreEqual(item1, itemsCollection[0]);
            Assert.AreEqual(item3, itemsCollection[1]);
        }

        [TestMethod]
        public void All_ShouldReturnAllItemsInCollection()
        {
            // Arrange
            var item1 = new Item() { Id = 1 };
            var item2 = new Item() { Id = 2 };
            var item3 = new Item() { Id = 3 };

            var itemsCollection = new List<Item>();

            var queryData = itemsCollection.AsQueryable();

            this.itemsDbSet.As<IQueryable<Item>>().Setup(m => m.Provider).Returns(queryData.Provider);
            this.itemsDbSet.As<IQueryable<Item>>().Setup(m => m.Expression).Returns(queryData.Expression);
            this.itemsDbSet.As<IQueryable<Item>>().Setup(m => m.ElementType).Returns(queryData.ElementType);
            this.itemsDbSet.As<IQueryable<Item>>().Setup(m => m.GetEnumerator()).Returns(queryData.GetEnumerator());
            this.itemsDbSet.Setup(i => i.Add(It.IsAny<Item>()))
                      .Callback<Item>(i => itemsCollection.Add(i));
            this.context.Setup(c => c.Set<Item>()).Returns(this.itemsDbSet.Object);

            this.repo = new EfGenericRepository<Item>(this.context.Object);

            // Act
            this.repo.Add(item1);
            this.repo.Add(item2);
            this.repo.Add(item3);
 
            var expectedCount = 3;
            var actualCount = this.repo.All.Count();
            
            // Assert
            Assert.AreEqual(expectedCount, actualCount);
        }

        [TestMethod]
        public void GetById_ShouldReturnItemWithTheGivenId_CaseSmallNumberId()
        {
            // Arrange
            var id1 = 1;
            var id2 = 2;
            var id3 = 33131231;

            var item1 = new Item() { Id = id1 };
            var item2 = new Item() { Id = id2 };
            var item3 = new Item() { Id = id3 };

            var itemsCollection = new List<Item>() { item1, item2, item3 };

            var queryData = itemsCollection.AsQueryable();

            this.itemsDbSet.As<IQueryable<Item>>().Setup(m => m.Provider).Returns(queryData.Provider);
            this.itemsDbSet.As<IQueryable<Item>>().Setup(m => m.Expression).Returns(queryData.Expression);
            this.itemsDbSet.As<IQueryable<Item>>().Setup(m => m.ElementType).Returns(queryData.ElementType);
            this.itemsDbSet.As<IQueryable<Item>>().Setup(m => m.GetEnumerator()).Returns(queryData.GetEnumerator());

            this.context.Setup(c => c.Set<Item>()).Returns(this.itemsDbSet.Object);

            this.repo = new EfGenericRepository<Item>(this.context.Object);

            // Act
            var item = this.repo.GetById(id1);

            // Assert
            this.itemsDbSet.Verify(i => i.Find(id1), Times.Once);
        }

        [TestMethod]
        public void GetById_ShouldReturnItemWithTheGivenId_CaseLargeNumberId()
        {
            // Arrange
            var id1 = 1;
            var id2 = 2;
            var id3 = 33131231;

            var item1 = new Item() { Id = id1 };
            var item2 = new Item() { Id = id2 };
            var item3 = new Item() { Id = id3 };

            var items = new List<Item>() { item1, item2, item3 };
            var queryData = items.AsQueryable();

            this.itemsDbSet.Setup(i => i.Find(It.IsNotNull<object>()));
            this.itemsDbSet.As<IQueryable<Item>>().Setup(m => m.Provider).Returns(queryData.Provider);
            this.itemsDbSet.As<IQueryable<Item>>().Setup(m => m.Expression).Returns(queryData.Expression);
            this.itemsDbSet.As<IQueryable<Item>>().Setup(m => m.ElementType).Returns(queryData.ElementType);
            this.itemsDbSet.As<IQueryable<Item>>().Setup(m => m.GetEnumerator()).Returns(queryData.GetEnumerator());

            this.context.Setup(c => c.Set<Item>()).Returns(this.itemsDbSet.Object);

            this.repo = new EfGenericRepository<Item>(this.context.Object);

            // Act
            var item = this.repo.GetById(id3);

            // Assert
            this.itemsDbSet.Verify(i => i.Find(id3), Times.Once);
        }

        [TestMethod]
        public void GetSingle_ShouldReturnSingleResult_IfSingleItemIsPresentInDatabase()
        {
            // Arrange
            var id1 = 1;
            var id2 = 2;
            var id3 = 3;

            var item1 = new Item() { Id = id1 };
            var item2 = new Item() { Id = id2 };
            var item3 = new Item() { Id = id3 };

            var itemsCollection = new List<Item>() { item1, item2, item3 };

            var queryData = itemsCollection.AsQueryable();

            this.itemsDbSet.As<IQueryable<Item>>().Setup(m => m.Provider).Returns(queryData.Provider);
            this.itemsDbSet.As<IQueryable<Item>>().Setup(m => m.Expression).Returns(queryData.Expression);
            this.itemsDbSet.As<IQueryable<Item>>().Setup(m => m.ElementType).Returns(queryData.ElementType);
            this.itemsDbSet.As<IQueryable<Item>>().Setup(m => m.GetEnumerator()).Returns(queryData.GetEnumerator());

            this.context.Setup(c => c.Set<Item>()).Returns(this.itemsDbSet.Object);

            this.repo = new EfGenericRepository<Item>(this.context.Object);

            // Act
            var item = this.repo.GetSingle(i => i.Id == id1);

            // Assert
            Assert.AreEqual(item, item1);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void GetSingle_ShouldThrowInvalidOperationException_IfMoreThanONeItemThatSatisfyTheConditionArePresentInDatabase()
        {
            // Arrange
            var id1 = 1;
            var id2 = 2;
            var id3 = 3;

            var item1 = new Item() { Id = id1 };
            var item2 = new Item() { Id = id2 };
            var item3 = new Item() { Id = id3 };

            var itemsCollection = new List<Item>() { item1, item2, item3 };
            itemsCollection.Add(item1);

            var queryData = itemsCollection.AsQueryable();

            this.itemsDbSet.As<IQueryable<Item>>().Setup(m => m.Provider).Returns(queryData.Provider);
            this.itemsDbSet.As<IQueryable<Item>>().Setup(m => m.Expression).Returns(queryData.Expression);
            this.itemsDbSet.As<IQueryable<Item>>().Setup(m => m.ElementType).Returns(queryData.ElementType);
            this.itemsDbSet.As<IQueryable<Item>>().Setup(m => m.GetEnumerator()).Returns(queryData.GetEnumerator());

            this.context.Setup(c => c.Set<Item>()).Returns(this.itemsDbSet.Object);

            this.repo = new EfGenericRepository<Item>(this.context.Object);

            // Act
            var item = this.repo.GetSingle(i => i.Id == id1);

            // Assert
            Assert.AreEqual(item, item1);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void GetSingle_ShouldThrowInvalidOperationException_IfNoItemsAreFound()
        {
            // Arrange
            var id1 = 1;
            var id2 = 2;
            var id3 = 3;
            var invalidId = 4;

            var item1 = new Item() { Id = id1 };
            var item2 = new Item() { Id = id2 };
            var item3 = new Item() { Id = id3 };

            var itemsCollection = new List<Item>() { item1, item2, item3 };
            itemsCollection.Add(item1);

            var queryData = itemsCollection.AsQueryable();

            this.itemsDbSet.As<IQueryable<Item>>().Setup(m => m.Provider).Returns(queryData.Provider);
            this.itemsDbSet.As<IQueryable<Item>>().Setup(m => m.Expression).Returns(queryData.Expression);
            this.itemsDbSet.As<IQueryable<Item>>().Setup(m => m.ElementType).Returns(queryData.ElementType);
            this.itemsDbSet.As<IQueryable<Item>>().Setup(m => m.GetEnumerator()).Returns(queryData.GetEnumerator());

            this.context.Setup(c => c.Set<Item>()).Returns(this.itemsDbSet.Object);

            this.repo = new EfGenericRepository<Item>(this.context.Object);

            // Act
            var item = this.repo.GetSingle(i => i.Id == invalidId);

            // Assert
            Assert.AreEqual(item, item1);
        }

        [TestMethod]
        public void GetSingleOrDefault_ShouldReturnSingleResult_IfSingleItemIsPresentInDatabase()
        {
            // Arrange
            var id1 = 1;
            var id2 = 2;
            var id3 = 3;

            var item1 = new Item() { Id = id1 };
            var item2 = new Item() { Id = id2 };
            var item3 = new Item() { Id = id3 };

            var itemsCollection = new List<Item>() { item1, item2, item3 };

            var queryData = itemsCollection.AsQueryable();

            this.itemsDbSet.As<IQueryable<Item>>().Setup(m => m.Provider).Returns(queryData.Provider);
            this.itemsDbSet.As<IQueryable<Item>>().Setup(m => m.Expression).Returns(queryData.Expression);
            this.itemsDbSet.As<IQueryable<Item>>().Setup(m => m.ElementType).Returns(queryData.ElementType);
            this.itemsDbSet.As<IQueryable<Item>>().Setup(m => m.GetEnumerator()).Returns(queryData.GetEnumerator());

            this.context.Setup(c => c.Set<Item>()).Returns(this.itemsDbSet.Object);

            this.repo = new EfGenericRepository<Item>(this.context.Object);

            // Act
            var item = this.repo.GetSingleOrDefault(i => i.Id == id1);

            // Assert
            Assert.AreEqual(item, item1);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void GetSingle_ShouldReturnNull_IfMoreThanONeItemThatSatisfyTheConditionArePresentInDatabase()
        {
            // Arrange
            var id1 = 1;
            var id2 = 2;
            var id3 = 3;

            var item1 = new Item() { Id = id1 };
            var item2 = new Item() { Id = id2 };
            var item3 = new Item() { Id = id3 };

            var itemsCollection = new List<Item>() { item1, item2, item3 };
            itemsCollection.Add(item1);

            var queryData = itemsCollection.AsQueryable();

            this.itemsDbSet.As<IQueryable<Item>>().Setup(m => m.Provider).Returns(queryData.Provider);
            this.itemsDbSet.As<IQueryable<Item>>().Setup(m => m.Expression).Returns(queryData.Expression);
            this.itemsDbSet.As<IQueryable<Item>>().Setup(m => m.ElementType).Returns(queryData.ElementType);
            this.itemsDbSet.As<IQueryable<Item>>().Setup(m => m.GetEnumerator()).Returns(queryData.GetEnumerator());

            this.context.Setup(c => c.Set<Item>()).Returns(this.itemsDbSet.Object);

            this.repo = new EfGenericRepository<Item>(this.context.Object);

            // Act
            var item = this.repo.GetSingleOrDefault(i => i.Id == id1);

            // Assert
            Assert.AreEqual(item, null);
        }

        [TestMethod]
        public void GetFirst_ShouldReturnFirstResultInDatabase()
        {
            // Arrange
            var id1 = 1;
            var id2 = 2;
            var id3 = 3;

            var item1 = new Item() { Id = id1 };
            var item2 = new Item() { Id = id2 };
            var item3 = new Item() { Id = id3 };

            var itemsCollection = new List<Item>() { item1, item2, item3 };

            var queryData = itemsCollection.AsQueryable();

            this.itemsDbSet.As<IQueryable<Item>>().Setup(m => m.Provider).Returns(queryData.Provider);
            this.itemsDbSet.As<IQueryable<Item>>().Setup(m => m.Expression).Returns(queryData.Expression);
            this.itemsDbSet.As<IQueryable<Item>>().Setup(m => m.ElementType).Returns(queryData.ElementType);
            this.itemsDbSet.As<IQueryable<Item>>().Setup(m => m.GetEnumerator()).Returns(queryData.GetEnumerator());

            this.context.Setup(c => c.Set<Item>()).Returns(this.itemsDbSet.Object);

            this.repo = new EfGenericRepository<Item>(this.context.Object);

            // Act
            var item = this.repo.GetFirst(i => i.Id == id2);

            // Assert
            Assert.AreEqual(item, item2);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void GetFirst_ShouldThrowInvalidOperationException_IfNoItemIsFound()
        {
            // Arrange
            var id1 = 1;
            var id2 = 2;
            var id3 = 3;
            var invalidId = 4;

            var item1 = new Item() { Id = id1 };
            var item2 = new Item() { Id = id2 };
            var item3 = new Item() { Id = id3 };

            var itemsCollection = new List<Item>() { item1, item2, item3 };

            var queryData = itemsCollection.AsQueryable();

            this.itemsDbSet.As<IQueryable<Item>>().Setup(m => m.Provider).Returns(queryData.Provider);
            this.itemsDbSet.As<IQueryable<Item>>().Setup(m => m.Expression).Returns(queryData.Expression);
            this.itemsDbSet.As<IQueryable<Item>>().Setup(m => m.ElementType).Returns(queryData.ElementType);
            this.itemsDbSet.As<IQueryable<Item>>().Setup(m => m.GetEnumerator()).Returns(queryData.GetEnumerator());

            this.context.Setup(c => c.Set<Item>()).Returns(this.itemsDbSet.Object);

            this.repo = new EfGenericRepository<Item>(this.context.Object);

            // Act
            var item = this.repo.GetSingle(i => i.Id == invalidId);

            // Assert
            Assert.AreEqual(item, item1);
        }

        [TestMethod]
        public void GetFirstOrDefault_ShouldReturnFirstResult_IfItemIsPresentInDatabase()
        {
            // Arrange
            var id1 = 1;
            var id2 = 2;
            var id3 = 3;

            var item1 = new Item() { Id = id1 };
            var item2 = new Item() { Id = id2 };
            var item3 = new Item() { Id = id3 };

            var itemsCollection = new List<Item>() { item1, item2, item3 };

            var queryData = itemsCollection.AsQueryable();

            this.itemsDbSet.As<IQueryable<Item>>().Setup(m => m.Provider).Returns(queryData.Provider);
            this.itemsDbSet.As<IQueryable<Item>>().Setup(m => m.Expression).Returns(queryData.Expression);
            this.itemsDbSet.As<IQueryable<Item>>().Setup(m => m.ElementType).Returns(queryData.ElementType);
            this.itemsDbSet.As<IQueryable<Item>>().Setup(m => m.GetEnumerator()).Returns(queryData.GetEnumerator());

            this.context.Setup(c => c.Set<Item>()).Returns(this.itemsDbSet.Object);

            this.repo = new EfGenericRepository<Item>(this.context.Object);

            // Act
            var item = this.repo.GetFirstOrDefault(i => i.Id == id2);

            // Assert
            Assert.AreEqual(item, item2);
        }

        [TestMethod]
        public void GetFirstOrDefault_ShouldReturnFirstResult_IfManyItemsArePresentInDatabase()
        {
            // Arrange
            var id1 = 1;
            var id2 = 2;
            var id3 = 3;

            var item1 = new Item() { Id = id1 };
            var item2 = new Item() { Id = id2 };
            var item3 = new Item() { Id = id3 };
            var item4 = new Item() { Id = id2 };
            var item5 = new Item() { Id = id2 };

            var itemsCollection = new List<Item>() { item1, item2, item3, item4, item5 };

            var queryData = itemsCollection.AsQueryable();

            this.itemsDbSet.As<IQueryable<Item>>().Setup(m => m.Provider).Returns(queryData.Provider);
            this.itemsDbSet.As<IQueryable<Item>>().Setup(m => m.Expression).Returns(queryData.Expression);
            this.itemsDbSet.As<IQueryable<Item>>().Setup(m => m.ElementType).Returns(queryData.ElementType);
            this.itemsDbSet.As<IQueryable<Item>>().Setup(m => m.GetEnumerator()).Returns(queryData.GetEnumerator());

            this.context.Setup(c => c.Set<Item>()).Returns(this.itemsDbSet.Object);

            this.repo = new EfGenericRepository<Item>(this.context.Object);

            // Act
            var item = this.repo.GetFirstOrDefault(i => i.Id == id2);

            // Assert
            Assert.AreEqual(item, item2);
        }

        [TestMethod]
        public void GetFirstOrDefault_ShouldReturnNull_IfMoreThanONeItemThatSatisfyTheConditionIsPresentInDatabase()
        {
            // Arrange
            var id1 = 1;
            var id2 = 2;
            var id3 = 3;
            var invalidId = 4;

            var item1 = new Item() { Id = id1 };
            var item2 = new Item() { Id = id2 };
            var item3 = new Item() { Id = id3 };

            var itemsCollection = new List<Item>() { item1, item2, item3 };

            var queryData = itemsCollection.AsQueryable();

            this.itemsDbSet.As<IQueryable<Item>>().Setup(m => m.Provider).Returns(queryData.Provider);
            this.itemsDbSet.As<IQueryable<Item>>().Setup(m => m.Expression).Returns(queryData.Expression);
            this.itemsDbSet.As<IQueryable<Item>>().Setup(m => m.ElementType).Returns(queryData.ElementType);
            this.itemsDbSet.As<IQueryable<Item>>().Setup(m => m.GetEnumerator()).Returns(queryData.GetEnumerator());

            this.context.Setup(c => c.Set<Item>()).Returns(this.itemsDbSet.Object);

            this.repo = new EfGenericRepository<Item>(this.context.Object);

            // Act
            var item = this.repo.GetFirstOrDefault(i => i.Id == invalidId);

            // Assert
            Assert.AreEqual(item, null);
        }

        [TestMethod]
        public void GetAll_FilteredByMainMaterial_ShouldReturnCorrectResult()
        {
            // Arrange
            var item1 = new Item() { Id = 1, MainMaterial = MainMaterialType.Alloy, MainColour = MainColourType.Red, Price = 10 };
            var item2 = new Item() { Id = 2, MainMaterial = MainMaterialType.Swarovski, MainColour = MainColourType.Blue, Price = 14 };
            var item3 = new Item() { Id = 3, MainMaterial = MainMaterialType.Swarovski, MainColour = MainColourType.Golden, Price = 15 };
            var item4 = new Item() { Id = 4, MainMaterial = MainMaterialType.Alloy, MainColour = MainColourType.Red, Price = 100 };
            var item5 = new Item() { Id = 5, MainMaterial = MainMaterialType.PinkGold, MainColour = MainColourType.Blue, Price = 250 };
            var item6 = new Item() { Id = 6, MainMaterial = MainMaterialType.Alloy, MainColour = MainColourType.Red, Price = 5 };

            var itemsCollection = new List<Item>() { item1, item2, item3, item4, item5, item6 };

            var queryData = itemsCollection.AsQueryable();

            this.itemsDbSet.As<IQueryable<Item>>().Setup(m => m.Provider).Returns(queryData.Provider);
            this.itemsDbSet.As<IQueryable<Item>>().Setup(m => m.Expression).Returns(queryData.Expression);
            this.itemsDbSet.As<IQueryable<Item>>().Setup(m => m.ElementType).Returns(queryData.ElementType);
            this.itemsDbSet.As<IQueryable<Item>>().Setup(m => m.GetEnumerator()).Returns(queryData.GetEnumerator());

            this.context.Setup(c => c.Set<Item>()).Returns(this.itemsDbSet.Object);

            this.repo = new EfGenericRepository<Item>(this.context.Object);

            // Act
            var actualFiltered = this.repo.GetAll(i => i.MainMaterial == MainMaterialType.Swarovski).ToList();
            var expectedFiltered = new List<Item>() { item2, item3 };
            
            // Assert
            CollectionAssert.AreEqual(actualFiltered, expectedFiltered);
        }

        [TestMethod]
        public void GetAll_FilteredByMainColour_ShouldReturnCorrectResult()
        {
            // Arrange
            var item1 = new Item() { Id = 1, MainMaterial = MainMaterialType.Alloy, MainColour = MainColourType.Red, Price = 10 };
            var item2 = new Item() { Id = 2, MainMaterial = MainMaterialType.Swarovski, MainColour = MainColourType.Blue, Price = 14 };
            var item3 = new Item() { Id = 3, MainMaterial = MainMaterialType.Swarovski, MainColour = MainColourType.Golden, Price = 15 };
            var item4 = new Item() { Id = 4, MainMaterial = MainMaterialType.Alloy, MainColour = MainColourType.Red, Price = 100 };
            var item5 = new Item() { Id = 5, MainMaterial = MainMaterialType.PinkGold, MainColour = MainColourType.Blue, Price = 250 };
            var item6 = new Item() { Id = 6, MainMaterial = MainMaterialType.Alloy, MainColour = MainColourType.Red, Price = 5 };

            var itemsCollection = new List<Item>() { item1, item2, item3, item4, item5, item6 };

            var queryData = itemsCollection.AsQueryable();

            this.itemsDbSet.As<IQueryable<Item>>().Setup(m => m.Provider).Returns(queryData.Provider);
            this.itemsDbSet.As<IQueryable<Item>>().Setup(m => m.Expression).Returns(queryData.Expression);
            this.itemsDbSet.As<IQueryable<Item>>().Setup(m => m.ElementType).Returns(queryData.ElementType);
            this.itemsDbSet.As<IQueryable<Item>>().Setup(m => m.GetEnumerator()).Returns(queryData.GetEnumerator());

            this.context.Setup(c => c.Set<Item>()).Returns(this.itemsDbSet.Object);

            this.repo = new EfGenericRepository<Item>(this.context.Object);

            // Act
            var actualFiltered = this.repo.GetAll(i => i.MainColour == MainColourType.Red).ToList();
            var expectedFiltered = new List<Item>() { item1, item4, item6 };

            // Assert
            CollectionAssert.AreEqual(actualFiltered, expectedFiltered);
        }

        [TestMethod]
        public void GetAll_OrderedByPrice_ShouldReturnCorrectResult()
        {
            // Arrange
            var item1 = new Item() { Id = 1, MainMaterial = MainMaterialType.Alloy, MainColour = MainColourType.Red, Price = 10 };
            var item2 = new Item() { Id = 2, MainMaterial = MainMaterialType.Swarovski, MainColour = MainColourType.Blue, Price = 14 };
            var item3 = new Item() { Id = 3, MainMaterial = MainMaterialType.Swarovski, MainColour = MainColourType.Golden, Price = 15 };
            var item4 = new Item() { Id = 4, MainMaterial = MainMaterialType.Alloy, MainColour = MainColourType.Red, Price = 1000 };
            var item5 = new Item() { Id = 5, MainMaterial = MainMaterialType.PinkGold, MainColour = MainColourType.Blue, Price = 250 };
            var item6 = new Item() { Id = 6, MainMaterial = MainMaterialType.Alloy, MainColour = MainColourType.Red, Price = 5 };

            var itemsCollection = new List<Item>() { item1, item2, item3, item4, item5, item6 };

            var queryData = itemsCollection.AsQueryable();

            this.itemsDbSet.As<IQueryable<Item>>().Setup(m => m.Provider).Returns(queryData.Provider);
            this.itemsDbSet.As<IQueryable<Item>>().Setup(m => m.Expression).Returns(queryData.Expression);
            this.itemsDbSet.As<IQueryable<Item>>().Setup(m => m.ElementType).Returns(queryData.ElementType);
            this.itemsDbSet.As<IQueryable<Item>>().Setup(m => m.GetEnumerator()).Returns(queryData.GetEnumerator());

            this.context.Setup(c => c.Set<Item>()).Returns(this.itemsDbSet.Object);

            this.repo = new EfGenericRepository<Item>(this.context.Object);

            // Act
            var actualFiltered = this.repo.GetAll(null, i => i.Price).ToList();
            var expectedFiltered = new List<Item>() { item6, item1, item2, item3, item5, item4 };

            // Assert
            CollectionAssert.AreEqual(actualFiltered, expectedFiltered);
        }
    }
}
