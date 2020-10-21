using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Library.Data
{
    public class LoanDTO
    {
        public int Id { get; set; }

        public int TomeId { get; set; }

        public TomeDTO Tome { get; set; }

        public int UserId { get; set; }

        public DateTime FirstDay { get; set; }

        public DateTime LastDay { get; set; }

        public bool IsActive { get; set; }

        public Boolean IsConflicting(DateTime firstDay, DateTime lastDay)
        {
            return FirstDay >= firstDay && FirstDay < lastDay ||
                   LastDay >= firstDay && LastDay < lastDay ||
                   FirstDay < firstDay && LastDay > lastDay ||
                   FirstDay > firstDay && LastDay < lastDay;
        }

        public override bool Equals(object obj)
        {
            return (obj is LoanDTO dto) &&
                Id == dto.Id &&
                UserId == dto.UserId &&
                FirstDay == dto.FirstDay &&
                LastDay == dto.LastDay &&
                IsActive == dto.IsActive;
        }

        public override int GetHashCode()
        {
            return Id;
        }
    }
}
