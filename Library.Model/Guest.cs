using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Library.Model
{
    public class Guest : IdentityUser<int>
    {
        public Guest()
        {
            Loans = new HashSet<Loan>();
        }

        public string Name { get; set; }

        public ICollection<Loan> Loans { get; set; }
    }
}
