using DocumentFormat.OpenXml.Office2016.Drawing.ChartDrawing;
using Ioc.Data.Data;
using Ioc.Data.UnitOfWorkRepo;
using Ioc.Service.Interfaces;
using Ioc.Service.Interfaces.Common;
using Ioc.Service.Interfaces.Validation;
using Ioc.Service.Services;
using Ioc.Service.Services.Common;
using Ioc.Service.Services.Validation;

namespace AgentWebApp.Mapping
{
    public static class ExtForServiceRegistration
    {
        public static void MapServiceSingleton(this IServiceCollection builder)
        {
            builder.AddScoped<IUserService, UserService>();
            builder.AddScoped<ICategoryRepository, CategoryRepository>();
            builder.AddScoped<ICommonCategory, CommonCategory>();
            builder.AddScoped<ISubCategoryService, SubCategoryService>();
            builder.AddScoped<IQuizSetupService, QuizSetupService>();
            builder.AddScoped<IQuestionSetupService, QuestionSetupService>();
            builder.AddScoped<IAnswerSetupService, AnswerSetupService>();
            builder.AddScoped<IQuizDescService, QuizDescService>();
            builder.AddScoped<ICompleteUserDetailsService, CompleteUserDetailsService>();
            builder.AddScoped<IProductService, ProductService>();
            builder.AddScoped<ISalesPersonService, SalesPersonService>();
            builder.AddScoped<IInvoiceService, InvoiceService>();
            builder.AddScoped<IAddressService, AddressService>(); 
            builder.AddScoped<ICustomerService, CustomerService>(); 
            builder.AddScoped<IValidationRuleRepository, ValidationRuleRepository>(); 
            builder.AddScoped<IUnitOfWork, UnitOfWork>();
            builder.AddScoped<ISiteSetupService, SiteSetupService>();

            builder.AddScoped<IocDbContext>();

            //singleton for services that do not require state
            builder.AddSingleton<ISettingService, SettingService>();


        }
    }
}
