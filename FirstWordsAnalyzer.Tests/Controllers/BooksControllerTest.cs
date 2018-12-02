using System;
using System.Threading.Tasks;
using System.Web.Mvc;
using FirstWordsAnalyzer.Controllers;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FirstWordsAnalyzer.Tests.Controllers
{
    [TestClass]
    public class BooksControllerTest
    {
        [TestMethod]
        public void Index()
        {
            // Arrange
            BooksController controller = new BooksController();

            // Act
            ViewResult result = controller.Index().Result as ViewResult;

            // Assert
            Assert.IsNotNull(result);
        }
    }
}
