using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SportsStore.Domain.Entities;
using System.Linq;
using Moq;
using SportsStore.Domain.Abstract;
using SportsStore.WebUI.Controllers;
using System.Web.Mvc;
namespace SportsStore.UnitTests
{
    [TestClass]
    public class CartTest
    {
        [TestMethod]
        public void Add_New_Lines_To_Cart()
        {
            Product p1 = new Product { ProductID = 1, Name = "P1" };
            Product p2 = new Product { ProductID = 2, Name = "P2" };

            Cart target = new Cart();
            target.AddItem(p1,1);
            target.AddItem(p2, 1);
            CartLine[] res = target.Lines.ToArray();

            Assert.AreEqual(res.Length,2);
            Assert.AreEqual(res[0].Product, p1);
            Assert.AreEqual(res[1].Product, p2);
        }

        [TestMethod]
        public void Can_Clear_Contents()
        {
            Product p1 = new Product { ProductID = 1, Name = "P1" };
            Product p2 = new Product { ProductID = 2, Name = "P2" };

            Cart target = new Cart();
            target.AddItem(p1, 1);
            target.AddItem(p2, 1);
            target.Clear();
            
            Assert.AreEqual(target.Lines.Count(), 0);
        }

        [TestMethod]
        public void Calculate_Card_Total()
        {
            Product p1 = new Product { ProductID = 1, Name = "P1",Price=100 };
            Product p2 = new Product { ProductID = 2, Name = "P2",Price=150 };

            Cart target = new Cart();
            target.AddItem(p1, 1);
            target.AddItem(p2, 1);
            decimal res= target.ComputeTotalValue();

            Assert.AreEqual(res, 250);
        }
        [TestMethod]
        public void Cannot_Checkout_Empty_Cart() {
            Mock<IOrderProcessor> mock = new Mock<IOrderProcessor>();
            Cart cart = new Cart();
            ShippingDetails shippingDetails = new ShippingDetails();
            CartController target = new CartController(null,mock.Object);

            ViewResult result = target.Checkout(cart, shippingDetails);

            mock.Verify(m=>m.ProcessOrder(It.IsAny<Cart>(),It.IsAny<ShippingDetails>()),Times.Never());

            Assert.AreEqual("",result.ViewName);
            Assert.AreEqual(false, result.ViewData.ModelState.IsValid);
        }

        [TestMethod]
        public void Cannot_Checkout_Invalid_ShippingDetails() {
            Mock<IOrderProcessor> mock = new Mock<IOrderProcessor>();
            Cart cart = new Cart();
            cart.AddItem(new Product(),1);

            CartController target = new CartController(null, mock.Object);

            target.ModelState.AddModelError("error","error");

            ViewResult result = target.Checkout(cart,new ShippingDetails());

            mock.Verify(m=>m.ProcessOrder(It.IsAny<Cart>(),It.IsAny<ShippingDetails>()),Times.Never());

            Assert.AreEqual("",result.ViewName);
            Assert.AreEqual(false,result.ViewData.ModelState.IsValid);
        }

        [TestMethod]
        public void Can_Checkout_And_Submit_Order()
        {
            // Arrange - create a mock order processor   
            Mock<IOrderProcessor> mock = new Mock<IOrderProcessor>();
            // Arrange - create a cart with an item   
            Cart cart = new Cart();
            cart.AddItem(new Product(), 1);
            // Arrange - create an instance of the controller   
            CartController target = new CartController(null, mock.Object);
            // Act - try to checkout   
            ViewResult result = target.Checkout(cart, new ShippingDetails());
            // Assert - check that the order has been passed on to the processor 
            mock.Verify(m =>
                m.ProcessOrder(It.IsAny<Cart>(), It.IsAny<ShippingDetails>()), Times.Once());
            // Assert - check that the method is returning the Completed view   
            Assert.AreEqual("Completed", result.ViewName);
            // Assert - check that we are passing a valid model to the view   
            Assert.AreEqual(true, result.ViewData.ModelState.IsValid);
        }
    }
}
