using Fancy.Web.Controllers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Web.Mvc;

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
