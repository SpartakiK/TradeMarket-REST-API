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
    public class ReceiptDetailRepository : Repository<ReceiptDetail>, IReceiptDetailRepository
    {
        public ReceiptDetailRepository(TradeMarketDbContext tradeMarketDbContext) : base(tradeMarketDbContext)
        {
        }

        public async Task<IEnumerable<ReceiptDetail>> GetAllWithDetailsAsync()
        {
            return await _tradeMarketDbContext.ReceiptsDetails
                .Include(x => x.Receipt)
                .ThenInclude(x => x.Customer)
                .Include(x => x.Product)
                .ThenInclude(x => x.Category).ToListAsync();
        }
    }
}
