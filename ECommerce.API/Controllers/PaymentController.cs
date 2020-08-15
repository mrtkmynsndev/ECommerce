using System.IO;
using System.Threading.Tasks;
using ECommerce.API.Dtos;
using ECommerce.API.Errors;
using ECommerce.Core.Entities;
using ECommerce.Core.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Stripe;
using Order = ECommerce.Core.Entities.Orders.Order;

namespace ECommerce.API.Controllers
{
    public class PaymentController : BaseApiController
    {
        private readonly IPaymentService _paymentService;
        private readonly ILogger<IPaymentService> _logger;
        private readonly IConfiguration _configuration;

        public PaymentController(IPaymentService paymentService, ILogger<IPaymentService> logger, IConfiguration configuration)
        {
            _configuration = configuration;
            _logger = logger;
            _paymentService = paymentService;
        }

        [HttpPost("{basketId}")]
        [Authorize]
        public async Task<ActionResult<CustomerBasket>> CreateOrUpdatePayment(string basketId)
        {
            var basket = await _paymentService.CreateOrUpdatePaymentIntent(basketId);

            if (basket == default(CustomerBasket)) return BadRequest(new ApiResponse(400, "Problem occured while getting basket"));

            return Ok(basket);
        }

        [HttpPost("webhook")]
        public async Task<ActionResult> StripeWebHook()
        {
            var json = await new StreamReader(HttpContext.Request.Body).ReadToEndAsync();

            var stripeEvent = EventUtility.ConstructEvent(json, Request.Headers["Stripe-Signature"], _configuration["StripeSettings:WhSecret"]);

            PaymentIntent intent = null;
            Order order = null;

            switch (stripeEvent.Type)
            {
                case "payment_intent.succeeded":
                    intent = (PaymentIntent)stripeEvent.Data.Object;
                    _logger.LogInformation("Paymnet succeed: " + intent.Id);
                    //TODO: Update Order Status
                    order = await _paymentService.UpdatePaymentIntentSucceeded(intent.Id);
                    _logger.LogInformation("Payment Review with Order: " + order.Id);
                    break;
                case "payment_intent.payment_failed":
                    intent = (PaymentIntent)stripeEvent.Data.Object;
                    _logger.LogInformation("Paymnet failed: " + intent.Id);
                    //TODO: Update Order Status
                    order = await _paymentService.UpdatePaymentIntentFailed(intent.Id);
                    _logger.LogInformation("Payment Failed with Order: " + order.Id);
                    break;
            }

            return new EmptyResult();
        }
    }
}