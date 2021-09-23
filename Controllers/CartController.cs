using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AnimeShop.DAL;
using AnimeShop.Models;
using System.Data.Entity;

using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using System.Data.Entity.Validation;

namespace AnimeShop.Controllers {

    public class CartController : Controller {


        private AnimeShopContext db = new AnimeShopContext();

        private Dictionary<ProductModel, int> cartToProductCart (HttpSessionStateBase session) {

            var cart = MyServer.GetCart(session);

            Dictionary<ProductModel, int> productCart = null;

            if (cart.Count > 0) {

                productCart = new Dictionary<ProductModel, int>();

                List<int> archivedProducts = new List<int>();

                foreach (int key in cart.Keys) {

                    ProductModel selectedProduct = db.Products.Find(key);

                    if (selectedProduct == null) {

                        archivedProducts.Add(key);

                    } else {

                        if (selectedProduct.Active) {

                            productCart[selectedProduct] = cart[key];

                        } else {

                            archivedProducts.Add(key);
                        } 
                    }
                }

                foreach (var archId in archivedProducts) {
                    MyServer.removeFromCart(session, archId);
                }

                if (cart.Count == 0) productCart = null;
            }

            return productCart;
        }

        // GET: Cart
        public ActionResult Index(string message = "") {

            Dictionary<ProductModel, int> productCart = cartToProductCart(Session);
            double productCartSum = 0.0;

            if (productCart != null) {
                foreach (ProductModel product in productCart.Keys) {

                    productCartSum += product.Price * productCart[product];
                }
            }

            ViewData["productCartSum"] = productCartSum;
            ViewData["productCart"] = productCart;

            ViewData["shipments"] = db.Shipments.Where(s => s.Active);
            ViewData["message"] = message;

            return View();
        }


        public ActionResult Delete(int id) {
            
            MyServer.removeFromCart(Session, id);

            return RedirectToAction("Index", "Cart");
        }



        [Authorize]
        public ActionResult Order(FormCollection collection) {


            var userId = User.Identity.GetUserId();
            var customerListTemp = db.Customers.Where(c => c.UserId == userId);
            CustomerModel customer = null;
            if (customerListTemp.Count() > 0) customer = customerListTemp.First();

            if (customer == null) { return RedirectToAction("Create", "CustomerModel"); }


            // User ma dane osobowe

            String info = null;
            int shipment_id = 0;

            ValueProviderResult infoT = collection.GetValue("info");
            ValueProviderResult shipment_idT = collection.GetValue("shipment_id");

            if (infoT == null || shipment_idT == null) return RedirectToAction("Index", new { message = "Błędne dane" });

            info = infoT.ConvertTo(typeof(String)) as String;
            shipment_id = (int)shipment_idT.ConvertTo(typeof(Int32));

            var shipmentListTemp = db.Shipments.Where(s => s.ShipmentModelId == shipment_id)/*.AsNoTracking()*/;
            ShipmentModel shipment = null;
            if (shipmentListTemp.Count() > 0) shipment = shipmentListTemp.First();

            if (shipment == null || !shipment.Active) {

                return RedirectToAction("Index", new { message = "Wybrana dostawa nie jest dostępna" });
            }

            double productCartSum = shipment.Cost;

            Dictionary<ProductModel, int> productCart = cartToProductCart(Session);
                
            if (productCart != null) {
                foreach (ProductModel product in productCart.Keys) {

                    productCartSum += product.Price * productCart[product];
                }
            }


            string shipmentInfo = customer.FirstName + " " + customer.LastName + "\n"
                + User.Identity.GetUserName()
                + "\n" + customer.PhoneNumber
                + "\n" + customer.Street + " " + customer.Building + " " + customer.Apartment
                + "\n" + customer.ZipCode + " " + customer.City;

             
            ViewData["productCartSum"] = productCartSum;
            ViewData["productCart"] = productCart;

            ViewData["shipment"] = shipment;
            ViewData["shipmentInfo"] = shipmentInfo;
            ViewData["additionalInfo"] = info == null || info == "" ? "brak" : info;

            return View();
        }



        [HttpPost]
        [Authorize]
        public ActionResult ConfirmOrder (FormCollection collection) {

            var userId = User.Identity.GetUserId();
            var customerListTemp = db.Customers.Where(c => c.UserId == userId);
            CustomerModel customer = null;
            if (customerListTemp.Count() > 0) customer = customerListTemp.First();

            if (customer == null) { return RedirectToAction("Create", "CustomerModel"); }


            string shipmentInfo = customer.FirstName + " " + customer.LastName + "\n"
               + User.Identity.GetUserName()
               + "\n" + customer.PhoneNumber
               + "\n" + customer.Street + " " + customer.Building + " " + customer.Apartment
               + "\n" + customer.ZipCode + " " + customer.City;



            String info = null;
            int shipment_id = 0;
            double productCartSumF = 0.0;
 

            ValueProviderResult infoT = collection.GetValue("additionalInfo");
            ValueProviderResult shipment_idT = collection.GetValue("shipment_id");
            ValueProviderResult productCartSumFT = collection.GetValue("cartValue");

            if (infoT == null || shipment_idT == null || productCartSumFT == null) return RedirectToAction("Index", new { message = "Błędne dane" });

            info = infoT.ConvertTo(typeof(String)) as String;
            shipment_id = (int)shipment_idT.ConvertTo(typeof(Int32));
            productCartSumF = (double)productCartSumFT.ConvertTo(typeof(Double));

            var shipmentListTemp = db.Shipments.Where(s => s.ShipmentModelId == shipment_id)/*.AsNoTracking()*/;
            ShipmentModel shipment = null;
            if (shipmentListTemp.Count() > 0) shipment = shipmentListTemp.First();

            if (shipment == null || !shipment.Active) {

                return RedirectToAction("Index", new { message = "Wybrana dostawa nie jest dostępna" });
            }

            double productCartSum = shipment.Cost;

            Dictionary<ProductModel, int> productCart = cartToProductCart(Session);

            List<OrderListModel> orderlist = new List<OrderListModel>();

            if (productCart != null) {

                foreach (ProductModel product in productCart.Keys) {

                    productCartSum += product.Price * productCart[product];
                    OrderListModel tempOrderItem = new OrderListModel();
                    tempOrderItem.Product = product;
                    tempOrderItem.Price = product.Price;
                    tempOrderItem.Amount = productCart[product];
                    orderlist.Add(tempOrderItem);
                }

            } else {

                return RedirectToAction("Index", new { message = "Produkty nie są dostępne" });
            }

            //System.Diagnostics.Debug.WriteLine(productCartSum + " | " + productCartSumF);
            if (Math.Abs(productCartSum - productCartSumF) > 0.01) return RedirectToAction("Index", new { message = "Jeden lub więcej produktów nie jest dostępnych" });

            var statusTemp = db.OrderStatuses.Where(s => s.Name == "Przygotowywanie zamówienia");
            OrderStatusModel status = statusTemp != null && statusTemp.Count() > 0 ? statusTemp.First() : null;

            if (status == null) return RedirectToAction("Index", new { message = "Baza danych jest niedostępna. Skontaktuj się z administratorem" });

            // powodzenie
            OrderModel newOrder = new OrderModel();
            newOrder.Customer = customer;
            newOrder.ShipmentInfo = shipmentInfo;
            newOrder.Shipment = shipment;
            newOrder.OrderLists = orderlist;
            newOrder.AdditionalInfo = info;
            newOrder.Status = status;
            newOrder.Date = DateTime.Now;

            db.Orders.Add(newOrder);

            db.SaveChanges();

            MyServer.cleanCart(Session);
            return RedirectToAction("Index", "Manage");
        }
    }
}