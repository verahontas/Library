using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Library.Data
{
    public class ImageDTO
    {
        public int Id { get; set; }
        public int BookId { get; set; }
        public Byte[] ImageSmall { get; set; }
        public Byte[] ImageLarge { get; set; }
    }
}
