using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SportsStore.Domain.Entities;
using System.Linq;
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

    }
}
