using ShoppingCart.Domain.Enums;
using ShoppingCart.Domain.Repositories;
using ShoppingCart.Web.Services.Abstraction;

namespace ShoppingCart.Web.Services
{
    public class ServiceManager : IServiceManager
    {
        private readonly Lazy<IProductService> _productService;
        private readonly Lazy<ICartService> _cartService;

        //This should be  cached from an external service at specified intervals

        private readonly IDictionary<Currency, decimal> _currencyConverter = new Dictionary<Currency, decimal>
        {
            { Currency.AUD, 1m },
            { Currency.USD, 0.67m },
            { Currency.NZD, 1.1m },
            { Currency.JPY, 91.28m }
        };

        public ServiceManager(IRepositoryManager repositoryManager)
        {
            _productService = new Lazy<IProductService>((() => new ProductService(repositoryManager.ProductRepository)));
            _cartService = new Lazy<ICartService>((() => new CartService(repositoryManager.ProductRepository, _currencyConverter)));
        }

        public IProductService ProductService => _productService.Value;
        public ICartService CartService => _cartService.Value;
    }
}
