using System;
using System.ComponentModel.DataAnnotations;

namespace Library.Web.Models
{
    public class GuestViewModel
    {
        [Required(ErrorMessage = "A név megadása kötelező.")]
        [StringLength(60, ErrorMessage = "A foglaló neve maximum 60 karakter lehet.")]
        public String GuestName { get; set; }

        [Required(ErrorMessage = "Az e-mail cím megadása kötelező.")]
        [EmailAddress(ErrorMessage = "Az e-mail cím nem megfelelő formátumú.")]
        [DataType(DataType.EmailAddress)]
        public String GuestEmail { get; set; }

        [Required(ErrorMessage = "A telefonszám megadása kötelező.")]
        [Phone(ErrorMessage = "A telefonszám formátuma nem megfelelő.")]
        [DataType(DataType.PhoneNumber)]
        public String GuestPhoneNumber { get; set; }
    }
}