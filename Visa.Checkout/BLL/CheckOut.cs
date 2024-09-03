using Visa.Checkout.Entity;

namespace Visa.Checkout.BLL
{
    public class CheckOut
    {
        private readonly IEnumerable<PricingRule> pricingRules;
        private IEnumerable<char> items = new List<char>();

        public CheckOut(IEnumerable<PricingRule> pricingRules)
        {
            this.pricingRules = pricingRules;
        }

        public void Scan(char item)
        {
            items = items.Append(item);
        }

        public int Total()
        {
            return items.GroupBy(item => item)
                .Select(itemGroup => ItemPrice(itemGroup.Key, itemGroup.Count()))
                .Sum();
        }

        private int ItemPrice(char item, int units)
        {
            var rule = pricingRules.Single(pr => pr.Item == item);
            if (rule == null)
                return 0;
            var normalPrice = rule.UnitPrice * units;
            var discountPrice = default(int);
            if (rule.DiscountPriceUnits.HasValue && rule.DiscountPrice.HasValue)
            {
                discountPrice = Convert.ToInt32(Math.Floor(units / (decimal)rule.DiscountPriceUnits.Value)) * rule.DiscountPrice.Value;
            }
            return normalPrice - discountPrice;
        }
    }
}
