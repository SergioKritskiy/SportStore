using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SportsStore.Domain.Abstract;
using SportsStore.Domain.Entities;
using SportsStore.WebUI.Controllers;
using System.Linq;
using System.Web.Mvc;
using Moq;

namespace SportsStore.UnitTests
{
    [TestClass]
    public class ImageTests
    {
        [TestMethod]
        public void Can_Retrive_Image_Data()
        {
            Product prod = new Product
            {
                ProductID = 2,
                Name = "Test",
                ImageData = new byte[] { },
                ImageMineType = "image/png"
            };
            Mock<IProductRepository> mock = new Mock<IProductRepository>();
            mock.Setup(m => m.Products).Returns(new Product []{
            new Product{ ProductID=1, Name="p1"},
            prod,
            new Product{ProductID =3,Name="p3"}
            }.AsQueryable());

            ProductController target = new ProductController(mock.Object);
            ActionResult result = target.GetImage(2);
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(FilePathResult));
            Assert.AreEqual(prod.ImageMineType,((FileResult)result).ContentType);
        }
        [TestMethod]
        public void Cennot_Retrive_Image_Data_For_Invalid_Id() {
            Mock<IProductRepository> mock = new Mock<IProductRepository>();
            mock.Setup(m => m.Products).Returns(new Product[] {
                        new Product{ ProductID=1, Name="p1"},
            new Product{ProductID =2,Name="p2"}
            }.AsQueryable());

            ProductController target = new ProductController(mock.Object);

            ActionResult result = target.GetImage(100);
            Assert.IsNull(result);
        }

    }
}
