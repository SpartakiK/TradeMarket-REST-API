using AutoMapper;
using Business.Interfaces;
using Business.Models;
using Business.Validation;
using Data.Entities;
using Data.Interfaces;
using Data.Repositories;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Services
{
    public class ProductService : IProductService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IProductRepository _productRepository;
        private readonly IProductCategoryRepository _categoryRepository;
        private readonly IMapper _mapper;
        public ProductService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _productRepository = _unitOfWork.ProductRepository;
            _categoryRepository = _unitOfWork.ProductCategoryRepository;
        }

        public async Task AddAsync(ProductModel model)
        {
            ValidateProductModel(model);
            var product = _mapper.Map<Product>(model);

            product.Category = null;
            product.ProductCategoryId = model.ProductCategoryId;

            await _productRepository.AddAsync(product);
            await _unitOfWork.SaveAsync();
        }

        public async Task AddCategoryAsync(ProductCategoryModel categoryModel)
        {
            ValidateProductCategoryModel(categoryModel);
            var category = _mapper.Map<ProductCategory>(categoryModel);
            await _categoryRepository.AddAsync(category);
            await _unitOfWork.SaveAsync();
        }

        public async Task DeleteAsync(int modelId)
        {
            await _productRepository.DeleteByIdAsync(modelId);
            await _unitOfWork.SaveAsync();
        }

        public async Task<IEnumerable<ProductModel>> GetAllAsync()
        {
            var products = await _productRepository.GetAllWithDetailsAsync();
            return products.Select(x => _mapper.Map<ProductModel>(x));
        }

        public async Task<IEnumerable<ProductCategoryModel>> GetAllProductCategoriesAsync()
        {
            var categories = await _categoryRepository.GetAllAsync();
            return categories.Select(x => _mapper.Map<ProductCategoryModel>(x));
        }

        public async Task<IEnumerable<ProductModel>> GetByFilterAsync(FilterSearchModel filterSearch)
        {
            var products = await _productRepository.GetAllWithDetailsAsync();
            var filteredProducts = products;
            if (filterSearch != null && filterSearch.CategoryId != null)
            {
                filteredProducts = filteredProducts.Where(x => x.ProductCategoryId == filterSearch.CategoryId);
            }
            if (filterSearch != null && filterSearch.MaxPrice != null)
            {
                filteredProducts = filteredProducts.Where(x => x.Price < filterSearch.MaxPrice);
            }
            if (filterSearch != null && filterSearch.MinPrice != null)
            {
                filteredProducts = filteredProducts.Where(x => x.Price > filterSearch.MinPrice);
            }

            return filteredProducts.Select(x => _mapper.Map<ProductModel>(x));
        }

        public async Task<ProductModel> GetByIdAsync(int id)
        {
            var product = await _productRepository.GetByIdWithDetailsAsync(id);
            return _mapper.Map<ProductModel>(product);
        }

        public async Task RemoveCategoryAsync(int categoryId)
        {
            await _categoryRepository.DeleteByIdAsync(categoryId);
            await _unitOfWork.SaveAsync();
        }

        public async Task UpdateAsync(ProductModel model)
        {
            ValidateProductModel(model);
            await Task.Run(() => _productRepository.Update(_mapper.Map<Product>(model)));
            await _unitOfWork.SaveAsync();
        }

        public async Task UpdateCategoryAsync(ProductCategoryModel categoryModel)
        {
            ValidateProductCategoryModel(categoryModel);
            await Task.Run(() => _categoryRepository.Update(_mapper.Map<ProductCategory>(categoryModel)));
            await _unitOfWork.SaveAsync();
        }


        private static void ValidateProductModel(ProductModel productModel)
        {
            if (productModel == null)
            {
                throw new MarketException("Invalid ProductModel!");
            }
            else if (string.IsNullOrEmpty(productModel.ProductName))
            {
                throw new MarketException("Invalid Name For ProductModel!");
            }
            else if (productModel.Price < 0)
            {
                throw new MarketException("Invalid Price Range For ProductModel!");
            }
        }
        private static void ValidateProductCategoryModel(ProductCategoryModel productCategoryModel)
        {
            if (productCategoryModel == null)
            {
                throw new MarketException("Invalid ProductCategoryModel!");
            }
            else if (string.IsNullOrEmpty(productCategoryModel.CategoryName))
            {
                throw new MarketException("Invalid Name For ProductModel!");
            }
        }
    }
}
