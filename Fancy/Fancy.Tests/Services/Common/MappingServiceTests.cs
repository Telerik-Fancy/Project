using System;
using System.Text;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Fancy.Services.Common;
using AutoMapper;
using Moq;

namespace Fancy.Tests.Services.Common
{
    [TestClass]
    public class MappingServiceTests
    {
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Constructor_ShouldThrowArgumentNullException_WhenMapperIsNull()
        {
            // Arrange, Act & Assert
            var mappingService = new MappingService(null);
        }
    }
}
