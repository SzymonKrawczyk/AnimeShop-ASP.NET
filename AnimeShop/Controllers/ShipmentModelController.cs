using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AnimeShop.DAL;
using AnimeShop.Models;

using System.Data.Entity;

namespace AnimeShop.Controllers {

    [Authorize(Roles = "admin")]
    public class ShipmentModelController : Controller {

        private AnimeShopContext db = new AnimeShopContext();


        public ActionResult Create() {

            return View();
        }

        [HttpPost]
        public ActionResult Create([Bind(Include = "Name, Cost, Active")] ShipmentModel newShipment) {

            if (ModelState.IsValid) {

                db.Shipments.Add(newShipment);
                db.SaveChanges();
                return RedirectToAction("Index", "Manage");
            }
            return View(newShipment);
        }

        public ActionResult Edit(int? id) {

            if (id == null) {

                return RedirectToAction("Index", "Manage");
            }

            ShipmentModel shipment = db.Shipments.Find(id);

            if (shipment == null) {

                return HttpNotFound();
            }

            return View(shipment);
        }

        [HttpPost]
        public ActionResult Edit(int? id, [Bind(Include = "Name, Active")] ShipmentModel newShipment) {

            if (id == null) {

                return RedirectToAction("Index", "Manage");
            }

            ShipmentModel shipment = db.Shipments.Find(id);

            if (newShipment == null) {

                return HttpNotFound();
            }

            if (ModelState.IsValid) {

                shipment.Name = newShipment.Name;
                shipment.Active = newShipment.Active;

                db.SaveChanges();
                return RedirectToAction("Index", "Manage");
            }

            return View(newShipment);
        }

        public ActionResult Delete(int? id, bool confirm = false) {

            if (id == null) {

                return RedirectToAction("Index", "Manage");
            }

            ShipmentModel shipment = db.Shipments.Find(id);

            if (shipment == null) {

                return HttpNotFound();
            }

            if (confirm) {

                shipment.Active = false;

                db.SaveChanges();
                return RedirectToAction("Index", "Manage");

            } else {

                return View(shipment);
            }
        }
    }
}