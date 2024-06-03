using AutoMapper;
using Business.Interfaces;
using Business.Models;
using Business.Validation;
using Data.Entities;
using Data.Interfaces;
using Data.Repositories;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Services
{
    public class ReceiptService : IReceiptService
    {
        private readonly IReceiptRepository _receiptRepository;
        private readonly IReceiptDetailRepository _receiptDetailRepository;
        private readonly IProductRepository _productRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public ReceiptService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _receiptRepository = _unitOfWork.ReceiptRepository;
            _receiptDetailRepository = _unitOfWork.ReceiptDetailRepository;
            _productRepository = _unitOfWork.ProductRepository;
        }

        public async Task AddAsync(ReceiptModel model)
        {
            var receipt = _mapper.Map<Receipt>(model);
            await _receiptRepository.AddAsync(receipt);
            await _unitOfWork.SaveAsync();
        }

        public async Task AddProductAsync(int productId, int receiptId, int quantity)
        {
            var receipt = await _receiptRepository.GetByIdWithDetailsAsync(receiptId);
            if (receipt == null)
            {
                throw new MarketException("Reciept Does Not Exist!");
            }
            if (receipt.ReceiptDetails == null)
            {
                var product = await _productRepository.GetByIdAsync(productId);
                if (product == null)
                {
                    throw new MarketException("Product Does Not Exist!");
                }

                var newReceiptDetail = new ReceiptDetail
                {
                    ProductId = productId,
                    ReceiptId = receiptId,
                    Quantity = quantity,
                    DiscountUnitPrice = product.Price - (receipt.Customer.DiscountValue * product.Price) / 100,
                    UnitPrice = product.Price,
                };
                await _receiptDetailRepository.AddAsync(newReceiptDetail);
                await _unitOfWork.SaveAsync();
                return;
            }

            var receiptDetail = receipt.ReceiptDetails.FirstOrDefault(x => x.ProductId == productId);
            if (receiptDetail != null)
            {
                receiptDetail.Quantity += quantity;
            }
            else
            {
                var product = await _productRepository.GetByIdAsync(productId);
                if (product == null)
                {
                    throw new MarketException("Product Does Not Exist!");
                }

                var newReceiptDetail = new ReceiptDetail
                {
                    ProductId = productId,
                    ReceiptId = receiptId,
                    Quantity = quantity,
                    DiscountUnitPrice = product.Price - (receipt.Customer.DiscountValue * product.Price) / 100,
                    UnitPrice = product.Price,
                };
                await _receiptDetailRepository.AddAsync(newReceiptDetail);
            }

            await _unitOfWork.SaveAsync();
        }

        public async Task CheckOutAsync(int receiptId)
        {
            var receipt = await _receiptRepository.GetByIdAsync(receiptId);
            receipt.IsCheckedOut = true;
            await Task.Run(() => _receiptRepository.Update(receipt));
            await _unitOfWork.SaveAsync();
        }

        public async Task DeleteAsync(int modelId)
        {
            var receipt = await _receiptRepository.GetByIdWithDetailsAsync(modelId);
            foreach (var item in receipt.ReceiptDetails)
            {
                _receiptDetailRepository.Delete(item);
            }

            _receiptRepository.Delete(receipt);
            await _receiptRepository.DeleteByIdAsync(modelId);
            await _unitOfWork.SaveAsync();
        }

        public async Task<IEnumerable<ReceiptModel>> GetAllAsync()
        {
            var receiptModels = await _receiptRepository.GetAllWithDetailsAsync();
            return receiptModels.Select(x => _mapper.Map<ReceiptModel>(x));
        }

        public async Task<ReceiptModel> GetByIdAsync(int id)
        {
            var receipt = await _receiptRepository.GetByIdWithDetailsAsync(id);
            return _mapper.Map<ReceiptModel>(receipt);
        }

        public async Task<IEnumerable<ReceiptDetailModel>> GetReceiptDetailsAsync(int receiptId)
        {
            var receiptDetails = (await _receiptRepository.GetByIdWithDetailsAsync(receiptId)).ReceiptDetails;
            return receiptDetails.Select(x => _mapper.Map<ReceiptDetailModel>(x));
        }

        public async Task<IEnumerable<ReceiptModel>> GetReceiptsByPeriodAsync(DateTime startDate, DateTime endDate)
        {
            var receipts = (await _receiptRepository.GetAllWithDetailsAsync()).Where(x => x.OperationDate > startDate && x.OperationDate < endDate);
            return receipts.Select(x => _mapper.Map<ReceiptModel>(x));
        }

        public async Task RemoveProductAsync(int productId, int receiptId, int quantity)
        {
            var receipt = await _receiptRepository.GetByIdWithDetailsAsync(receiptId);
            var receiptDetail = receipt.ReceiptDetails.FirstOrDefault(x => x.ProductId == productId);
            if (receiptDetail == null) 
            {
                throw new MarketException("Invalid Data Given!");
            }
            if (receiptDetail.Quantity - quantity <= 0)
            {
                _receiptDetailRepository.Delete(receiptDetail);
                await _unitOfWork.SaveAsync();
                return;
            }

            receiptDetail.Quantity -= quantity;
            await _unitOfWork.SaveAsync();
        }

        public async Task<decimal> ToPayAsync(int receiptId)
        {
            var receipt = await _receiptRepository.GetByIdWithDetailsAsync(receiptId);
            decimal toPay = 0;
            foreach (var item in receipt.ReceiptDetails)
            {
                toPay += item.DiscountUnitPrice * item.Quantity;
            }

            return toPay;
        }

        public async Task UpdateAsync(ReceiptModel model)
        {
            await Task.Run(() => _receiptRepository.Update(_mapper.Map<Receipt>(model)));
            await _unitOfWork.SaveAsync();
        }
    }
}
