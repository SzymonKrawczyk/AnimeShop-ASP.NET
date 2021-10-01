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

            Session["newOrder"] = null;

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
        public ActionResult Order(FormCollection collection = null, bool confirm = false) {


            var userId = User.Identity.GetUserId();
            var customerListTemp = db.Customers.Where(c => c.UserId == userId);
            CustomerModel customer = null;
            if (customerListTemp.Count() > 0) customer = customerListTemp.First();

            if (customer == null) { return RedirectToAction("Create", "CustomerModel"); }


            // User ma dane osobowe
            if (!confirm) {

                if (collection == null) { RedirectToAction("Index", new { message = "Błąd" }); }

                String info = null;
                int shipment_id = 0;

                //System.Diagnostics.Debug.WriteLine(collection.GetValue("info").ConvertTo(typeof(String)));
                //System.Diagnostics.Debug.WriteLine(collection.GetValue("shipment_id").ConvertTo(typeof(Int32)));

                ValueProviderResult infoT = collection.GetValue("info");
                ValueProviderResult shipment_idT = collection.GetValue("shipment_id");

                if (infoT == null || shipment_idT == null) return RedirectToAction("Index", new { message = "Błędne dane" });

                info = infoT.ConvertTo(typeof(String)) as String;
                shipment_id = (int)shipment_idT.ConvertTo(typeof(Int32));

                var shipmentListTemp = db.Shipments.Where(s => s.ShipmentModelId == shipment_id);
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
                }

                var statusTemp = db.OrderStatuses.Where(s => s.Name == "Przygotowywanie zamówienia");
                OrderStatusModel status = statusTemp != null && statusTemp.Count() > 0 ? statusTemp.First() : null;

                string shipmentInfo = customer.FirstName + " " + customer.LastName + "\n"
                    + User.Identity.GetUserName()
                    + "\n" + customer.PhoneNumber
                    + "\n" + customer.Street + " " + customer.Building + " " + customer.Apartment
                    + "\n" + customer.ZipCode + " " + customer.City;

                OrderModel newOrder = new OrderModel();
                newOrder.Customer = customer;
                newOrder.ShipmentInfo = shipmentInfo;
                newOrder.Shipment = shipment;
                newOrder.OrderLists = orderlist;
                newOrder.AdditionalInfo = info;
                newOrder.Status = status;

                ViewData["productCartSum"] = productCartSum;
                Session["newOrder"] = newOrder;
                return View();

            } else {
                //System.Diagnostics.Debug.WriteLine(confirm);

                OrderModel newOrderC = Session["newOrder"] as OrderModel;

                if (newOrderC == null) RedirectToAction("Index", new { message = "Błąd" });

                // update
                string shipmentInfo = customer.FirstName + " " + customer.LastName + "\n"
                    + User.Identity.GetUserName()
                    + "\n" + customer.PhoneNumber
                    + "\n" + customer.Street + " " + customer.Building + " " + customer.Apartment
                    + "\n" + customer.ZipCode + " " + customer.City;
                newOrderC.ShipmentInfo = shipmentInfo;

                var shipmentListTemp = db.Shipments.Where(s => s.ShipmentModelId == newOrderC.Shipment.ShipmentModelId);
                ShipmentModel shipment = null;
                if (shipmentListTemp.Count() > 0) shipment = shipmentListTemp.First();

                if (shipment == null || !shipment.Active) {

                    return RedirectToAction("Index", new { message = "Wybrana dostawa nie jest dostępna" });
                }


                Dictionary<ProductModel, int> productCart = cartToProductCart(Session);

                List<OrderListModel> orderlist = new List<OrderListModel>();

                if (productCart != null) {

                    foreach (OrderListModel item in newOrderC.OrderLists) {

                        bool found = false;
                        foreach (ProductModel product in productCart.Keys) {

                            if (item.Product.ProductModelId == product.ProductModelId) {
                                found = true;
                                break;
                            }
                        }
                        if (!found) return RedirectToAction("Index", new { message = "Jeden lub więcej produktów jest niedostępnych" });
                    }
                } else {
                    return RedirectToAction("Index", new { message = "Produkty niedostępne" });
                }


                OrderModel tempOrder = new OrderModel();
                tempOrder.Date = DateTime.Now;
                tempOrder.Customer = newOrderC.Customer;
                tempOrder.ShipmentInfo = newOrderC.ShipmentInfo;
                tempOrder.Shipment = newOrderC.Shipment;
                //tempOrder.OrderLists = new List<OrderListModel>();
                tempOrder.AdditionalInfo = newOrderC.AdditionalInfo;
                tempOrder.Status = newOrderC.Status;

                // powodzenie
                foreach (OrderListModel item in newOrderC.OrderLists) {

                    OrderListModel tempItem = new OrderListModel();
                    tempItem.Product = item.Product;
                    tempItem.Price = item.Price;
                    tempItem.Amount = item.Amount;
                    tempItem.ParentOrder = tempOrder;
                    //db.OrderLists.Add(tempItem);

                    //tempOrder.OrderLists.Add(tempItem);
                }
                db.Orders.Add(tempOrder);
                db.SaveChanges();

                MyServer.cleanCart(Session);
                Session["newOrder"] = null;
                return RedirectToAction("Index", "Manage");
            }
            //return RedirectToAction("Index");
        }
    }
}