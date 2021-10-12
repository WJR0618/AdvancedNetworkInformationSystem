using finalProject_V1.Models;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;

namespace finalProject_V1.Controllers
{
    public class HomeController : Controller
    {
        cartsEntities db = new cartsEntities();

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
        // GET: Home
        public ActionResult Homepage()
        {
            using (Models.cartsEntities db = new Models.cartsEntities())
            {
                var result = (from pd in db.tProducts select pd).ToList();
                return View(result);
            }
        }

        public ActionResult Index()
        {

            using (Models.cartsEntities db = new Models.cartsEntities())
            {
                var result = (from pd in db.tProducts select pd).ToList();
                return View(result);
            }
        }


        public ActionResult Details(int id)
        {
            var product = db.tProducts.Where(m => m.fId == id).FirstOrDefault();
            return View(product);
        }

        public ActionResult AddintoCart(int id)
        {
            var product = db.tProducts.Where(m => m.fId == id).FirstOrDefault();
            return View(product);
        }

        [HttpPost]
        public ActionResult AddintoCart(int id, string fQuantity)
        {
            tCart cart = new tCart();
            var product = db.tProducts.Where(m => m.fId == id).FirstOrDefault();
            cart.fUserId = User.Identity.GetUserId();
            cart.fPdImage = product.fImage;
            cart.fPdName = product.fName;
            cart.fPdDescription = product.fDescription;
            cart.fPdPrice = product.fPrice * Convert.ToInt32(fQuantity);
            cart.fBuyQuantity = Convert.ToInt32(fQuantity);
            product.fQuantity -= Convert.ToInt32(fQuantity);
            db.tCart.Add(cart);
            db.SaveChanges();

            return RedirectToAction("ShowCarts");
        }

        public ActionResult DeleteCartItem(int id)
        {

            var cart = db.tCart.Where(m => m.fId == id).FirstOrDefault();
            var product = db.tProducts.Where(m => m.fName == cart.fPdName).FirstOrDefault();
            product.fQuantity += cart.fBuyQuantity;
            db.tCart.Remove(cart);
            db.SaveChanges();

            return RedirectToAction("ShowCarts");
        }


        public ActionResult ShowCarts()
        {
            var cart = db.tCart.OrderBy(m => m.fId).ToList();
            return View(cart);
        }

        [HttpPost]
        public ActionResult PayMoney(string Name, string Phone, string Address, string Payway)
        {
            tOrder order = new tOrder();
            var carts = db.tCart.ToList();
            foreach (var item in carts)
            {
                if (User.Identity.GetUserId() == item.fUserId)
                {
                    order.fBuyQuantity = item.fBuyQuantity;
                    order.fPdDescription = item.fPdDescription;
                    order.fPdImage = item.fPdImage;
                    order.fPdName = item.fPdName;
                    order.fPdPrice = item.fPdPrice;
                    order.fUserId = User.Identity.GetUserId();
                    order.fUserName = Name;
                    order.fUserPhone = Phone;
                    order.fUserAddress = Address;
                    order.fUserPayway = Payway;
                    db.tOrder.Add(order);
                    db.tCart.Remove(item);
                    db.SaveChanges();
                }
            }


            return RedirectToAction("BuyList");
        }

        public ActionResult BuyList()
        {
            var order = db.tOrder.OrderBy(m => m.fId).ToList();
            return View(order);
        }


    }
}