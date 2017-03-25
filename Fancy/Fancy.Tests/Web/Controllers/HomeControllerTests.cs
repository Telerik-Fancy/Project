using System.Web.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Fancy.Web.Controllers;

namespace Fancy.Tests.Web.Controllers
{
    [TestClass]
    public class HomeControllerTests
    {
        [TestMethod]
        public void HomePage_ShouldReturnViewNotNull()
        {
            //Arange
            var homeController = new HomeController();

            //Act
            var viewResult = homeController.HomePage() as ViewResult;

            //Assert
            Assert.IsNotNull(viewResult);
        }
    }
}
