using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Visa.Checkout.BLL;
using Visa.Checkout.DAL;
using Visa.Checkout.DAL.Data;
using Visa.Checkout.Entity;

namespace Visa.Checkout.Controllers
{
    [EnableCors("AllowSpecificOrigins")]
    [ApiController]
    [Route("api/[controller]")]
    public class RulesController : Controller
    {
        private readonly IRuleService _service;

        public RulesController(IRuleService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IEnumerable<PricingRule>> GetRules() 
        {
            return await _service.GetRules();

        }

      
    }
}
