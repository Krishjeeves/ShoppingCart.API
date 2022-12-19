using ShoppingCart.Domain.Entities;
using ShoppingCart.Domain.Repositories;
using Bogus;

namespace ShoppingCart.DataAccess.Repositories
{
    public class ProductRepository : IProductRepository
    {
        public IQueryable<Product> GetQuery()
        {
            var id = 0;
            var products = new Faker<Product>()
                .RuleFor(p => p.Id, _ => ++id)
              //  .RuleFor(p => p.NoOfItemsAvailable, m => m.Random.Int(0, 100))
                .RuleFor(p => p.Name, m => m.Commerce.ProductName())
                .RuleFor(p => p.Description, m => m.Commerce.ProductDescription())
                .RuleFor(p => p.Image, m => m.Image.PicsumUrl())
                .RuleFor(p => p.Price, m => Math.Round(m.Random.Decimal(3, 65), 2));

            return products.UseSeed(20122022).Generate(100).AsQueryable();
        }
    }
}
