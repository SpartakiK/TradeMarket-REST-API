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
    public class CustomerRepository : Repository<Customer>, ICustomerRepository
    {
        public CustomerRepository(TradeMarketDbContext tradeMarketDbContext) : base(tradeMarketDbContext)
        {
        }

        public async Task<IEnumerable<Customer>> GetAllWithDetailsAsync()
        {
            var customers = await _tradeMarketDbContext.Customers
                .Include(x => x.Receipts)
                .ThenInclude(x => x.ReceiptDetails)
                .Include(x => x.Person).ToListAsync();

            return customers;
        }

        public async Task<Customer> GetByIdWithDetailsAsync(int id)
        {
            var customer = await _tradeMarketDbContext.Customers
                .Include(x => x.Receipts)
                .ThenInclude(x => x.ReceiptDetails)
                .Include(x => x.Person)
                .FirstOrDefaultAsync(x => x.Id == id);

            if (customer == null)
            {
                throw new DataBaseException("Customer Not Found!");
            }

            return customer;
        }
    }
}
