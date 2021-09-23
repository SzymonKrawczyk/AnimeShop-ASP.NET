using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace AnimeShop.Models
{

    //[Table("Kategorie")]
    public class CategoryModel
    {

        public int CategoryModelId { get; set; }

        [Required(ErrorMessage = "Nazwa jest wymagana")]
        [MinLength(3, ErrorMessage = "Nazwa musi mieć przynajmniej 3 znaki")]
        [MaxLength(128, ErrorMessage = "Długość nazwy nie może przekraczać 128 znaków")]
        [Display(Name = "Nazwa")]
        public string Name { get; set; }

        public virtual ICollection<ProductModel> Products { get; set; }
    }
}