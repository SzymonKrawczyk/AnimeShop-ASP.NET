using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace AnimeShop.Models
{

    //[Table("Kupujący")]
    public class CustomerModel
    {
        public int CustomerModelId { get; set; }
        public string UserId { get; set; }
        
        //[Required] [RegularExpression(@"^(.+@.+\..+)$")] [MaxLength(128)] [Display(Name = "Adres email")] public string Email { get; set; }
        
        [Required] 
        [RegularExpression(@"^[a-zA-ZąćęłńóśźżĄĆĘŁŃÓŚŹŻ''-'\s]{1,40}$", ErrorMessage = "Wprowadź poprawne imię")] 
        [Display(Name = "Imię")]
        public string FirstName { get; set; }
       
        [Required]
        [RegularExpression(@"^[a-zA-ZąćęłńóśźżĄĆĘŁŃÓŚŹŻ''-'\s]{1,40}$", ErrorMessage = "Wprowadź poprawne nazwisko")] 
        [Display(Name = "Nazwisko")] 
        public string LastName { get; set; }
        
        [Required]
        [Range(99999999, 999999999)] 
        [Display(Name = "Numer telefonu")] 
        public int PhoneNumber { get; set; }
        
        [Required]
        [RegularExpression(@"^([0-9][0-9]-[0-9][0-9][0-9])$", ErrorMessage = "Kod pocztowy musi mieć format 00-000")] 
        [Display(Name = "Kod Pocztowy")] 
        public string ZipCode { get; set; }
        
        [Required] 
        [RegularExpression(@"^[a-zA-ZąćęłńóśźżĄĆĘŁŃÓŚŹŻ''-'\s]{1,40}$", ErrorMessage = "Wprowadź poprawną nazwę miasta")] 
        [Display(Name = "Miasto")] 
        public string City { get; set; }
        
        [Required] 
        [MinLength(2)] 
        [MaxLength(128)] 
        [Display(Name = "Ulica")] 
        public string Street { get; set; }

        [Required] 
        [MinLength(1)] 
        [MaxLength(16)] 
        [Display(Name = "Nr Budynku")] 
        public string Building { get; set; }

        [MinLength(1)] 
        [MaxLength(16)] 
        [Display(Name = "Nr Lokalu")] 
        public string Apartment { get; set; }


        //[NotMapped] public string FullName { get; set; }
        
        
        //public virtual ApplicationUser User { get; set; }
        public virtual ICollection<OrderModel> Orders { get; set; }
    }
}