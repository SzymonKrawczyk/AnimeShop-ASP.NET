using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AnimeShop.DAL;
using AnimeShop.Models;

namespace AnimeShop.Controllers {

    [Authorize (Roles = "admin")]
    public class OrderController : Controller {

        private AnimeShopContext db = new AnimeShopContext();


        public ActionResult ConfirmSendOrder(int id) {

            OrderModel order = db.Orders.Find(id);
            var newStatusListTemp = db.OrderStatuses.Where(os => os.Name == "Wysłano");

            if (order == null) {

                return HttpNotFound();
            }

            if (newStatusListTemp == null || newStatusListTemp.Count() == 0) {

                return HttpNotFound();
            }

            order.Status = newStatusListTemp.First();
            db.SaveChanges();

            return RedirectToAction("Index", "Manage");
        }
    }
}