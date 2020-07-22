using ECommerce.API.Errors;
using ECommerce.Core.Entities;
using ECommerce.Infrastructure.Data;
using Microsoft.AspNetCore.Mvc;

namespace ECommerce.API.Controllers
{
    public class BuggyController : BaseApiController
    {
        private readonly ECommerceContext _context;
        public BuggyController(ECommerceContext context)
        {
            _context = context;
        }

        [HttpGet("notfound")]
        public IActionResult GetNotFoundRequest()
        {
            var product = _context.Products.Find(51);

            if (product == default(Product))
                return NotFound(new ApiResponse(400));

            return Ok(product);
        }

        [HttpGet("notfound/{id}")]
        public IActionResult GetNotFoundRequest(int id)
        {
            return Ok();
        }

        [HttpGet("servererror")]
        public IActionResult GetServerError()
        {
            var product = _context.Products.Find(51);
            var productStr = product.ToString();
            return Ok();
        }

        [HttpGet("badrequest")]
        public IActionResult GetBadRequest(){
            return BadRequest(new ApiResponse(400));
        }
    }
}