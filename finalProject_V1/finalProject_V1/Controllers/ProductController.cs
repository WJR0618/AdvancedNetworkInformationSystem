using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using finalProject_V1.Models;

namespace finalProject_V1.Controllers
{
    public class ProductController : Controller
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
        public ActionResult Homepage()
        {
            using (Models.cartsEntities db = new Models.cartsEntities())
            {
                var result = (from pd in db.tProducts select pd).ToList();
                return View(result);
            }
        }

        // GET: Home
        public ActionResult Index()
        {
            var products = db.tProducts.OrderBy(m => m.fId).ToList();
            return View(products);
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(string fName, string fImage, int fPrice, string fDescription, int fQuantity, HttpPostedFileBase file)
        {
            tProducts product = new tProducts();
            //product.fId = fId;
            product.fName = fName;
            product.fImage = fImage;
            product.fPrice = fPrice;
            product.fDescription = fDescription;
            product.fQuantity = fQuantity;

            if (file != null && file.ContentLength > 0)
            {
                string fileName = Request.Form["fName"] + Path.GetExtension(file.FileName);
                product.fImage = fileName;
                var path = Path.Combine(Server.MapPath("~/Images"), fileName);
                file.SaveAs(path);
            }

            db.tProducts.Add(product);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        public ActionResult Edit(int id)
        {
            var product = db.tProducts.Where(m => m.fId == id).FirstOrDefault();
            return View(product);
        }

        [HttpPost]

        public ActionResult Edit(int id, string fName, string fImage, int fPrice, string fDescription, int fQuantity, HttpPostedFileBase file)
        {
            var product = db.tProducts.Where(m => m.fId == id).FirstOrDefault();
            string path = "";
            if (product.fImage != null)
                path = Path.Combine(Server.MapPath("~/Images"), product.fImage.ToString());

            product.fName = fName;
            product.fPrice = fPrice;
            product.fDescription = fDescription;
            product.fQuantity = fQuantity;

            /*如果有新的檔案上傳 就要把之前的刪掉再新增*/
            if (file != null && file.ContentLength > 0)
            {
                if (System.IO.File.Exists(path))
                    System.IO.File.Delete(path);

                string fileName = fName + Path.GetExtension(file.FileName);
                product.fImage = fileName;
                path = Path.Combine(Server.MapPath("~/Images"), fileName);
                file.SaveAs(path);
            }

            db.SaveChanges();

            return RedirectToAction("Index");
        }

        public ActionResult Delete(int id)
        {

            var product = db.tProducts.Where(m => m.fId == id).FirstOrDefault();
            var path = "";
            if (product.fImage != "")
            {
                path = Path.Combine(Server.MapPath("~/Images"), product.fImage);
            }

            if (System.IO.File.Exists(path))
            {
                System.IO.File.Delete(path);

            }

            db.tProducts.Remove(product);
            db.SaveChanges();

            return RedirectToAction("Index");
        }



        public ActionResult Details(int id)
        {
            var product = db.tProducts.Where(m => m.fId == id).FirstOrDefault();
            return View(product);
        }

        public ActionResult Buy(int id)
        {
            var product = db.tProducts.Where(m => m.fId == id).FirstOrDefault();
            return View(product);
        }

        [HttpPost]
        public ActionResult Buy1(string uId, string name, string phone, string addr)
        {
            return View();
        }
    }
}