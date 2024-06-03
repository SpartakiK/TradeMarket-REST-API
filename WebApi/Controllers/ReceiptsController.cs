using Business.Interfaces;
using Business.Models;
using Business.Validation;
using Data.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Common;
using System.Threading.Tasks;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReceiptsController : ControllerBase
    {
        private readonly IReceiptService _receiptService;
        public ReceiptsController(IReceiptService receiptService)
        {
            _receiptService = receiptService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ReceiptModel>>> GetAllReceipts()
        {
            var receipts = await _receiptService.GetAllAsync();
            return Ok(receipts);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ReceiptModel>> GetReceiptById(int id) 
        {
            try
            {
                var receipt = await _receiptService.GetByIdAsync(id);
                return Ok(receipt);
            }
            catch (DbException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("{id}/details")]
        public async Task<ActionResult<IEnumerable<ReceiptDetailModel>>> GetReceiptDetails(int id)
        {
            try
            {
                var receiptDetails = await _receiptService.GetReceiptDetailsAsync(id);
                return Ok(receiptDetails);
            }
            catch (DbException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("{id}/sum")]
        public async Task<ActionResult<IEnumerable<ReceiptDetailModel>>> GetReceiptSum(int id)
        {
            try
            {
                var sum = await _receiptService.ToPayAsync(id);
                return Ok(sum);
            }
            catch (DbException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("period")]
        public async Task<ActionResult<IEnumerable<ReceiptDetailModel>>> GetAllByPeriod([FromQuery] DateTime startDate, [FromQuery] DateTime endDate)
        {
            var receipts = await _receiptService.GetReceiptsByPeriodAsync(startDate, endDate);
            return Ok(receipts);
        }

        [HttpPost]
        public async Task<ActionResult> AddReceipt([FromBody] ReceiptModel receiptModel)
        {
            try
            {
                await _receiptService.AddAsync(receiptModel);
                return Ok(receiptModel);
            }
            catch (MarketException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateReceipt([FromBody] ReceiptModel receiptModel)
        {
            await _receiptService.UpdateAsync(receiptModel);
            return Ok(receiptModel);
        }

        [HttpPut("{id}/products/add/{productId}/{quantity}")]
        public async Task<ActionResult> AddProductToReceipt(int id, int productId, int quantity)
        {
            try
            {
                await _receiptService.AddProductAsync(productId, id, quantity);
                return Ok();
            }
            catch (MarketException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("{id}/products/remove/{productId}/{quantity}")]
        public async Task<ActionResult> RemoveProductToReceipt(int id, int productId, int quantity)
        {
            try
            {
                await _receiptService.RemoveProductAsync(productId, id, quantity);
                return Ok();
            }
            catch (MarketException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("{id}/checkout")]
        public async Task<ActionResult> CheckOut(int id)
        {
            try
            {
                await _receiptService.CheckOutAsync(id);
                return Ok();
            }
            catch (MarketException ex)
            {
                return BadRequest(ex.Message);
            } 
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteReceipt(int id)
        {
            try
            {
                await _receiptService.DeleteAsync(id);
                return Ok();
            }
            catch (DbException ex)
            {
                return BadRequest(ex.Message);
            }
        }

    }
}
