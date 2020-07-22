using System.Collections.Generic;
using ECommerce.API.Helpers;
using Microsoft.AspNetCore.Mvc;

namespace ECommerce.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BaseApiController : ControllerBase
    {
        public IActionResult OkWithPagination<T>(int pageIndex, int pageSize, int totalItems, IReadOnlyList<T> data) where T : class
        {
            return Ok(new Pagination<T>(pageIndex, pageSize, totalItems, data));
        }
    }
}