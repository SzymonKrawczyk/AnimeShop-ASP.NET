using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace AnimeShop.Models
{

    //[Table("Status")]
    public class OrderStatusModel
    {

        public int OrderStatusModelId { get; set; }
        [Required] [MaxLength(128)] [Display(Name = "Nazwa")] public string Name { get; set; }

        public virtual ICollection<OrderModel> Orders { get; set; }
    }
}