using MockQueryable.Moq;
using Moq;
using ShoppingCart.Domain.Entities;
using ShoppingCart.Domain.Enums;
using ShoppingCart.Domain.Repositories;
using ShoppingCart.Web.Models.Request;
using ShoppingCart.Web.Services;
using ShoppingCart.Web.Services.Abstraction;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace ShoppingCart.Web.Tests
{
    public class CartServiceTest
    {
        private readonly ICartService _cartService;
        private List<Product> Products { get; set; }
        private readonly IDictionary<Currency, decimal> _currencyConverter; 

        public CartServiceTest()
        {
            _currencyConverter = new Dictionary<Currency, decimal>();
            Products = new List<Product>();
            var mock = Products.AsQueryable().BuildMock();

            var productRepository = new Mock<IProductRepository>();

            productRepository.Setup(p => p.GetQuery()).Returns(mock);
            _cartService = new CartService(productRepository.Object, _currencyConverter);
        }

        [Fact]
        public async Task When_Empty_Cart_Shipping_Cost_Is_Zero()
        {
            var cartRequest = new CartRequest();

            var cartResponse = await _cartService.GetCartItemsAsync(cartRequest);

            Assert.Equal(0m, cartResponse.ShippingCost);
        }

        [Fact]
        public async Task When_Total_Less_Than_50_Shipping_Cost_Is_10()
        {
            Products.AddRange(new[] { new Product {
                Id = 1, Price = 37.5m
            }, new Product { Id = 2, Price = 12.49m } });
            var cartRequest = new CartRequest { ProductIds = Products.Select(c => c.Id).ToArray() };

            var cartResponse = await _cartService.GetCartItemsAsync(cartRequest);

            Assert.Equal(10m, cartResponse.ShippingCost);
        }

        [Fact]
        public async Task When_Total_Equal_50_Shipping_Cost_Is_20()
        {
            Products.AddRange(new[] { new Product {
                Id = 1, Price = 37.5m
            }, new Product { Id = 2, Price = 12.5m } });
            var cartRequest = new CartRequest { ProductIds = Products.Select(c => c.Id).ToArray() };

            var cartResponse = await _cartService.GetCartItemsAsync(cartRequest);

            Assert.Equal(20m, cartResponse.ShippingCost);
        }

        [Fact]
        public async Task When_Total_More_Than_50_Shipping_Cost_Is_20()
        {
            Products.AddRange(new[] { new Product {
                Id = 1, Price = 37.5m
            }, new Product { Id = 2, Price = 12.51m } });
            var cartRequest = new CartRequest { ProductIds = Products.Select(c => c.Id).ToArray() };

            var cartResponse = await _cartService.GetCartItemsAsync(cartRequest);

            Assert.Equal(20m, cartResponse.ShippingCost);
        }

        [Theory]
        [InlineData(30.75, 0.65, Currency.USD, 19.99)]
        [InlineData(50.75, 0.65, Currency.AUD, 50.75)]
        [InlineData(80.32, 91.38, Currency.JPY, 7339.65)]
        [InlineData(50, 1.08, Currency.NZD, 54)]
        public async Task When_Currency_Cart_Request_Then_Price_In_New_Currency(decimal priceinAUD, decimal conversion, Currency currency, decimal expected)
        {
            Products.Add(new Product { Id = 1, Price = priceinAUD });
            _currencyConverter.Add(currency, conversion);
            var cartRequest = new CartRequest { ProductIds = new[] { 1 }, Currency = currency };

            var cartResponse = await _cartService.GetCartItemsAsync(cartRequest);

            Assert.Equal(expected, cartResponse.CartItems.First().Price);
        }

        [Theory]
        [InlineData(30.75, 0.65, Currency.USD, 6.5)]
        [InlineData(50.75, 0.65, Currency.AUD, 20)]
        [InlineData(80.32, 91.38, Currency.JPY, 1827.6)]
        [InlineData(49.99, 1.08, Currency.NZD, 10.8)]
        public async Task When_Currency_Cart_Request_Then_Shipping_Price_In_New_Currency(decimal priceinAUD, decimal conversion, Currency currency, decimal expected)
        {
            Products.Add(new Product { Id = 1, Price = priceinAUD });
            _currencyConverter.Add(currency, conversion);
            var cartRequest = new CartRequest { ProductIds = new[] { 1 }, Currency = currency };

            var cartResponse = await _cartService.GetCartItemsAsync(cartRequest);

            Assert.Equal(expected, cartResponse.ShippingCost);
        }
    }
}
