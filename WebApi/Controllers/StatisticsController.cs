using Business.Interfaces;
using Business.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StatisticsController : ControllerBase
    {
        private readonly IStatisticService _statisticService;
        public StatisticsController(IStatisticService statisticService) 
        {
            _statisticService = statisticService;
        }

        [HttpGet("popularProducts")]
        public async Task<ActionResult<IEnumerable<ProductModel>>> GetMostPopularProductsAsync([FromQuery] int productCount)
        {
            var products = await _statisticService.GetMostPopularProductsAsync(productCount);
            return Ok(products);
        }

        [HttpGet("customer/{id}/{productCount}")]
        public async Task<ActionResult<IEnumerable<ProductModel>>> GetCustomersMostPopularProducts(int id, int productCount)
        {
            var products = await _statisticService.GetCustomersMostPopularProductsAsync(productCount, id);
            return Ok(products);
        }

        [HttpGet("activity/{customerCount}")]
        public async Task<ActionResult<IEnumerable<CustomerModel>>> GetMostActiveCustomers(int customerCount, [FromQuery] DateTime startDate, [FromQuery] DateTime endDate)
        {
            var customers = await _statisticService.GetMostValuableCustomersAsync(customerCount, startDate, endDate);
            return Ok(customers);
        }

        [HttpGet("income/{categoryId}")]
        public async Task<ActionResult<decimal>> GetIncomeOfCategoryInPeriod(int categoryId, [FromQuery] DateTime startDate, [FromQuery] DateTime endDate) 
        {
           var income = await _statisticService.GetIncomeOfCategoryInPeriod(categoryId, startDate, endDate);
           return Ok(income);
        }


    }
}
