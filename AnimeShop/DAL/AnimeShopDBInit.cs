using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace AnimeShop.DAL
{
    public class AnimeShopDBInit<T> : CreateDatabaseIfNotExists<AnimeShopContext>
    {

        protected override void Seed(AnimeShopContext context)
        {
            base.Seed(context);

            IList<Models.CategoryModel> Categories = new List<Models.CategoryModel> {

                  new Models.CategoryModel(){Name = "Figurki"}
                , new Models.CategoryModel(){Name = "Naklejki"}
                , new Models.CategoryModel(){Name = "Przypinki"}
            };

            foreach (Models.CategoryModel cat in Categories)
            {
                context.Categories.Add(cat);
            }

            Models.ProductModel productMeguminNendoroid = new Models.ProductModel()
            {
                  Name = "Megumin Nendoroid"
                , CategoryModelId = 1
                , Anime = "Kono Subarashii Sekai ni Shukufuku wo! (KONOSUBA)"
                , Size = "10cm"
                , Manufacturer = "Good Smile Company"
                , Price = 52.99
                , Description = "Ponowne wydanie Nendoroida największego arcymaga zaklęcia eksplozji - Megumin! " +
                    "Jest wyposażona w trzy osłony twarzy, w tym uśmiechniętą twarz, poważną twarz do rzucania zaklęć, a także słodki, nieśmiały wyraz twarzy." +
                    "\n\nJej charakterystyczny kapelusz czarodzieja można łatwo założyć i zdjąć w celu szybkiej zmiany wyglądu, " +
                    "a także różne inne opcjonalne części, takie jak laska, część z efektem wybuchu, modna opaska na oko i błyszczące części z efektem magii!"
                , Media = "Megumin_Nendoroid"
                , Active = true
            };

            Models.ProductModel productMeguminSchoolgirl = new Models.ProductModel()
            {
                  Name = "Megumin w mundurku szkolnym"
                , CategoryModelId = 1
                , Anime = "Kono Subarashii Sekai ni Shukufuku wo! (KONOSUBA)"
                , Size = "24cm"
                , Manufacturer = "Chara-Ani"
                , Price = 149.95
                , Description = "Arcymag z Klanu Szkarłatnych Demonów -  Megumin - w mundurze z czasów, gdy uczęszczała do szkoły w Szkarłatnej Wiosce Demonów!\nJej płaszcz powiewający na wietrze i mundur zostały wykonane w najdrobniejszych szczegółach.\n\nMegumin ma pewny wyraz twarzy, gdy przyjmuje pozę.\n\nPamiętaj, aby dodać ją do swojej kolekcji!"
                , Media = "Megumin_School_Uniform"
                , Active = true
            };   

            Models.ProductModel productDarling50Stickers = new Models.ProductModel()
            {
                  Name = "Darling in the FranXX | 50 unikalnych"
                , CategoryModelId = 2
                , Anime = "Darling in the FranXX"
                , Size = "50 sztuk"
                , Manufacturer = "Anime Shop"
                , Price = 9.99
                , Description = "50 niepowtarzających się wysokiej jakości naklejek!\n\nPojedyńcza naklejka ma rozmiar ~4 cm.\nWytrzymały materiał odporny na zarysowania(winyl), w 100 % wodoodporny, nie zmieniający koloru.\n\nDoskonałe na telefon, laptop, walizkę, lodówkę, szafę, samochód, motocykl, rower i wiele innych!"
                , Media = "Darling_Franxx_50"
                , Active = true
            }; 

            Models.ProductModel productGenshin50Stickers = new Models.ProductModel()
            {
                  Name = "Genshin Impact | 50 unikalnych"
                , CategoryModelId = 2
                , Anime = "Genshin Impact"
                , Size = "50 sztuk"
                , Manufacturer = "Anime Shop"
                , Price = 9.99
                , Description = "50 niepowtarzających się wysokiej jakości naklejek!\n\nPojedyńcza naklejka ma rozmiar ~4 cm.\nWytrzymały materiał odporny na zarysowania(winyl), w 100 % wodoodporny, nie zmieniający koloru.\n\nDoskonałe na telefon, laptop, walizkę, lodówkę, szafę, samochód, motocykl, rower i wiele innych!"
                , Media = "Genshin_Impact_50"
                , Active = true
            };    

            Models.ProductModel productEmiliaPin = new Models.ProductModel()
            {
                  Name = "Emilia | 5.8cm"
                , CategoryModelId = 3
                , Anime = "Re:Zero kara Hajimeru Isekai Seikatsu"
                , Size = "5.8cm"
                , Manufacturer = "Anime Shop"
                , Price = 4.99
                , Description = "Duża, wytrzymała przypinka wysokiej jakości z wizerunkiem Emilii."
                , Media = "Emilia_pin"
                , Active = true
            };    

            Models.ProductModel productKurumiPin = new Models.ProductModel()
            {
                  Name = "Kurumi | 5.8cm"
                , CategoryModelId = 3
                , Anime = "Date A Live"
                , Size = "5.8cm"
                , Manufacturer = "Anime Shop"
                , Price = 4.99
                , Description = "Duża, wytrzymała przypinka wysokiej jakości z wizerunkiem Kurumi."
                , Media = "Kurumi_pin"
                , Active = false
            };

            Models.ProductModel productAquaPOPUP = new Models.ProductModel()
            {
                  Name = "Aqua POP UP PARADE"
                , CategoryModelId = 1
                , Anime = "Kono Subarashii Sekai ni Shukufuku wo! (KONOSUBA)"
                , Size = "18.5cm"
                , Manufacturer = "Good Smile Company"
                , Price = 114.45
                , Description = "Wspaniała bogini Aqua z serii POP UP PARADE!\n\nJej energiczna osobowość z anime została starannie zachowana w formie figurki."
                , Media = "Aqua_Pop_Up_Parade"
                , Active = true
            };

            context.Products.Add(productMeguminNendoroid);
            context.Products.Add(productMeguminSchoolgirl);
            context.Products.Add(productDarling50Stickers);
            context.Products.Add(productGenshin50Stickers);
            context.Products.Add(productEmiliaPin);
            context.Products.Add(productKurumiPin);
            context.Products.Add(productAquaPOPUP);










            IList<Models.ShipmentModel> Shipments = new List<Models.ShipmentModel> {

                  new Models.ShipmentModel(){Name = "Kurier InPost", Cost = 19.99, Active = true}
                , new Models.ShipmentModel(){Name = "Kurier DHL", Cost = 24.99, Active = true}
            };

            foreach (Models.ShipmentModel ship in Shipments)
            {
                context.Shipments.Add(ship);
            }




            IList<Models.OrderStatusModel> OrderStatuses = new List<Models.OrderStatusModel> {

                  new Models.OrderStatusModel(){Name = "Przygotowywanie zamówienia"}
                , new Models.OrderStatusModel(){Name = "Wysłano"}
            };

            foreach (Models.OrderStatusModel ordSt in OrderStatuses)
            {
                context.OrderStatuses.Add(ordSt);
            }





            context.SaveChanges();
        }
    }
}