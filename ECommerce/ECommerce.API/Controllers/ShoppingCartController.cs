using ECommerce.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace ECommerce.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ShoppingCartController : ControllerBase
    {
        private readonly IShoppingCartService _cartService;

        public ShoppingCartController(IShoppingCartService cartService)
        {
            _cartService = cartService;
        }

        // GET: api/shoppingcart/user123
        [HttpGet("{userId}")]
        public async Task<IActionResult> GetAll(string userId)
        {
            var items = await _cartService.GetAllAsync(userId);
            return Ok(items);
        }

        // GET: api/shoppingcart/user123/product/5
        [HttpGet("{userId}/product/{productId:int}")]
        public async Task<IActionResult> GetItem(string userId, int productId)
        {
            var item = await _cartService.GetItemAsync(userId, productId);
            if (item == null)
                return NotFound();

            return Ok(item);
        }

        // PUT: api/shoppingcart/user123/product/5?updateBy=1
        [HttpPut("{userId}/product/{productId:int}")]
        public async Task<IActionResult> Update(string userId, int productId, [FromQuery] int updateBy)
        {
            var success = await _cartService.UpdateCartAsync(userId, productId, updateBy);

            if (!success)
                return BadRequest("Unable to update cart.");

            return NoContent();
        }

        // DELETE: api/shoppingcart/user123
        [HttpDelete("{userId}")]
        public async Task<IActionResult> Clear(string userId)
        {
            var success = await _cartService.ClearCartAsync(userId);

            if (!success)
                return BadRequest("Unable to clear cart.");

            return NoContent();
        }

        // GET: api/shoppingcart/user123/count
        [HttpGet("{userId}/count")]
        public async Task<IActionResult> GetTotalCount(string userId)
        {
            var count = await _cartService.GetTotalCartCountAsync(userId);
            return Ok(count);
        }
    }
}