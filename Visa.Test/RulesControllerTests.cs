using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Visa.Checkout.BLL;
using Visa.Checkout.Controllers;
using Visa.Checkout.Entity;

namespace Visa.Test
{
    public class RulesControllerTests
    {
        private Mock<IRuleService> _mockRuleService;
        private RulesController _controller;

        [SetUp]
        public void Setup()
        {
            _mockRuleService = new Mock<IRuleService>();
            _controller = new RulesController(_mockRuleService.Object);
        }

        [Test]
        public async Task GetRules_ReturnsExpectedRules()
        {
            // Arrange
            var expectedRules = new List<PricingRule>
            {
                new PricingRule { Item = 'A', UnitPrice = 50, DiscountPriceUnits = 3, DiscountPrice = 130 },
                new PricingRule { Item = 'B', UnitPrice = 30, DiscountPriceUnits = 2, DiscountPrice = 45 },
            };

            _mockRuleService.Setup(s => s.GetRules()).ReturnsAsync(expectedRules);

            // Act
            var result = await _controller.GetRules();

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(expectedRules.Count, result.Count());
            Assert.AreEqual(expectedRules.First().Item, result.First().Item);
        }

        [Test]
        public async Task GetRules_ReturnsEmptyList_WhenNoRulesAvailable()
        {
            // Arrange
            var emptyRules = new List<PricingRule>();
            _mockRuleService.Setup(s => s.GetRules()).ReturnsAsync(emptyRules);

            // Act
            var result = await _controller.GetRules();

            // Assert
            Assert.IsNotNull(result);
            Assert.IsEmpty(result);
        }

        [Test]
        public async Task GetRules_CallsServiceMethodOnce()
        {
            // Arrange
            _mockRuleService.Setup(s => s.GetRules()).ReturnsAsync(new List<PricingRule>());

            // Act
            var result = await _controller.GetRules();

            // Assert
            _mockRuleService.Verify(s => s.GetRules(), Times.Once);
        }
    }
}
