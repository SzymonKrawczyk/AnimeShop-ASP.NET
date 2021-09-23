using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AnimeShop.DAL;
using AnimeShop.Models;

namespace AnimeShop.Controllers
{
    [Authorize (Roles = "admin")]
    public class CategoryModelController : Controller
    {

        private AnimeShopContext db = new AnimeShopContext();


        public ActionResult Create() {

            return View();
        }

        [HttpPost]
        public ActionResult Create([Bind(Include = "Name")] CategoryModel newCategory) {

            if (ModelState.IsValid) {

                db.Categories.Add(newCategory);
                db.SaveChanges();
                return RedirectToAction("Index", "Manage");
            }
            return View(newCategory);

        }

        public ActionResult Edit(int? id) {

            if (id == null) {

                return RedirectToAction("Index", "Manage");
            }

            CategoryModel category = db.Categories.Find(id);

            if (category == null) {

                return HttpNotFound();
            }

            return View(category);
        }

        [HttpPost]
        public ActionResult Edit(int? id, [Bind(Include = "Name")] CategoryModel newCategory) {

            if (id == null) {

                return RedirectToAction("Index", "Manage");
            }

            CategoryModel category = db.Categories.Find(id);
            if (category == null) {

                return HttpNotFound();
            }

            if (ModelState.IsValid) {

                category.Name = newCategory.Name;

                db.SaveChanges();
                return RedirectToAction("Index", "Manage");
            }

            return View(newCategory);
        }



        public ActionResult Delete(int? id, bool confirm = false) {


            if (id == null) {

                return RedirectToAction("Index", "Manage");
            }

            CategoryModel category = db.Categories.Find(id);
            if (category == null) {

                return HttpNotFound();
            }

            if (confirm) {

                var prodWCat = category.Products;

                foreach (var item in prodWCat) {
                    item.CategoryModelId = null;
                }

                db.Categories.Remove(category);

                db.SaveChanges();
                return RedirectToAction("Index", "Manage");

            } else {

                return View(category);
            }
        }
    }
}