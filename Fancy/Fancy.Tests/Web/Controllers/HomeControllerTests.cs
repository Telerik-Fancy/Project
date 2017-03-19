using Fancy.Web.Controllers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Web.Mvc;

namespace Fancy.Tests.Web.Controllers
{
    [TestClass]
    public class HomeControllerTests
    {
        [TestMethod]
        public void Index_ShouldReturnViewNotNull()
        {
            //Arange
            HomeController homeController = new HomeController();

            //Act
            ViewResult viewResult = homeController.HomePage() as ViewResult;

            //Assert
            Assert.IsNotNull(viewResult);
        }
    }
}
