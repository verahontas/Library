using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Library.Web.Models
{
    public enum LoanDateError
    {
        None,
        StartInvalid,
        EndInvalid,
        LengthInvalid,
        Conflicting
    }
}
