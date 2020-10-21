using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Library.Model
{
    public class BookImage
    {
        public int Id { get; set; }
        public int BookId { get; set; }
        public byte[] ImageSmall { get; set; }
        public byte[] ImageLarge { get; set; }
        public Book Book { get; set; }
    }
}
