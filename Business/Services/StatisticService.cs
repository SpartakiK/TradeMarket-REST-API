using AutoMapper;
using Business.Interfaces;
using Business.Models;
using Data.Entities;
using Data.Interfaces;
using Data.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Services
{
    public class StatisticService : IStatisticService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IReceiptRepository _receiptRepository;
        private readonly IReceiptDetailRepository _receiptDetailRepository;
        private readonly IMapper _mapper;
        public StatisticService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _receiptRepository = _unitOfWork.ReceiptRepository;
            _receiptDetailRepository = _unitOfWork.ReceiptDetailRepository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<ProductModel>> GetCustomersMostPopularProductsAsync(int productCount, int customerId)
        {

            var receipts = await _receiptRepository.GetAllWithDetailsAsync();
            var products = receipts
                .Where(x => x.CustomerId == customerId)
                .SelectMany(x => x.ReceiptDetails.Select(z => new { Product = z.Product, Quantity = z.Quantity }))
                .OrderByDescending(x => x.Quantity)
                .Select(x => x.Product)
                .Take(productCount);

            return products.Select(x => _mapper.Map<ProductModel>(x));
        }

        public async Task<decimal> GetIncomeOfCategoryInPeriod(int categoryId, DateTime startDate, DateTime endDate)
        {
            var receipts = (await _receiptRepository.GetAllWithDetailsAsync())
                .Where(x => x.OperationDate > startDate && x.OperationDate < endDate);

            var products = receipts
                .SelectMany(x => x.ReceiptDetails.Select(z => new
                {
                    product = z.Product,
                    sumsales = z.DiscountUnitPrice * z.Quantity
                }))
                .Where(x => x.product.ProductCategoryId == categoryId);

            decimal result = 0;
            foreach (var x in products)
            {
                result += x.sumsales;
            }

            return result;
        }

        public async Task<IEnumerable<ProductModel>> GetMostPopularProductsAsync(int productCount)
        {
            var receiptDetails = await _receiptDetailRepository.GetAllWithDetailsAsync();
            var products = receiptDetails
                .GroupBy(x => x.Product)
                .OrderByDescending(x => x.Sum(x => x.Quantity))
                .Select(x => x.Key)
                .Take(productCount);

            return products.Select(x => _mapper.Map<ProductModel>(x));
        }

        public async Task<IEnumerable<CustomerActivityModel>> GetMostValuableCustomersAsync(int customerCount, DateTime startDate, DateTime endDate)
        {
            var receipts = await _unitOfWork.ReceiptRepository.GetAllWithDetailsAsync();
            var filtered = receipts.Where(x => x.OperationDate >= startDate && x.OperationDate <= endDate)
                .GroupBy(x => x.Customer)
                .Select(g => new
                {
                    Customer = g.Key,
                    TotalExpenses = g.Sum(x => x.ReceiptDetails.Sum(rd => rd.Quantity * rd.DiscountUnitPrice))
                })
                .OrderByDescending(x => x.TotalExpenses)
                .Take(customerCount);

            return filtered.Select(x => new CustomerActivityModel(x.Customer.Id, x.Customer.Person.Name + " " + x.Customer.Person.Surname, x.TotalExpenses));
            
        }
    }
}
