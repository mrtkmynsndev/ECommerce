using System.Threading.Tasks;
using ECommerce.Core.Entities;
using ECommerce.Core.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ECommerce.API.Controllers
{
    [Authorize()]
    public class BasketController : BaseApiController
    {
        private readonly IBasketRepository _basketRepository;

        public BasketController(IBasketRepository basketRepository)
        {
            _basketRepository = basketRepository;

        }

        [HttpGet]
        //[Produces("application/json", Type = typeof(CustomerBasket))]
        [ProducesResponseType(typeof(CustomerBasket), StatusCodes.Status200OK)]
        public async Task<IActionResult> GetBasketById(string id)
        {
            var basket = await _basketRepository.GetBasketAsync(id);

            return Ok(basket ?? new CustomerBasket(id));
        }

        [HttpPost]
        public async Task<IActionResult> UpdateBasketAsync(CustomerBasket basket)
        {
            var updatedBasket = await _basketRepository.UpdateAsync(basket);

            return Ok(updatedBasket);
        }

        [HttpDelete]
        public async Task DeleteBasketAsync(string id) => await _basketRepository.DeleteBasket(id);
    }
}