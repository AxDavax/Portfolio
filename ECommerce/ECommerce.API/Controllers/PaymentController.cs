using ECommerce.Application.Payments.Interfaces;
using ECommerce.Contracts.DTO;
using ECommerce.Contracts.DTO.Payment;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ECommerce.API.Controllers
{
    [Authorize]
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
            var session = await _paymentService.CreateCheckoutSessionAsync(order);
            return Ok(session);
        }

        // GET: api/payment/verify/{sessionId}
        [HttpGet("verify/{sessionId}")]
        public async Task<IActionResult> VerifyPayment(string sessionId)
        {
            var order = await _paymentService.VerifyPaymentAsync(sessionId);
            
            if(order is null)
            {
                return Ok(new PaymentVerifyResponse { 
                    Success = false, 
                    Order = null,
                    Message = "Payment verification failed." 
                });
            }

            return Ok(new PaymentVerifyResponse 
            { 
                Success = true, 
                Order = order, 
                Message = "Payment verification succeeded." 
            });
        }
    }
}