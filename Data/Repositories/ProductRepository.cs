using Data.Data;
using Data.Entities;
using Data.Exceptions;
using Data.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Repositories
{
    public class ProductRepository : Repository<Product>, IProductRepository
    {
        public ProductRepository(TradeMarketDbContext tradeMarketDbContext) : base(tradeMarketDbContext)
        {
        }

        public async Task<IEnumerable<Product>> GetAllWithDetailsAsync()
        {
            return await _tradeMarketDbContext.Products
                .Include(x => x.ReceiptDetails)
                .Include(x => x.Category).ToListAsync();
        }

        public async Task<Product> GetByIdWithDetailsAsync(int id)
        {
           var product = await _tradeMarketDbContext.Products
                .Include(x => x.ReceiptDetails)
                .Include(x => x.Category)
                .FirstOrDefaultAsync(x => x.Id == id);
            if (product == null)
            {
                throw new DataBaseException("Product Not Found!");
            }
            return product;
        }
    }
}
