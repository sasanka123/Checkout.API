using Visa.Checkout.Entity;

namespace Visa.Checkout.BLL
{
    public interface IRuleService
    {
        Task<IEnumerable<PricingRule>> GetRules();
    }
}
