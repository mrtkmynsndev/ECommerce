using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using ECommerce.API.Dtos;
using ECommerce.API.Errors;
using ECommerce.API.Extensions;
using ECommerce.Core.Entities.Orders;
using ECommerce.Core.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ECommerce.API.Controllers
{
    [Authorize]
    public class OrderController : BaseApiController
    {
        private readonly IOrderService _orderService;
        private readonly IMapper _mapper;
        public OrderController(IOrderService orderService, IMapper mapper)
        {
            _mapper = mapper;
            _orderService = orderService;
        }

        [HttpPost]
        public async Task<ActionResult<OrderDto>> CreateOrder([FromBody] OrderDto order)
        {
            var userName = HttpContext.User.GetUserName();

            var shipToAdress = _mapper.Map<AddressDto, Adress>(order.ShippToAdress);

            var createdOrder = await _orderService.CreateOrderAsync(userName, order.DeliveryMethodId, order.BasketId, shipToAdress);

            if (createdOrder == default(Order)) return BadRequest(new ApiResponse(400, "Problem while creating order"));

            return Ok(createdOrder);
        }

        [HttpGet]
        public async Task<ActionResult<IReadOnlyList<OrderResponseDto>>> GetOrders()
        {
            var userName = HttpContext.User.GetUserName();

            var orders = await _orderService.GetOrdersByUserNameAsync(userName);

            return Ok(_mapper.Map<IReadOnlyList<Order>, IReadOnlyList<OrderResponseDto>>(orders));
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<OrderResponseDto>> GetOrder(int id)
        {
            var userName = HttpContext.User.GetUserName();

            var order = await _orderService.GetOrderByIdAsync(id, userName);

            return Ok(_mapper.Map<Order, OrderResponseDto>(order));
        }
        
        [HttpGet("deliverymethods")]
        public async Task<ActionResult<DeliveryMethod>> GetDeliveryMethods()
        {
            return Ok(await _orderService.GetDeliveryMethodsAsync());
        }
    }
}