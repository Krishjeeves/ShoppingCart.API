using ShoppingCart.Domain.Repositories;

namespace ShoppingCart.DataAccess.Repositories
{
    public class RepositoryManager : IRepositoryManager
    {
        private readonly Lazy<IProductRepository> _productRepository;

        public RepositoryManager()
        {
            _productRepository =  new Lazy<IProductRepository>(() => new ProductRepository());
        }

        public IProductRepository ProductRepository => _productRepository.Value;
    }
}
