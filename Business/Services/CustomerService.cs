using AutoMapper;
using AutoMapper.Configuration.Annotations;
using Business.Interfaces;
using Business.Models;
using Business.Validation;
using Data.Data;
using Data.Entities;
using Data.Exceptions;
using Data.Interfaces;
using Data.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Services
{
    public class CustomerService : ICustomerService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ICustomerRepository _customerRepository;

        public CustomerService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _customerRepository = _unitOfWork.CustomerRepository;
        }

        public async Task AddAsync(CustomerModel model)
        {
            ValidateCustomerModel(model);
            var customer = _mapper.Map<Customer>(model);
            await _customerRepository.AddAsync(customer);
            await _unitOfWork.SaveAsync();
        }

        public async Task DeleteAsync(int modelId)
        {
            await _customerRepository.DeleteByIdAsync(modelId);
            await _unitOfWork.SaveAsync();
        }

        public async Task<IEnumerable<CustomerModel>> GetAllAsync()
        {
            var customerList = await _customerRepository.GetAllWithDetailsAsync();
            return customerList.Select(x => _mapper.Map<CustomerModel>(x));
        }

        public async Task<CustomerModel> GetByIdAsync(int id)
        {
            var customer = await _customerRepository.GetByIdWithDetailsAsync(id);
            var customerModel = _mapper.Map<CustomerModel>(customer);
            return customerModel;
        }

        public async Task<IEnumerable<CustomerModel>> GetCustomersByProductIdAsync(int productId)
        {
            var customerList = await _customerRepository.GetAllWithDetailsAsync();
            var costumersByProductId = customerList
                .Where(x => x.Receipts.FirstOrDefault(z => z.ReceiptDetails.FirstOrDefault(c => c.ProductId == productId) != null) != null);

            return costumersByProductId.Select(x => _mapper.Map<CustomerModel>(x));
        }

        public async Task UpdateAsync(CustomerModel model)
        {
            ValidateCustomerModel(model);
            var user = _mapper.Map<Customer>(model);
            user.Person.Id = user.Id;
            _customerRepository.Update(user);
            await _unitOfWork.SaveAsync();
        }


        private static void ValidateCustomerModel(CustomerModel customerModel)
        {
            if (customerModel == null)
            {
                throw new MarketException("Invalid Customer Model!");
            }
            else if (string.IsNullOrEmpty(customerModel.Name) || string.IsNullOrEmpty(customerModel.Surname))
            {
                throw new MarketException("Invalid Name/Surname For CustomerModel");
            }
            else if (customerModel.DiscountValue < 0)
            {
                throw new MarketException("Discount Value can't be non positive!");
            }
            var minDate = new DateTime(1894, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            var maxDate = DateTime.Now;
            if (customerModel.BirthDate <= minDate || customerModel.BirthDate >= maxDate)
            {
                throw new MarketException("Invalid Date! For CustomerModel");
            }
        }
    }
}
