using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Library.Model
{
    public class Loan
    {
        public int Id { get; set; }

        public int TomeId { get; set; }

        [Required]
        public Tome Tome { get; set; }

        [ForeignKey("Guest")]
        public int UserId { get; set; }

        [Required]
        public DateTime FirstDay { get; set; }

        [Required]
        public DateTime LastDay { get; set; }

        public bool IsActive { get; set; }

        public Boolean IsConflicting(DateTime firstDay, DateTime lastDay)
        {
            return FirstDay >= firstDay && FirstDay < lastDay ||
                   LastDay >= firstDay && LastDay < lastDay ||
                   FirstDay < firstDay && LastDay > lastDay ||
                   FirstDay > firstDay && LastDay < lastDay;
        }

        public Boolean IsConflicting(DateTime date)
        {
            return IsConflicting(date, date + TimeSpan.FromDays(1));
        }
    }
}
