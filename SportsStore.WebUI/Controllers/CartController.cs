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
        public RedirectToRouteResult AddToCart(int productId,string returnUrl) {
            Product prod = repository.Products.FirstOrDefault(p => p.ProductID == productId);
            if (prod!=null){
                GetCard().AddItem(prod, 1);
            }
            return RedirectToAction("Index", new { returnUrl });
        }

        public RedirectToRouteResult RemoveFromCart(int productId, string returnUrl) {
        
            Product prod = repository.Products.FirstOrDefault(p=>p.ProductID==productId);
            if (prod!=null){
            GetCard().RemoveLine(prod);
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
        public ViewResult Index(string returnUrl) {
            return View(new CartIndexViewModel
            {
                Cart = GetCard(),
                ReturnUrl = returnUrl
            });
        }
    }
}
