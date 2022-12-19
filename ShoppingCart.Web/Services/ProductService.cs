using ShoppingCart.Domain.Repositories;
using ShoppingCart.Web.Models.Response;
using ShoppingCart.Web.Services.Abstraction;
using System.Linq;

namespace ShoppingCart.Web.Services
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository _productRepository;

        public ProductService(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        //Now static list but DB calls are supposed to be async  . Hence the function here .
        public async Task<ProductItemResponse[]> GetAsync(int skip = 0, int take = 100)
        {
            return  _productRepository.GetQuery().OrderBy(p => p.Id).Skip(skip).Take(take).Select(
                p => new ProductItemResponse
                {
                    Id = p.Id,
                    Name = p.Name,
                    Description = p.Description,
                    Image = p.Image,
                    Price = p.Price
                }).AsEnumerable().ToArray();
        }
    }
}
