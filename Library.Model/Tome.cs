using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Library.Model
{
    public class Tome
    {
        public Tome()
        {
            Loans = new HashSet<Loan>();
        }

        public int Id { get; set; }

        public int BookId { get; set; }

        [Required]
        public Book Book { get; set; }

        public ICollection<Loan> Loans { get; set; }
    }
}
