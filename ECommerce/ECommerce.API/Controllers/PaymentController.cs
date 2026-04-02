using ECommerce.Application.DTO;
using ECommerce.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace ECommerce.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PaymentController : ControllerBase
    {
        private readonly IPaymentService _paymentService;

        public PaymentController(IPaymentService paymentService)
        {
            _paymentService = paymentService;
        }

        // POST: api/payment/create-session
        [HttpPost("create-session")]
        public async Task<IActionResult> CreateCheckoutSession([FromBody] OrderHeaderDTO order)
        {
            var url = await _paymentService.CreateCheckoutSessionAsync(order);
            return Ok(new { url });
        }

        // GET: api/payment/verify/{sessionId}
        [HttpGet("verify/{sessionId}")]
        public async Task<IActionResult> VerifyPayment(string sessionId)
        {
            var success = await _paymentService.VerifyPaymentAsync(sessionId);
            return Ok(new { success });
        }
    }
}
