using Library.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Library.Web.Models
{
    public class LoanDateValidator
    {
        private readonly LibraryContext _context;

        public LoanDateValidator(LibraryContext context)
        {
            _context = context;
        }

        public LoanDateError Validate(DateTime firstDay, DateTime lastDay, Int32 tomeId)
        {
            if (lastDay < firstDay)
                return LoanDateError.EndInvalid;

            if (lastDay == firstDay)
                return LoanDateError.LengthInvalid;

            Tome selectedTome = _context.Tomes.FirstOrDefault(l => l.Id == tomeId);

            if (selectedTome == null)
                return LoanDateError.None;

            if (_context.Loans.Where(l => l.TomeId == selectedTome.Id && l.LastDay >= firstDay)
                                .ToList()
                                .Any(l => l.IsConflicting(firstDay, lastDay)))
                return LoanDateError.Conflicting;

            return LoanDateError.None;
        }
    }
}
