using Visa.Checkout.Entity;

namespace Visa.Checkout.BLL
{
    public interface ICheckOutService
    {
        Task<int> GetCheckOutPrice(string shoppingList);
    }
}
