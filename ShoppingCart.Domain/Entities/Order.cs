using ShoppingCart.Domain.Enums;

namespace ShoppingCart.Domain.Entities
{
    public class Order
    {
        public int Id { get; set; }
        public OrderState State { get; set; }
        public DateTimeOffset CreatedAtUtc { get; set; }
    }
}
