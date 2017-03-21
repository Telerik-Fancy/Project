using System;
using System.Text;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Fancy.Web.Controllers;
using System.Web.Mvc;
using Moq;

namespace Fancy.Tests.Web.Controllers
{
    /// <summary>
    /// Summary description for AccountControllerTests
    /// </summary>
    [TestClass]
    public class AccountControllerTests
    {
        [TestMethod]
        public void Login_ShouldReturnViewNotNull()
        {
            //Arange
            AccountController accController = new AccountController();

            //Act
            ViewResult viewResult = accController.Login(It.IsAny<string>()) as ViewResult;

            //Assert
            Assert.IsNotNull(viewResult);
        }

        [TestMethod]
        public void Register_ShouldReturnViewNotNull()
        {
            //Arange
            AccountController accController = new AccountController();

            //Act
            ViewResult viewResult = accController.Register() as ViewResult;

            //Assert
            Assert.IsNotNull(viewResult);
        }
    }
}
