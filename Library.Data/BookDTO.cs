using System;
using System.Collections.Generic;
using System.Text;

namespace Library.Data
{
    public class BookDTO
    {
        public BookDTO()
        {
            Images = new List<ImageDTO>();
        }

        public int Id { get; set; }

        public string ISBN { get; set; }

        public string Title { get; set; }
        
        public string Author { get; set; }

        public int Year { get; set; }

        public int NumberOfLoans { get; set; }

        public IList<ImageDTO> Images { get; set; }

        public override Boolean Equals(Object obj)
        {
            return (obj is BookDTO dto) &&
                   Id == dto.Id &&
                   Title == dto.Title;
        }

        public override int GetHashCode()
        {
            return Id;
        }

        public override String ToString()
        {
            return Title;
        }
    }
}
