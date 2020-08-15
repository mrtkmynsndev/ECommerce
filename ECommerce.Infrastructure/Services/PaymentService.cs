using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ECommerce.Core.Entities;
using ECommerce.Core.Entities.Orders;
using ECommerce.Core.Interfaces;
using ECommerce.Core.Specifications;
using Microsoft.Extensions.Configuration;
using Stripe;
using Product = ECommerce.Core.Entities.Product;
using Order = ECommerce.Core.Entities.Orders.Order;

namespace ECommerce.Infrastructure.Services
{
    public class PaymentService : IPaymentService
    {
        private readonly IBasketRepository _basketRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IConfiguration _configuration;
        public PaymentService(IBasketRepository basketRepository, IUnitOfWork unitOfWork, IConfiguration configuration)
        {
            _configuration = configuration;
            _unitOfWork = unitOfWork;
            _basketRepository = basketRepository;
        }

        public async Task<CustomerBasket> CreateOrUpdatePaymentIntent(string id)
        {
            StripeConfiguration.ApiKey = _configuration["StripeSettings:SecretKey"];

            var basket = await _basketRepository.GetBasketAsync(id);

            if (basket == default(CustomerBasket)) return default(CustomerBasket);

            var shippingPrice = 0.0m;


            if (basket.DeliveryMethodId.HasValue)
            {
                var deliveryMethodRepository = _unitOfWork.Repository<DeliveryMethod>();

                var deliveryMethod = await deliveryMethodRepository.GetByIdAsync(basket.DeliveryMethodId.Value);

                shippingPrice = deliveryMethod.Price;
            }

            // accurate the price
            foreach (var item in basket.Items)
            {
                var productItem = await _unitOfWork.Repository<Product>().GetByIdAsync(item.Id);

                if (item.Price != productItem.Price)
                {
                    item.Price = productItem.Price;
                }
            }

            var service = new PaymentIntentService();

            PaymentIntent intent = null;

            var calculatedAmount = (long)basket.Items.Sum(x => x.Quantity * (x.Price * 100)) + (long)shippingPrice * 100;

            if (string.IsNullOrEmpty(basket.PaymentIntentId))
            {
                //create option
                var createOptions = new PaymentIntentCreateOptions()
                {
                    Amount = calculatedAmount,
                    Currency = "usd",
                    PaymentMethodTypes = new List<string> { "card" }
                };

                intent = await service.CreateAsync(createOptions);

                basket.PaymentIntentId = intent.Id;
                basket.ClientSecret = intent.ClientSecret;
            }
            else
            {
                // update option
                var updateOptions = new PaymentIntentUpdateOptions()
                {
                    Amount = calculatedAmount
                };

                await service.UpdateAsync(basket.PaymentIntentId, updateOptions);
            }

            await _basketRepository.UpdateAsync(basket);

            return basket;
        }

        public async Task<Core.Entities.Orders.Order> UpdatePaymentIntentFailed(string paymentIntentId)
        {
            var spec = new OrderByPaymentIntentIdWithSpecification(paymentIntentId);

            var order = await _unitOfWork.Repository<Order>().GetEntityWithSpecAsync(spec);

            if (order == default(Order)) return null;

            order.ChangeStatus(OrderStatus.PaymentFailed);
            
            _unitOfWork.Repository<Order>().Update(order);

            await _unitOfWork.SaveAsync(new System.Threading.CancellationToken());

            return order;
        }

        public async Task<Core.Entities.Orders.Order> UpdatePaymentIntentSucceeded(string paymentIntentId)
        {
            var spec = new OrderByPaymentIntentIdWithSpecification(paymentIntentId);

            var order = await _unitOfWork.Repository<Order>().GetEntityWithSpecAsync(spec);

            if (order == default(Order)) return null;

            order.ChangeStatus(OrderStatus.PaymentRecevied);

            _unitOfWork.Repository<Order>().Update(order);

            await _unitOfWork.SaveAsync(new System.Threading.CancellationToken());

            return order;
        }
    }
}