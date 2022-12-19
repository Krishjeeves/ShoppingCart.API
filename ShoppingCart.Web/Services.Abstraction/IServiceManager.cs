namespace ShoppingCart.Web.Services.Abstraction
{
    public interface IServiceManager
    {
       IProductService ProductService { get; }
       ICartService CartService { get; }
    }
}
