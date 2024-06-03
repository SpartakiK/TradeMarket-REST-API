using Data.Data;
using Data.Entities;
using Data.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Repositories
{
    public class ReceiptRepository : Repository<Receipt>, IReceiptRepository
    {
        public ReceiptRepository(TradeMarketDbContext tradeMarketDbContext) : base(tradeMarketDbContext)
        {
        }

        public async Task<IEnumerable<Receipt>> GetAllWithDetailsAsync()
        {
            return await _tradeMarketDbContext.Receipts
                .Include(x => x.Customer)
                .ThenInclude(x => x.Person)
                .Include(x => x.ReceiptDetails)
                .ThenInclude(x => x.Product)
                .ThenInclude(x => x.Category).ToListAsync();
        }

        public async Task<Receipt> GetByIdWithDetailsAsync(int id)
        {
            var reciept = await _tradeMarketDbContext.Receipts
                .Include(x => x.Customer)
                .ThenInclude(x => x.Person)
                .Include(x => x.ReceiptDetails)
                .ThenInclude(x => x.Product)
                .ThenInclude(x => x.Category).FirstOrDefaultAsync(x => x.Id == id);
            return reciept;
        }
    }
}
