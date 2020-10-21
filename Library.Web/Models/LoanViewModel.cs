using Library.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Library.Web.Models
{
    public class LoanViewModel : GuestViewModel
    {
        public Tome Tome { get; set; }

        [Required(ErrorMessage = "A kezdő dátum megadása kötelező.")]
        [DataType(DataType.Date)]
        public DateTime LoanFirstDay { get; set; }

        [Required(ErrorMessage = "A vége dátum megadása kötelező.")]
        [DataType(DataType.Date)]
        public DateTime LoanLastDay { get; set; }
    }
}
