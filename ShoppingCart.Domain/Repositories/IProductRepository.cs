using ShoppingCart.Domain.Entities;

namespace ShoppingCart.Domain.Repositories
{
    public interface IProductRepository
    {
        IQueryable<Product> GetQuery();

    }
}
