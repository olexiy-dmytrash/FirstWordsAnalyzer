using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Web.Mvc;
using FirstWordsAnalyzer.Controllers;
using FirstWordsAnalyzer.Models;
using FirsWordsAnalyzer.DAL.Interfaces;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace FirstWordsAnalyzer.Tests.Controllers
{
    [TestClass]
    public class WordsPopularityWithCognatesTest
    {
        [TestMethod]
        public void Index()
        {
            // Arrange
            var mock = new Mock<IRepository<WordsPopularityWithCognates>>();
            mock.Setup(a => a.GetAll()).Returns(new List<WordsPopularityWithCognates>());
            WordsPopularityWithCognatesController controller = new WordsPopularityWithCognatesController(mock.Object);

            // Act
            ViewResult result = controller.Index(null) as ViewResult;

            // Assert
            Assert.IsNotNull(result);
        }
    }
}
