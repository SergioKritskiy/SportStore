using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SportsStore.Domain.Abstract;
using SportsStore.Domain.Entities;
using SportsStore.WebUI.Models;

namespace SportsStore.WebUI.Controllers
{
    public class CartController : Controller
    {
        private IProductRepository repository;
        public CartController(IProductRepository repo) {
            repository = repo;
        }
        public RedirectToRouteResult AddToCart(Cart cart,int productId,string returnUrl) {
            Product prod = repository.Products.FirstOrDefault(p => p.ProductID == productId);
            if (prod!=null){
                cart.AddItem(prod, 1);
            }
            return RedirectToAction("Index", new { returnUrl });
        }

        public RedirectToRouteResult RemoveFromCart(Cart cart,int productId, string returnUrl) {
        
            Product prod = repository.Products.FirstOrDefault(p=>p.ProductID==productId);
            if (prod!=null){
            cart.RemoveLine(prod);
            }
            return RedirectToAction("Index", new { returnUrl });
        }

        private Cart GetCard() {
            Cart cart = (Cart)Session["Cart"];
            if (cart==null){
                cart = new Cart();
                Session["Cart"] = cart;
            }
            return cart;
        }
        public ViewResult Index(Cart cart ,string returnUrl) {
            return View(new CartIndexViewModel
            {
                Cart = cart,
                ReturnUrl = returnUrl
            });
        }

        public PartialViewResult Summary(Cart cart) {
            return PartialView(cart);
        }

        public ViewResult Checkout() {
            return View(new ShippingDetails());
        }

    }
}
