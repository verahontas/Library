using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Library.Web.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Library.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoanDateController : ControllerBase
    {

        private ILoanService _service;

        public LoanDateController(ILoanService service)
        {
            _service = service;
        }

        /// <summary>
        /// Adott adott hónap szabad napjainak lekérdezése.
        /// </summary>
        /// <param name="apartmentId">Apartman azonosító.</param>
        /// <param name="year">Az év.</param>
        /// <param name="month">A hónap.</param>
        /// <returns>Az adott hónap szabad napjai egy gyűjteményben.</returns>
        [Route("{tomeId}/{year}/{month}")] // útvonal feloldás megadása
        public IActionResult Get(Int32? tomeId, Int32? year, Int32? month)
        {
            if (tomeId == null || year == null || month == null)
                return BadRequest(); // 400-as válaszkód

            return Ok(_service.GetLoanDates(tomeId.Value, year.Value, month.Value)); // 200-as válaszkód
        }
    }
}