using System.ComponentModel;
using Visa.Checkout.DAL;
using Visa.Checkout.Entity;

namespace Visa.Checkout.BLL
{
    public class CheckOutService : ICheckOutService
    {
        private IEnumerable<PricingRule> _pricingRules;
        private readonly IRuleService _service;

        public CheckOutService(IRuleService service)
        {
            _service = service;
        }

        private async Task GetPricingRules()
        {
            _pricingRules = await _service.GetRules();
        }

        public int Total(IEnumerable<char> items)
        {
            return items.GroupBy(item => item)
                .Select(itemGroup => ItemPrice(itemGroup.Key, itemGroup.Count()))
                .Sum();
        }

        //private int ItemPrice(char item, int units)
        //{
        //    var rule = _pricingRules.SingleOrDefault(pr => pr.Item == item);
        //    if (rule is null)
        //        return 0;
        //    var normalPrice = rule.UnitPrice * units;
        //    var discountPrice = default(int);
        //    if (rule.DiscountPriceUnits.HasValue && rule.DiscountPrice.HasValue)
        //    {
        //        discountPrice = Convert.ToInt32(Math.Floor(units / (decimal)rule.DiscountPriceUnits.Value)) * rule.DiscountPrice.Value;
        //    }
        //    return normalPrice - discountPrice;
        //}

        private int ItemPrice(char item, int units)
        {
            // Get all rules for the specified item
            var rules = _pricingRules.Where(pr => pr.Item == item).OrderByDescending(r => r.DiscountPriceUnits).ToList();
            if (!rules.Any())
                return 0;

            var totalDiscount = 0;
            var remainingUnits = units;

            // Apply the rules in order, largest discount units first
            foreach (var rule in rules)
            {
                if (remainingUnits >= rule.DiscountPriceUnits && rule.DiscountPriceUnits.HasValue && rule.DiscountPrice.HasValue)
                {
                    // Calculate how many times the discount can be applied
                    var discountApplies = remainingUnits / rule.DiscountPriceUnits.Value;

                    // Apply the discount
                    totalDiscount += discountApplies * rule.DiscountPrice.Value;

                    // Reduce the remaining units
                    remainingUnits -= discountApplies * rule.DiscountPriceUnits.Value;
                }
            }

            // Calculate the normal price for any remaining units that couldn't get a discount
            var normalPrice = remainingUnits * rules.First().UnitPrice;

            return normalPrice + (units - remainingUnits) * rules.First().UnitPrice - totalDiscount;
        }



        public async Task<int> GetCheckOutPrice(string shoppingList)
        {
            await GetPricingRules();
            return Total(shoppingList);
        }
    }
}
