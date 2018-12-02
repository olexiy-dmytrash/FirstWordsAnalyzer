using System;
using System.Threading.Tasks;
using System.Web.Mvc;
using FirstWordsAnalyzer.Controllers;
using FirstWordsAnalyzer.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using FirstWordsAnalyzer.Interfaces;
using System.Collections.Generic;

namespace FirstWordsAnalyzer.Tests.Controllers
{
    [TestClass]
    public class BooksControllerTest
    {
        [TestMethod]
        public void Index()
        {
            // Arrange
            var mock = new Mock<IRepositoryAsynk<Book>>();

            var myVar = Task.Run(() =>
            {
                return new List<Book>();
            });
            mock.Setup(a => a.GetAllAsynk()).Returns(myVar);
            BooksController controller = new BooksController(mock.Object);

            // Act
            ViewResult result = controller.Index().Result as ViewResult;

            // Assert
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void Details()
        {
            // Arrange
            var mock = new Mock<IRepositoryAsynk<Book>>();

            var myVar = Task.Run(() =>
            {
                return new Book() { Id = 1, Author = "Test", Name = "Test", PublishingYear = DateTime.Now};
            });
            mock.Setup(a => a.GetAsynk(1)).Returns(myVar);
            BooksController controller = new BooksController(mock.Object);

            // Act
            ViewResult result = controller.Details(1).Result as ViewResult;

            // Assert
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void Create()
        {
            // Arrange
            var mock = new Mock<IRepositoryAsynk<Book>>();

            var newBook = new Book() { Author = "Test", Name = "Test", PublishingYear = DateTime.Now };
            
            var myTask = Task.Run(() =>
            {
                return newBook;
            });
            mock.Setup(a => a.Create(newBook)).Returns(myTask);
            BooksController controller = new BooksController(mock.Object);

            // Act
            RedirectToRouteResult result = controller.Create(newBook).Result as RedirectToRouteResult;

            // Assert
            Assert.IsNotNull(result);
        }
    }
}
