using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AnimeShop.DAL;
using AnimeShop.Models;

using System.Data.Entity;

namespace AnimeShop.Controllers {

    public class ProductModelController : Controller {


        private AnimeShopContext db = new AnimeShopContext();

        public ActionResult Index() {

            return RedirectToAction("Catalog");
        }


        
        public ActionResult Catalog(int cat = 0, string search = "") {

            ViewData["search"] = search;
            ViewData["cat"] = cat;

            search = search.Trim().ToLower();
           
            var tempProd = db.Products.Where(s => s.Name.ToLower().Contains(search) || s.Anime.ToLower().Contains(search) || s.Manufacturer.ToLower().Contains(search)).Include(s => s.Category);
            
            if (cat != 0)  tempProd = tempProd.Where(s => s.CategoryModelId == cat);

            tempProd = tempProd.Where(s => s.Active);

            ViewData["products"] = tempProd.OrderBy(s => s.Name);
            ViewData["categories"] = db.Categories.OrderBy(s => s.Name);

            

            return View();
        }

        
        public ActionResult Details(int? id) {

            if (id == null) {

                return RedirectToAction("Catalog");
            }
            ProductModel selectedProduct = db.Products.Find(id);

            if (selectedProduct == null) {
                return HttpNotFound();
            }

            int mediaLength = System.IO.Directory.GetFiles(AppDomain.CurrentDomain.BaseDirectory + "Content/Media/" + selectedProduct.Media, "*.jpg", System.IO.SearchOption.AllDirectories).Length;
            ViewData["mediaLength"] = mediaLength;

            return View(selectedProduct);
        }


        [Authorize (Roles = "admin")]
        public ActionResult DetailsFull(int? id) {

            if (id == null) {

                return RedirectToAction("Catalog");
            }
            ProductModel selectedProduct = db.Products.Find(id);

            if (selectedProduct == null) {
                return HttpNotFound();
            }

            return View(selectedProduct);
        }


        
        [HttpPost]
        public ActionResult Details(int id, int amount) {

            ProductModel selectedProduct = db.Products.Find(id);

            if (selectedProduct == null || !selectedProduct.Active)
            {
                return HttpNotFound();
            }

            MyServer.addToCart(Session, id, amount);
            

            return RedirectToAction("Details", "ProductModel", id);
        }

        [Authorize(Roles = "admin")]
        public ActionResult Create() {

            ViewBag.CategoryModelId = new SelectList(db.Categories, "CategoryModelId", "Name");

            return View();
        }

        [Authorize(Roles = "admin")]
        [HttpPost]
        public ActionResult Create([Bind(Include = "Name, CategoryModelId, Anime, Size, Manufacturer, Price, Description, Media, Active")] ProductModel newProduct) {

            ViewBag.CategoryModelId = new SelectList(db.Categories, "CategoryModelId", "Name");

            if (ModelState.IsValid) {

                db.Products.Add(newProduct);
                db.SaveChanges();
                return RedirectToAction("Index", "Manage");
            }
            return View(newProduct);
        }

        [Authorize(Roles = "admin")]
        public ActionResult Edit(int? id) {

            if (id == null) {

                return RedirectToAction("Index", "Manage");
            }

            ProductModel product = db.Products.Find(id);

            if (product == null) {

                return HttpNotFound();
            }

            ViewBag.CategoryModelId = new SelectList(db.Categories, "CategoryModelId", "Name", product.Category.CategoryModelId);

            return View(product);
        }

        [Authorize(Roles = "admin")]
        [HttpPost]
        public ActionResult Edit(int? id, [Bind(Include = "Name, CategoryModelId, Anime, Size, Manufacturer, Price, Description, Media, Active")] ProductModel newProduct) {

            ViewBag.CategoryModelId = new SelectList(db.Categories, "CategoryModelId", "Name");

            if (id == null) {

                return RedirectToAction("Index", "Manage");
            }

            ProductModel product = db.Products.Find(id);

            if (product == null) {

                return HttpNotFound();
            }


            if (ModelState.IsValid) {

                product.Name =              newProduct.Name;
                product.CategoryModelId =   newProduct.CategoryModelId;
                product.Anime =             newProduct.Anime;
                product.Size =              newProduct.Size;
                product.Manufacturer =      newProduct.Manufacturer;
                product.Price =             newProduct.Price;
                product.Description =       newProduct.Description;
                product.Media =             newProduct.Media;
                product.Active =            newProduct.Active;

                db.SaveChanges();
                return RedirectToAction("Index", "Manage");
            }

            return View(newProduct);
        }


        [Authorize(Roles = "admin")]
        public ActionResult Delete(int? id, bool confirm = false) {

            if (id == null) {

                return RedirectToAction("Index", "Manage");
            }

            ProductModel product = db.Products.Find(id);

            if (product == null) {

                return HttpNotFound();
            }

            if (confirm) {

                product.Active = false;

                db.SaveChanges();
                return RedirectToAction("Index", "Manage");

            } else {

                return View(product);
            }
        }
    }
}
