using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace AnimeShop.Models
{

    //[Table("Dostawa")]
    public class ShipmentModel
    {

        public int ShipmentModelId { get; set; }

        [Required(ErrorMessage = "Nazwa jest wymagana")]
        [MinLength(5, ErrorMessage = "Nazwa musi mieć przynajmniej 5 znaków")]
        [MaxLength(128, ErrorMessage = "Długość nazwy nie może przekraczać 128 znaków")]
        [Display(Name = "Nazwa")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Koszt dostawy jest wymagany")]
        [Range(0.0, 100000.0, ErrorMessage = "Koszt dostawy musi być w przedziale [0; 100000]")]
        [Display(Name = "Koszt dostawy")]
        public double Cost { get; set; }

        [Required(ErrorMessage = "Należy określić, czy dostawa jest dostępna")]
        [Display(Name = "Dostępność")] 
        public bool Active { get; set; }

        public virtual ICollection<OrderModel> Orders { get; set; }
    }
}