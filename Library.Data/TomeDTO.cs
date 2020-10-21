using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Library.Data
{
    public class TomeDTO
    {
        public int Id { get; set; }

        public int BookId { get; set; }

        public string BookTitle { get; set; }

        public BookDTO Book { get; set; }

        public override bool Equals(object obj)
        {
            return (obj is TomeDTO dto) &&
                Id == dto.Id;
        }

        public override int GetHashCode()
        {
            return Id;
        }
    }
}
