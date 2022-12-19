using ShoppingCart.Domain.Enums;
using System.ComponentModel.DataAnnotations;

namespace ShoppingCart.Web.Models.Request
{
    public class CartRequest
    {
        [Required]
        [MinLength(1)]
        public int[] ProductIds { get; set; }
        [Required]
        public Currency Currency { get; set; } = Currency.AUD;
    }
}
