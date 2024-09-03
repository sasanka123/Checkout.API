using NUnit.Framework;
using Moq;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Visa.Checkout.BLL;
using Visa.Checkout.Entity;

namespace Visa.Test
{
    public class CheckOutServiceTests
    {
        private Mock<IRuleService> _mockRuleService;
        private CheckOutService _checkOutService;

        [SetUp]
        public void Setup()
        {
            _mockRuleService = new Mock<IRuleService>();
            _checkOutService = new CheckOutService(_mockRuleService.Object);
        }


        [TestCase("", 0)]
        [TestCase("A", 50)]
        [TestCase("AB", 80)]
        [TestCase("CDBA", 115)]
        [TestCase("CDBA", 115)]
        [TestCase("AA", 100)]
        [TestCase("AAA", 130)]
        [TestCase("AAAA", 180)]
        [TestCase("AAAAAA", 230)]
        [TestCase("AAAAAA", 260)]
        [TestCase("AAAB", 160)]
        [TestCase("AAABB", 175)]
        [TestCase("AAABBD", 190)]
        [TestCase("DABABA", 190)]
        public async Task GetCheckOutPrice_ReturnsCorrectTotal(string shoppingList, int expectedTotal)
        {
            // Arrange
            var pricingRules = new List<PricingRule>
            {
                new PricingRule { Item = 'A', UnitPrice = 50, DiscountPriceUnits = 3, DiscountPrice = 20 },
                new PricingRule { Item = 'B', UnitPrice = 30, DiscountPriceUnits = 2, DiscountPrice = 15 },
                new PricingRule { Item = 'C', UnitPrice = 20 },
                new PricingRule { Item = 'D', UnitPrice = 15 }
            };

            _mockRuleService.Setup(s => s.GetRules()).ReturnsAsync(pricingRules);

            // Act
            int result = await _checkOutService.GetCheckOutPrice(shoppingList);

            // Assert
            Assert.AreEqual(expectedTotal, result);
        }

        [Test]
        public async Task GetCheckOutPrice_ReturnsZero_WhenNoItems()
        {
            // Arrange
            var pricingRules = new List<PricingRule>
            {
                new PricingRule { Item = 'A', UnitPrice = 50, DiscountPriceUnits = 3, DiscountPrice = 20 },
                new PricingRule { Item = 'B', UnitPrice = 30, DiscountPriceUnits = 2, DiscountPrice = 15 }
            };

            _mockRuleService.Setup(s => s.GetRules()).ReturnsAsync(pricingRules);

            string shoppingList = "";

            // Act
            int result = await _checkOutService.GetCheckOutPrice(shoppingList);

            // Assert
            Assert.AreEqual(0, result);
        }

        [Test]
        public async Task GetCheckOutPrice_ReturnsZero_WhenItemNotInRules()
        {
            // Arrange
            var pricingRules = new List<PricingRule>
            {
                new PricingRule { Item = 'A', UnitPrice = 50 },
                new PricingRule { Item = 'B', UnitPrice = 30 }
            };

            _mockRuleService.Setup(s => s.GetRules()).ReturnsAsync(pricingRules);

            string shoppingList = "XYZ"; // Items not in pricing rules

            // Act
            int result = await _checkOutService.GetCheckOutPrice(shoppingList);

            // Assert
            Assert.AreEqual(0, result);
        }
    }
}
