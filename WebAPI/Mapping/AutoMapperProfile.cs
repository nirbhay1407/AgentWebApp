using AutoMapper;
using Ioc.Core;
using Ioc.Core.DbModel;
using Ioc.Core.DbModel.Models;
using Ioc.Core.DbModel.Models.SiteInfo;
using Ioc.Core.DbModel.SqlLoadModel;
using Ioc.ObjModels;
using Ioc.ObjModels.Model;
using Ioc.ObjModels.Model.CommonModel;
using Ioc.ObjModels.Model.SiteInfo;
using Ioc.ObjModels.Model.SqlLoadCheckModel;
using WebAPI.Models;

namespace WebAPI.Mapping
{
    internal class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            /* CreateMap<CategoryDto, Category>().ForMember(dest => dest.CategoryName, opt => opt.MapFrom(src => src.Name)).ForMember(dest => dest.CategoryId, opt => opt.Ignore());
             CreateMap<Category, CategoryDto>().ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.CategoryName));*/


            CreateMap<DateTime, string>().ConvertUsing(dt => dt.ToString("u"));
            CreateMap<string, DateTime>();

            CreateMap<DateTime, string?>().ConvertUsing(dt => dt.ToString("u"));

            CreateMap<PublicBaseModel, PublicBaseEntity>();
            //CreateMap<UserModel, User>();

            CreateMap<UserModel, User>();
            CreateMap<UserModel, User>();

            CreateMap<CategoryModel, Category>();
            CreateMap<Category, CategoryModel>();

            CreateMap<CommonGroupModel, CommonGroup>();
            CreateMap<CommonGroup, CommonGroupModel>();

            CreateMap<CustomerModel, Customer>();
            CreateMap<Customer, CustomerModel>();

            CreateMap<SubCategoryModel, SubCategory>();
            CreateMap<SubCategory, SubCategoryModel>();

            CreateMap<SettingModel, Setting>();
            CreateMap<Setting, SettingModel>();

            CreateMap<CompleteUserDetails, CompleteUserDet>();
            CreateMap<CompleteUserDet, CompleteUserDetails>();

            CreateMap<ProductModel, Product>();
            CreateMap<Product, ProductModel>();

            CreateMap<InvoiceModel, Invoice>();
            CreateMap<Invoice, InvoiceModel>();

            CreateMap<SalesPersonModel, SalesPerson>();
            CreateMap<SalesPerson, SalesPersonModel>();

            CreateMap<AddressModel, Address>();
            CreateMap<Address, AddressModel>();

            CreateMap<InvoiceModel, InvoiceNew>().ReverseMap();
            CreateMap<ContactDetailsModel, Contact>().ReverseMap();
            CreateMap<CompanyModel, Company>().ReverseMap();

            CreateMap<BankDetailsModel, BankDetails>().ReverseMap();


        }

    }
}
