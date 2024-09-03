using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Visa.Checkout.Entity
{
    public class PricingRule
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ItemId { get; set; }

        public char Item { get; set; }

        public int UnitPrice { get; set; }

        public int? DiscountPriceUnits { get; set; }

        public int? DiscountPrice { get; set; }

    }
}
