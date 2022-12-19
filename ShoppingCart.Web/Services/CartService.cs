using ShoppingCart.Domain.Enums;
using ShoppingCart.Domain.Repositories;
using ShoppingCart.Web.Models.Request;
using ShoppingCart.Web.Models.Response;
using ShoppingCart.Web.Services.Abstraction;

namespace ShoppingCart.Web.Services
{
    public class CartService : ICartService
    {
        private readonly IProductRepository _productRepository;

        //This should be  cached from an external service at specified intervals
        private readonly IDictionary<Currency, decimal> _currencyConverter;

        public CartService(IProductRepository productRepository, IDictionary<Currency, decimal> currencyConverter)
        {
            _productRepository = productRepository;
            _currencyConverter = currencyConverter;
        }

        public async Task<CartResponse> GetCartItemsAsync(CartRequest cartRequest)
        {
            var response = new CartResponse { Currency = cartRequest.Currency };

            if (cartRequest.ProductIds?.Count() > 0)
            {
                var products = _productRepository.GetQuery().Where(c => cartRequest.ProductIds.Contains(c.Id)).AsEnumerable();


                response.CartItems = products.Select(p => new ProductItemResponse
                {
                    Id = p.Id,
                    Name = p.Name,
                    Description = p.Description,
                    Price = GetConvertedPrice(p.Price, cartRequest.Currency),
                    Image = p.Image
                }).ToArray();

                response.ShippingCost = GetConvertedPrice(GetShippingCost(products.Sum(c => c.Price)), cartRequest.Currency);
            }
            return response;
        }

        private decimal GetConvertedPrice(decimal price, Currency currency)
        {
            if(currency == Currency.AUD)
                return price;

            var priceInCents = price * 100 * _currencyConverter[currency];

            return Math.Round((Math.Ceiling(priceInCents) / 100m),2) ;
        }

        private decimal GetShippingCost(decimal totalPrice)
        {
            return totalPrice < 50 ? 10m : 20m;               
        }
    }
}
