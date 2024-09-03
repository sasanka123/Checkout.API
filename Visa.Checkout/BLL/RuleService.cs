using Microsoft.EntityFrameworkCore;
using Visa.Checkout.DAL;
using Visa.Checkout.Entity;

namespace Visa.Checkout.BLL
{
    public class RuleService: IRuleService
    {
        private readonly IRuleRepository _repository;

        public RuleService(IRuleRepository repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<PricingRule>> GetRules()
        {
            return await _repository.GetRules();
        }
    }
}
