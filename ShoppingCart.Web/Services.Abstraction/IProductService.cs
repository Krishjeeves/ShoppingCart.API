using ShoppingCart.Web.Models.Response;

namespace ShoppingCart.Web.Services.Abstraction
{
    public interface IProductService
    {
        Task<ProductItemResponse[]> GetAsync(int skip = 0, int take = 100);        
    }
}
