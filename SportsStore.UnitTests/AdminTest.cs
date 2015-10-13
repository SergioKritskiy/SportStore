using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SportsStore.Domain.Entities;
using SportsStore.Domain.Abstract;
using Moq;
using System.Web.Mvc;
using System.Linq;
using SportsStore.WebUI.Controllers;
namespace SportsStore.UnitTests
{
    [TestClass]
    public class AdminTest
    {
        [TestMethod]
        public void Can_Edit_Product()
        {
            Mock<IProductRepository> mock = new Mock<IProductRepository>();
            mock.Setup(m => m.Products).Returns(new Product[]{
            new Product{ProductID=1,Name="p1"},
            new Product{ProductID=2,Name="p2"},
            new Product{ProductID=3,Name="p3"},
            }.AsQueryable());
            AdminController target = new AdminController(mock.Object);

            Product p1 = target.Edit(1).ViewData.Model as Product;
            Product p2 = target.Edit(2).ViewData.Model as Product;
            Product p3 = target.Edit(3).ViewData.Model as Product;

            Assert.AreEqual(1, p1.ProductID);
            Assert.AreEqual(2, p2.ProductID);
            Assert.AreEqual(3, p3.ProductID);
        }

        [TestMethod]
        public void Can_Edit_Nonexistint_Product()
        {
            Mock<IProductRepository> mock = new Mock<IProductRepository>();
            mock.Setup(m => m.Products).Returns(new Product[]{
            new Product{ProductID=1,Name="p1"},
            new Product{ProductID=2,Name="p2"},
            new Product{ProductID=3,Name="p3"},
            }.AsQueryable());
            AdminController target = new AdminController(mock.Object);

            Product result = target.Edit(4).ViewData.Model as Product;
            Assert.IsNull(result);
            }
    }
}
