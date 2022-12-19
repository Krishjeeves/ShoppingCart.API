using ShoppingCart.Web.Models.Request;
using ShoppingCart.Web.Models.Response;

namespace ShoppingCart.Web.Services.Abstraction
{
    public interface ICartService
    {
        Task<CartResponse> GetCartItemsAsync(CartRequest cartRequest);
    }
}
