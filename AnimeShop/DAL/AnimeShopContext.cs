using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using System.Web;
using AnimeShop.Models;

namespace AnimeShop.DAL
{

    public class AnimeShopContext : DbContext
    {

        public AnimeShopContext() : base("AnimeShopContext")
        {
            //public GarageContext() : base("name=DefaultConnection") {

            MyServer.init();

            Database.SetInitializer(new AnimeShopDBInit<AnimeShopContext>());
        }

        public DbSet<CategoryModel> Categories { get; set; }
        public DbSet<CustomerModel> Customers { get; set; }
        public DbSet<ProductModel> Products { get; set; }
        public DbSet<OrderModel> Orders { get; set; }
        public DbSet<OrderListModel> OrderLists { get; set; }
        public DbSet<OrderStatusModel> OrderStatuses { get; set; }
        public DbSet<ShipmentModel> Shipments { get; set; }


        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {

            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
        }

    }
}