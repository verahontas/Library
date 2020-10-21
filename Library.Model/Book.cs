using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Library.Model
{
    public class Book
    {
        public Book()
        {
            Tomes = new HashSet<Tome>();
        }

        public int Id { get; set; }

        [Required]
        [RegularExpression(@"^[0-9]{9,13}$",
         ErrorMessage = "Az ISBN szám legalább 9, maximum 13 számjegyből kell álljon!")]
        public string ISBN { get; set; }

        [Required]
        public string Title { get; set; }
        
        [Required]
        public string Author { get; set; }

        [Range(0, 2020, ErrorMessage = "A kiadási évnek 0 és 2020 közé kell esnie!")]
        public int Year { get; set; }

        public int NumberOfLoans { get; set; }

        public ICollection<Tome> Tomes { get; set; }
    }
}
