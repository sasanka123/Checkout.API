using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Visa.Checkout.BLL;
using Visa.Checkout.DAL;
using Visa.Checkout.DAL.Data;
using Visa.Checkout.Entity;
using System.Linq;

namespace Visa.Checkout.Controllers
{
    [EnableCors("AllowSpecificOrigins")]
    [ApiController]
    [Route("api/[controller]")]
    public class CheckOutController : Controller
    {
        private readonly ICheckOutService _checkOutService;

        public CheckOutController(ICheckOutService checkOutService)
        {
            _checkOutService = checkOutService;
        }

        [HttpPost]
        public async Task<int> GetCheckOutPriceAsync([FromBody] string items) 
        {

            return await _checkOutService.GetCheckOutPrice(items);
        }
    }
}
