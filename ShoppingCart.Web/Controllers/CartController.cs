using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ShoppingCart.Web.Models.Request;
using ShoppingCart.Web.Services.Abstraction;

namespace ShoppingCart.Controllers.Web
{
    [Route("api/[controller]")]
    [ApiController]
    public class CartController : ControllerBase
    {
        private readonly IServiceManager _serviceManager;

        public CartController(IServiceManager serviceManager)
        {
            _serviceManager = serviceManager;
        }

        [HttpPost]
        public async Task<IActionResult> Post(CartRequest cartRequest)
        {
            return Ok(await _serviceManager.CartService.GetCartItemsAsync(cartRequest));
        }
    }
}
