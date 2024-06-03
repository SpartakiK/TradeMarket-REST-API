using Business.Models;
using Microsoft.AspNetCore.Mvc;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StatisticsController : ControllerBase
    {
        [HttpGet("customers/{id}")]
        public async Task<ActionResult<IEnumerable<ProductModel>>> GetCustomersMostPopularProducts(int customerId)
        {
            return Ok();
        }

        [HttpGet("categories/{id}")]
        public async Task<ActionResult<decimal>> GetIncomeOfCategoryInPeriod(int categoryId) 
        {
            return Ok();
        }

        [HttpGet("{count}")]
        public async Task<ActionResult<IEnumerable<ProductModel>>> GetMostPopularProductsAsync(int productCount)
        {
            return Ok();
        }


    }
}
