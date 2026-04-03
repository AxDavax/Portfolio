using ECommerce.Contracts.DTO;
using ECommerce.Application.Mappings;
using ECommerce.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace ECommerce.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CartController : ControllerBase
    {
        private readonly ICartToOrder _cartToOrder;

        public CartController(ICartToOrder cartToOrder)
        {
            _cartToOrder = cartToOrder;
        }

        // POST: api/cart/convert
        [HttpPost("convert")]
        public IActionResult ConvertCartToOrderDetails([FromBody] List<ShoppingCartDTO> carts)
        {
            if (carts == null || carts.Count == 0)
                return BadRequest("Cart is empty.");

            var orderDetails = _cartToOrder.ConvertShoppingCartToOrderDetails(
                carts.Select(c => c.ToDomain()).ToList()
            );

            var result = orderDetails.Select(od => od.ToDTO()).ToList();

            return Ok(result);
        }
    }
}