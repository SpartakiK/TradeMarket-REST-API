using Business.Interfaces;
using Business.Models;
using Business.Services;
using Business.Validation;
using Data.Exceptions;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IProductService _productService;
        public ProductsController(IProductService productService)
        {
            _productService = productService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProductModel>>> GetByFilter([FromQuery]int? categoryId, [FromQuery]int? minPrice, [FromQuery] int? maxPrice)
        {
            if (categoryId != 00 || minPrice != 0 || maxPrice != 0)
            {
                var filter = new FilterSearchModel();
                filter.CategoryId = categoryId;
                filter.MinPrice = minPrice;
                filter.MaxPrice = maxPrice;
                var filteredProducts = await _productService.GetByFilterAsync(filter);
                return Ok(filteredProducts);
            }

            var products = await _productService.GetAllAsync();
            return Ok(products);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ProductModel>> GetProductById(int id)
        {
            try
            {
                var product = await _productService.GetByIdAsync(id);
                return Ok(product);
            }
            catch (DataBaseException ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpPost]
        public async Task<ActionResult> AddProduct([FromBody]ProductModel product)
        {
            try
            {
                await _productService.AddAsync(product);
                return Ok(product);
            }
            catch (MarketException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateProduct([FromBody] ProductModel model)
        {
            try
            {
                await _productService.UpdateAsync(model);
                return Ok(model);
            }
            catch (MarketException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("categories/{id}")]
        public async Task<ActionResult> UpdateCategory(int id, [FromBody] ProductCategoryModel model)
        {
            await _productService.UpdateCategoryAsync(model);
            return Ok(model);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteProduct(int id)
        {
            await _productService.DeleteAsync(id);
            return Ok();
        }

        [HttpGet("categories")]
        public async Task<ActionResult<IEnumerable<ProductCategoryModel>>> GetAllCategories()
        {
            var productCategories = await _productService.GetAllProductCategoriesAsync();
            return Ok(productCategories);
        }

        [HttpPost("categories")]
        public async Task<ActionResult> AddProductCategory([FromBody] ProductCategoryModel productCategory)
        {
            try
            {
                await _productService.AddCategoryAsync(productCategory);
                return Ok(productCategory);
            }
            catch (DataBaseException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("categories/{id}")]
        public async Task<ActionResult> DeleteProductCategory(int id)
        {
            await _productService.RemoveCategoryAsync(id);
            return Ok();
        }
    }
}
