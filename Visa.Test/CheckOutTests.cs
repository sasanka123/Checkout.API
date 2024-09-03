using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Visa.Checkout.Entity;
using Visa.Checkout.BLL;

namespace Visa.Test
{
    public class CheckOutTests
    {
        private IEnumerable<PricingRule> pricingRules = null;

        [OneTimeSetUp]
        public void OneTimeSetup()
        {
            pricingRules = new List<PricingRule>();
            pricingRules = pricingRules.Append(new PricingRule() { Item = 'A', UnitPrice = 50, DiscountPriceUnits = 3, DiscountPrice = 20 });
            pricingRules = pricingRules.Append(new PricingRule() { Item = 'B', UnitPrice = 30, DiscountPriceUnits = 2, DiscountPrice = 15 });
            pricingRules = pricingRules.Append(new PricingRule() { Item = 'C', UnitPrice = 20 });
            pricingRules = pricingRules.Append(new PricingRule() { Item = 'D', UnitPrice = 15 });
        }

        [Test]
        public void TestTotals()
        {
            Assert.That(Price(""), Is.EqualTo(0));
            Assert.That(Price("A"), Is.EqualTo(50));
            Assert.That(Price("AB"), Is.EqualTo(80));
            Assert.That(Price("CDBA"), Is.EqualTo(115));
            Assert.That(Price("AA"), Is.EqualTo(100));
            Assert.That(Price("AAA"), Is.EqualTo(130));
            Assert.That(Price("AAAA"), Is.EqualTo(180));
            Assert.That(Price("AAAAA"), Is.EqualTo(230));
            Assert.That(Price("AAAAAA"), Is.EqualTo(260));
            Assert.That(Price("AAAB"), Is.EqualTo(160));
            Assert.That(Price("AAABB"), Is.EqualTo(175));
            Assert.That(Price("AAABBD"), Is.EqualTo(190));
            Assert.That(Price("DABABA"), Is.EqualTo(190));
        }

        [Test]
        public void TestIncremental()
        {
            var co = new CheckOut(pricingRules);
            Assert.That(co.Total(), Is.EqualTo(0));
            co.Scan('A');
            Assert.That(co.Total(), Is.EqualTo(50));
            co.Scan('B');
            Assert.That(co.Total(), Is.EqualTo(80));
            co.Scan('A');
            Assert.That(co.Total(), Is.EqualTo(130));
            co.Scan('A');
            Assert.That(co.Total(), Is.EqualTo(160));
            co.Scan('B');
            Assert.That(co.Total(), Is.EqualTo(175));
        }

        private int Price(IEnumerable<char> items)
        {
            var co = new CheckOut(pricingRules);
            foreach (var item in items)
            {
                co.Scan(item);
            }
            return co.Total();
        }
    }
}
