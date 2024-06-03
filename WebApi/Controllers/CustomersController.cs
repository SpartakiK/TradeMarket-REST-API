using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Business.Interfaces;
using Business.Models;
using Business.Validation;
using Data.Exceptions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomersController : ControllerBase
    {
        private readonly ICustomerService _customerService;
        public CustomersController(ICustomerService customerService)
        {
            _customerService = customerService;
        }

        // GET: api/customers
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CustomerModel>>> Get()
        {
            var customers = await _customerService.GetAllAsync();
            return Ok(customers);
        }

        //GET: api/customers/1
        [HttpGet("{id}")]
        public async Task<ActionResult<CustomerModel>> GetById(int id)
        {
            try
            {
                var customer = await _customerService.GetByIdAsync(id);
                return Ok(customer);
            }
            catch (DataBaseException ex)
            {
                return NotFound(ex.Message);
            }
        }
        
        //GET: api/customers/products/1
        [HttpGet("products/{id}")]
        public async Task<ActionResult<CustomerModel>> GetByProductId(int id)
        {
            var customer = await _customerService.GetCustomersByProductIdAsync(id);
            return Ok(customer);
        }

        // POST: api/customers
        [HttpPost]
        public async Task<ActionResult> Post([FromBody] CustomerModel value)
        {
            try
            {
                await _customerService.AddAsync(value);
                return Ok(value);
            }
            catch (MarketException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // PUT: api/customers/1
        [HttpPut("{id}")]
        public async Task<ActionResult> Put(int id, [FromBody] CustomerModel value)
        {
            try
            {
                await _customerService.UpdateAsync(value);
                return Ok(value);
            }
            catch (MarketException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // DELETE: api/customers/1
        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            await _customerService.DeleteAsync(id);
            return Ok();
        }
    }
}
