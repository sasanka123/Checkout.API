using Microsoft.EntityFrameworkCore;
using Visa.Checkout.DAL.Data;
using Visa.Checkout.Entity;

namespace Visa.Checkout.DAL
{

    public class RuleRepository : IRuleRepository
    {
        private readonly AppDbContext _context;

        public RuleRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<PricingRule>> GetRules()
        {
                return await _context.PricingRules.ToListAsync();
        }

    }
}
