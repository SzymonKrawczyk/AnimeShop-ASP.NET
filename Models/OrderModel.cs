using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace AnimeShop.Models
{

    //[Table("Zamówienia")]
    public class OrderModel
    {

        public int OrderModelId { get; set; }
        [Required] [Display(Name = "Kupujący")] public int CustomerModelId { get; set; }
        [Required] [Display(Name = "Dostawa")] public int ShipmentModelId { get; set; }
        [Required] [MaxLength(128)] [Display(Name = "Dodatkowe informacje do dostawy")] public string ShipmentInfo { get; set; }
        [Required] [Display(Name = "Czas zakupu")] public DateTime Date { get; set; }
        [MaxLength(128)] [Display(Name = "Dodatkowe informacje")] public string AdditionalInfo { get; set; }
        [Required] [Display(Name = "Status zamówienia")] public int OrderStatusModelId { get; set; }

        public virtual ICollection<OrderListModel> OrderLists { get; set; }
        public virtual CustomerModel Customer { get; set; }
        public virtual ShipmentModel Shipment { get; set; }
        public virtual OrderStatusModel Status { get; set; }
    }
}