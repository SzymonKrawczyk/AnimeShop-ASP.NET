using System;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AnimeShop.DAL;
using AnimeShop.Models;
using Microsoft.AspNet.Identity;


namespace AnimeShop.Controllers {

    [Authorize]
    public class CustomerModelController : Controller{


        private AnimeShopContext db = new AnimeShopContext();

        // GET: CustomerModel/Create
        public ActionResult Create() {

            string id = User.Identity.GetUserId();

            var customerTemp = db.Customers.Where(c => c.UserId == id);
            CustomerModel customer = null;
            if (customerTemp.Count() > 0) customer = customerTemp.First();

            if (customer != null) {

                return RedirectToAction("Edit");
            }
            return View();
        }

        // POST: CustomerModel/Create
        [HttpPost]
        public ActionResult Create([Bind(Include = "FirstName, LastName, PhoneNumber, ZipCode, City, Street, Building, Apartment")] CustomerModel newCustomer) {

            string id = User.Identity.GetUserId();

            var customerTemp = db.Customers.Where(c => c.UserId == id);
            CustomerModel customer = null;
            if (customerTemp.Count() > 0) customer = customerTemp.First();

            if (customer != null) {

                return RedirectToAction("Edit");
            }

            if (ModelState.IsValid) {

                newCustomer.UserId = id;
                db.Customers.Add(newCustomer);
                db.SaveChanges();
                return RedirectToAction("Index", "Manage");
            }
            return View(newCustomer);
        }

        // GET: CustomerModel/Edit
        public ActionResult Edit() {

            string id = User.Identity.GetUserId();

            var customerTemp =  db.Customers.Where(c => c.UserId == id);
            CustomerModel customer = null;
            if (customerTemp.Count() > 0) customer = customerTemp.First();

            if (customer == null) {

                return RedirectToAction("Create");
            }

            return View(customer);
        }

        // POST: CustomerModel/Edit
        [HttpPost]
        public ActionResult Edit([Bind(Include = "FirstName, LastName, PhoneNumber, ZipCode, City, Street, Building, Apartment")] CustomerModel editedCustomer) {

            string id = User.Identity.GetUserId();

            var customerTemp = db.Customers.Where(c => c.UserId == id);
            CustomerModel customer = null;
            if (customerTemp.Count() > 0) customer = customerTemp.First();

            if (customer == null) {

                return RedirectToAction("Create");
            }

            if (ModelState.IsValid) {

                customer.FirstName = editedCustomer.FirstName;
                customer.LastName = editedCustomer.LastName;
                customer.PhoneNumber = editedCustomer.PhoneNumber;
                customer.ZipCode = editedCustomer.ZipCode;
                customer.City = editedCustomer.City;
                customer.Street = editedCustomer.Street;
                customer.Building = editedCustomer.Building;
                customer.Apartment = editedCustomer.Apartment;

                db.SaveChanges();
                return RedirectToAction("Index", "Manage");
            }

            return View(editedCustomer);
        }
    }
}
