using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace AnimeShop.Models
{
    // Możesz dodać dane profilu dla użytkownika, dodając więcej właściwości do klasy ApplicationUser. Odwiedź stronę https://go.microsoft.com/fwlink/?LinkID=317594, aby dowiedzieć się więcej.
    public class ApplicationUser : IdentityUser
    {
        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
        {
            // Element authenticationType musi pasować do elementu zdefiniowanego w elemencie CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Dodaj tutaj niestandardowe oświadczenia użytkownika
            return userIdentity;
        }

        //public virtual CustomerModel Customer { get; set; }
    }

    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext()
            //: base("DefaultConnection", throwIfV1Schema: false)
            : base("AnimeShopContext", throwIfV1Schema: false)
        {
            Database.SetInitializer(new IdentityDBInitializer<ApplicationDbContext>());
        }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }

        
    }

    public class IdentityDBInitializer<T> : CreateDatabaseIfNotExists<ApplicationDbContext>
    {
        protected override void Seed(ApplicationDbContext context)
        {
            context.Roles.Add(new IdentityRole { Name = "admin" });
            context.Roles.Add(new IdentityRole { Name = "user" });
            context.SaveChanges();

            var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));
            var store = new UserStore<ApplicationUser>(context);
            var manager = new UserManager<ApplicationUser>(store);

            var newAdmin = new ApplicationUser { UserName = "admin@admin.pl", Email = "admin@admin.pl" };
            manager.Create(newAdmin, "!2Qwerty");

            var roleAdmin = context.Roles.SingleOrDefault(m => m.Name == "admin");

            ApplicationUser newAdminT = userManager.FindByName("admin@admin.pl");
            userManager.AddToRole(newAdminT.Id, roleAdmin.Name);

            context.SaveChanges();
        }
    }


}