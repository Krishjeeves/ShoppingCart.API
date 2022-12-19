using ShoppingCart.Domain.Enums;

namespace ShoppingCart.Web.Models.Response
{
    public class CartResponse
    {
        public ProductItemResponse[] CartItems { get; set; }
        public decimal ShippingCost { get; set; }
        public Currency Currency { get; set; } = Currency.AUD;
    }
}
