using Data.Interfaces;
using Data.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;

namespace Data.Data
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly TradeMarketDbContext _context;
        private readonly CustomerRepository _customerRepository;
        private readonly PersonRepository _personRepository;
        private readonly ProductRepository _productRepository;
        private readonly ProductCategoryRepository _productCategoryRepository;
        private readonly ReceiptRepository _receiptRepository;
        private readonly ReceiptDetailRepository _receiptDetailRepository;
        public UnitOfWork(TradeMarketDbContext context)
        {
            _context = context;
            _customerRepository = new CustomerRepository(_context);
            _personRepository = new PersonRepository(_context);
            _productRepository = new ProductRepository(_context);
            _productCategoryRepository = new ProductCategoryRepository(_context);
            _receiptDetailRepository = new ReceiptDetailRepository(_context);
            _receiptRepository = new ReceiptRepository(_context);

        }
        public ICustomerRepository CustomerRepository => _customerRepository;

        public IPersonRepository PersonRepository => _personRepository;

        public IProductRepository ProductRepository => _productRepository;

        public IProductCategoryRepository ProductCategoryRepository => _productCategoryRepository;

        public IReceiptRepository ReceiptRepository => _receiptRepository;

        public IReceiptDetailRepository ReceiptDetailRepository => _receiptDetailRepository;

        public async Task SaveAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
