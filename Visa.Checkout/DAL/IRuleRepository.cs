using Visa.Checkout.Entity;

namespace Visa.Checkout.DAL
{
    public interface IRuleRepository
    {
        Task<IEnumerable<PricingRule>> GetRules();
    }
}
