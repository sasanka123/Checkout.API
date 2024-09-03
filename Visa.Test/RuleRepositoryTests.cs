using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Visa.Checkout.DAL.Data;
using Visa.Checkout.DAL;
using Visa.Checkout.Entity;

namespace Visa.Test
{
    public class RuleRepositoryTests
    {
        private RuleRepository _repository;
        private AppDbContext _context;

        [SetUp]
        public void Setup()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDb")
                .Options;

            _context = new AppDbContext(options);

            // Seed the in-memory database
            _context.PricingRules.AddRange(
                new PricingRule { Item = 'A', UnitPrice = 50, DiscountPriceUnits = 3, DiscountPrice = 130 },
                new PricingRule { Item = 'B', UnitPrice = 30, DiscountPriceUnits = 2, DiscountPrice = 45 }
            );

            _context.SaveChanges();

            _repository = new RuleRepository(_context);
        }

        [TearDown]
        public void TearDown()
        {
            _context.Database.EnsureDeleted();
            _context.Dispose();
        }

        [Test]
        public async Task GetRules_ReturnsAllRules()
        {
            // Act
            var result = await _repository.GetRules();

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(2, result.Count());
            Assert.AreEqual('A', result.First().Item);
        }

        [Test]
        public async Task GetRules_ReturnsEmptyList_WhenNoRulesExist()
        {
            // Arrange
            _context.PricingRules.RemoveRange(_context.PricingRules);
            await _context.SaveChangesAsync();

            // Act
            var result = await _repository.GetRules();

            // Assert
            Assert.IsNotNull(result);
            Assert.IsEmpty(result);
        }
    }
}
