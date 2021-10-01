using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace AnimeShop.Models
{

    //[Table("Składniki zamówienia")]
    public class OrderListModel
    {

        public int OrderListModelId { get; set; }
        [Required] [Display(Name = "Zamówienie")] public int OrderModelId { get; set; }
        [Required] [Display(Name = "Produkt")] public int ProductModelId { get; set; }
        [Required] [Range(0, 1000)] [Display(Name = "Ilość")] public int Amount { get; set; }
        [Required] [Range(0.0, 100000.0)] [Display(Name = "Cena za sztukę")] public double Price { get; set; }

        public virtual OrderModel ParentOrder { get; set; }
        public virtual ProductModel Product { get; set; }
    }
}