using Ioc.Service.Interfaces;
using Ioc.Service.Interfaces.Common;
using Ioc.Service.Interfaces.FakeDataHelp;
using Ioc.Service.Services;
using Ioc.Service.Services.Common;
using Ioc.Service.Services.FakeDataHelp;

namespace WebAPI.Mapping
{
    public static class ExtForServiceRegistration
    {
        public static void MapServiceSingleton(this IServiceCollection builder)
        {
            builder.AddScoped<IUserService, UserService>();
            builder.AddScoped<ICategoryRepository, CategoryRepository>();
            builder.AddScoped<ICommonCategory, CommonCategory>();
            builder.AddScoped<ICustomerService, CustomerService>();
            builder.AddScoped<ISubCategoryService, SubCategoryService>();
            builder.AddScoped<ISettingService, SettingService>();
            builder.AddScoped<IQuizSetupService, QuizSetupService>();
            builder.AddScoped<IQuestionSetupService, QuestionSetupService>();
            builder.AddScoped<IAnswerSetupService, AnswerSetupService>();
            builder.AddScoped<IQuizDescService, QuizDescService>();
            builder.AddScoped<ICompleteUserDetailsService, CompleteUserDetailsService>();
            builder.AddScoped<IProductService, ProductService>();
            builder.AddScoped<ISalesPersonService, SalesPersonService>();
            builder.AddScoped<IInvoiceService, InvoiceService>();
            builder.AddScoped<IAddressService, AddressService>();
            builder.AddScoped<IContactService, ContactService>();
            builder.AddScoped<ICompanyService, CompanyService>();
            builder.AddScoped<IBankDetailsService, BankDetailsService>(); 
            builder.AddScoped<IRandomGenHelpService, RandomGenHelpService>(); 
        }
    }
}
