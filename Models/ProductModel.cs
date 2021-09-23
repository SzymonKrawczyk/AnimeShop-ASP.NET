using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace AnimeShop.Models
{

    //[Table("Produkty")]
    public class ProductModel
    {

        public int ProductModelId { get; set; }
        [Required(ErrorMessage = "Nazwa jest wymagana")]
        [MinLength(3, ErrorMessage = "Nazwa musi mieć przynajmniej 3 znaki")]
        [MaxLength(128, ErrorMessage = "Długość nazwy nie może przekraczać 128 znaków")]
        [Display(Name = "Nazwa")]
        public string Name { get; set; }

        [Display(Name = "Kategoria")]
        public int? CategoryModelId { get; set; }

        [Required]
        [MinLength(3, ErrorMessage = "Nazwa anime musi mieć przynajmniej 3 znaki")]
        [MaxLength(256, ErrorMessage = "Długość anime nazwy nie może przekraczać 256 znaków")]
        [Display(Name = "Anime")]
        public string Anime { get; set; }

        [Required]
        [MaxLength(64, ErrorMessage = "Specyfikacja wielkości nie może przekraczać 64 znaków")]
        [Display(Name = "Wielkość")]
        public string Size { get; set; }

        [Required]
        [MinLength(3, ErrorMessage = "Nazwa producenta musi mieć przynajmniej 3 znaki")]
        [MaxLength(128, ErrorMessage = "Długość nazwy producenta nie może przekraczać 128 znaków")]
        [Display(Name = "Producent")]
        public string Manufacturer { get; set; }

        [Required(ErrorMessage = "Cena jest wymagana")]
        [Range(0.0, 100000.0, ErrorMessage = "Cena musi być w przedziale [0; 100000]")]
        [Display(Name = "Cena")]
        public double Price { get; set; }

        [Required]
        [MinLength(16, ErrorMessage = "Opis przynajmniej 16 znaków")]
        [MaxLength(1024, ErrorMessage = "Długość opisu nie może przekraczać 1024 znaków")]
        [Display(Name = "Opis")]
        public string Description { get; set; }

        [MaxLength(64, ErrorMessage = "Ścieżka do folderu ze zdjęciami nie może przekraczać 64 znaków")]
        [Display(Name = "Media")]
        public string Media { get; set; }

        [Required(ErrorMessage = "Należy określić, czy produkt jest dostępny")]
        [Display(Name = "Dostępność")]
        public bool Active { get; set; }

        public virtual ICollection<OrderListModel> OrderLists { get; set; }
        public virtual CategoryModel Category { get; set; }
    }
}