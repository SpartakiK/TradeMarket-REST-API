using AutoMapper;
using Business.Models;
using Data.Entities;
using System.Linq;

namespace Business
{
    public class AutomapperProfile : Profile
    {
        public AutomapperProfile()
        {
            CreateMap<Receipt, ReceiptModel>()
                .ForMember(rm => rm.ReceiptDetailsIds, r => r.MapFrom(x => x.ReceiptDetails.Select(rd => rd.Id)))
                .ReverseMap();

            CreateMap<Product, ProductModel>()
                .ForMember(pm => pm.ReceiptDetailIds, c => c.MapFrom(x => x.ReceiptDetails.Select(r => r.Id)))
                .ForMember(pm => pm.ProductCategoryId, c=> c.MapFrom(x => x.ProductCategoryId))
                .ForMember(pm => pm.CategoryName, c => c.MapFrom(x => x.Category.CategoryName))
                .ReverseMap();

            CreateMap<ReceiptDetail, ReceiptDetailModel>();

            CreateMap<Customer, CustomerModel>()
                .ForMember(cm => cm.ReceiptsIds, c => c.MapFrom(x => x.Receipts.Select(r => r.Id)))
                .ForMember(cm => cm.Name, c => c.MapFrom(x => x.Person.Name))
                .ForMember(cm => cm.Surname, c => c.MapFrom(x => x.Person.Surname))
                .ForMember(cm => cm.BirthDate, c => c.MapFrom(x => x.Person.BirthDate))
                .ReverseMap();

            CreateMap<ProductCategory, ProductCategoryModel>()
                .ForMember(pcm => pcm.ProductIds, c => c.MapFrom(x => x.Products.Select(r => r.Id)))
                .ReverseMap();
        }
    }
}