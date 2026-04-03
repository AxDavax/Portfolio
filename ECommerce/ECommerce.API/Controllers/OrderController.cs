using ECommerce.Contracts.DTO;
using ECommerce.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace ECommerce.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OrderController : ControllerBase
    {
        private readonly IOrderService _orderService;

        public OrderController(IOrderService orderService)
        {
            _orderService = orderService;
        }

        // GET: api/order
        // GET: api/order?userId=abc123
        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] string? userId = null)
        {
            var orders = await _orderService.GetAllAsync(userId);
            return Ok(orders);
        }

        // GET: api/order/5
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            var order = await _orderService.GetByIdAsync(id);
            if (order == null)
                return NotFound();

            return Ok(order);
        }

        // GET: api/order/session/xyz123
        [HttpGet("session/{sessionId}")]
        public async Task<IActionResult> GetBySessionId(string sessionId)
        {
            var order = await _orderService.GetOrderBySessionIdAsync(sessionId);
            if (order == null)
                return NotFound();

            return Ok(order);
        }

        // POST: api/order
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] OrderHeaderDTO dto)
        {
            var created = await _orderService.CreateAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
        }

        // PUT: api/order/5/status
        [HttpPut("{orderId:int}/status")]
        public async Task<IActionResult> UpdateStatus(
            int orderId,
            [FromQuery] string status,
            [FromQuery] string? paymentIntentId = null)
        {
            var updated = await _orderService.UpdateStatusAsync(orderId, status, paymentIntentId);

            if (updated == null)
                return NotFound();

            return Ok(updated);
        }
    }
}
