using AutoMapper;
using Bogus;
using Ioc.Core.DbModel;
using Ioc.Core.DbModel.Models;
using Ioc.Core.DbModel.Models.Quiz;
using Ioc.Core.DbModel.Models.SiteInfo;
using Ioc.Core.DbModel.SqlLoadModel;
using Ioc.Core.DbModel.Validation;
using Ioc.Data.Data;
using Ioc.ObjModels.Model;
using Ioc.ObjModels.Model.CommonModel;
using Ioc.ObjModels.Model.SiteInfo;
using Ioc.Service.Interfaces;
using Ioc.Service.Interfaces.Common;
using Ioc.Service.Interfaces.FakeDataHelp;
using Microsoft.AspNetCore.Mvc;
using WebAPI.FakerData;

namespace WebAPI.Controllers
{
    public class CommonController : ControllerBase
    {
        private readonly IAddressService _repository;
        private readonly IContactService _repositoryContact;
        private readonly ICompanyService _repositoryCompany;
        private readonly IBankDetailsService _repositoryBankDetails;
        private readonly IRandomGenHelpService _repositoryRandomGenHelp;
        private readonly IMapper _mapper;
        private readonly ILogger<InvoiceModel> _logger;
        private readonly IocDbContext dbContext;
        private readonly ICategoryRepository _categoryService;
        private readonly IProductService _productService;
        private readonly ICustomerService _customerService;
        private readonly ISalesPersonService _SalesPersonService;
        private readonly IInvoiceService _invoiceService;
        private readonly ISubCategoryService _subCategoryService;
        

        public CommonController(
            IAddressService repository,
            IContactService repositoryContact,
            ICompanyService repositoryCompany,
            IBankDetailsService repositoryBankDetails,
            IRandomGenHelpService repositoryRandomGenHelp,
            IMapper mapper,
            ILogger<InvoiceModel> logger,
            IocDbContext dbContext,
            ICategoryRepository categoryService,
            IProductService productService,
            ICustomerService customerService,
            ISalesPersonService salesPersonService,
            IInvoiceService invoiceService,
            ISubCategoryService subCategoryService)
        {
            _repository = repository;
            _repositoryContact = repositoryContact;
            _repositoryCompany = repositoryCompany;
            _repositoryBankDetails = repositoryBankDetails;
            _repositoryRandomGenHelp = repositoryRandomGenHelp;
            _mapper = mapper;
            _logger = logger;
            this.dbContext = dbContext;
            _categoryService = categoryService;
            _productService = productService;
            _customerService = customerService;
            _SalesPersonService = salesPersonService;
            _invoiceService = invoiceService;
            _subCategoryService = subCategoryService;
        }
        /*[HttpGet]
        public async Task<IActionResult> Get()
        {
            try
            {
                var coolestCategory = _repository.GetAll();
                var data = _mapper.Map<IEnumerable<CategoryModel>>(coolestCategory);
                return Ok(data);
            }
            catch (Exception ex)
            {
                throw;
            }
        }*/

        [HttpGet("SaveFakeDataAddress/{howMuch}")]
        public async Task<IActionResult> SaveFakeData(int howMuch)
        {
            var dataToCreate = _mapper.Map<List<Address>>(FakerDataClass.GetFakeDataAddress(howMuch));
            await _repository.CreateInRange(dataToCreate);
            return Ok($"{howMuch} TIMES DATA IS CREATED");
        }



        //ContactDetailsModel

        [HttpGet("SaveFakeContactDetailsModel/{howMuch}")]
        public async Task<IActionResult> SaveFakeDatarepositoryContact(int howMuch)
        {
            var dataToCreate = _mapper.Map<List<Contact>>(GetFakeContactDetailsModel(howMuch));
            await _repositoryContact.CreateInRange(dataToCreate);
            return Ok($"{howMuch} TIMES DATA IS CREATED");
        }


        private List<ContactDetailsModel> GetFakeContactDetailsModel(int recordsToInsert)
        {
            var bogus = new Faker<ContactDetailsModel>()
               .RuleFor(a => a.PhoneNumber, f => f.Person.Phone)
               .RuleFor(a => a.Email, f => f.Person.Email)
                .RuleFor(a => a.Fax, f => f.Person.Phone);

            var data = bogus.Generate(recordsToInsert);

            return data;
        }


        [HttpGet("SaveFakeDataCompany/{howMuch}")]
        public async Task<IActionResult> SaveFakeDataCompany(int howMuch)
        {
            var dataToCreate = _mapper.Map<List<Company>>(GetFakeDataCompany(howMuch));
            await _repositoryCompany.CreateInRange(dataToCreate);
            return Ok($"{howMuch} TIMES DATA IS CREATED");
        }


        private List<CompanyModel> GetFakeDataCompany(int recordsToInsert)
        {
            var bogus = new Faker<CompanyModel>()
               .RuleFor(a => a.CompanyName, f => f.Company.CompanyName())
               .RuleFor(a => a.CompanyAddress, FakerDataClass.GetFakeDataAddress(1).FirstOrDefault())
                .RuleFor(a => a.ContactDetails, GetFakeContactDetailsModel(1).FirstOrDefault())
                .RuleFor(a => a.BankDetails, FakerDataClass.GetFakeDataBankDetails(1).FirstOrDefault());

            var data = bogus.Generate(recordsToInsert);

            return data;
        }

        [HttpGet("SaveFakeBankDetails/{howMuch}")]
        public async Task<IActionResult> SaveFakeBankDetail(int howMuch)
        {
            var dataToCreate = _mapper.Map<List<BankDetails>>(FakerDataClass.GetFakeDataBankDetails(howMuch));
            await _repositoryBankDetails.CreateInRange(dataToCreate);
            return Ok($"{howMuch} TIMES DATA IS CREATED");
        }

        //GetFakeRandomGenHelp

        [HttpGet("SaveRandomGenHelp/{howMuch}/{type}")]
        public async Task<IActionResult> SaveRandomGenHelp(RandomType type, int howMuch)
        {
            var dataToCreate = FakerDataClass.GetFakeRandomGenHelp(type, howMuch);
            await _repositoryRandomGenHelp.CreateInRange(dataToCreate);
            return Ok($"{howMuch} TIMES DATA IS CREATED");
        }

        [HttpPost("AddFakeDataToDb")]
        public async Task<IActionResult> AddFakeDataToDb(int count = 10)
        {
            var saved = 0;
            try
            {
                // Example: Add fake categories
                var fakeCategories = new Faker<Category>()
                .RuleFor(c => c.Name, f => f.Commerce.Categories(1)[0])
                .RuleFor(c => c.Description, f => f.Lorem.Sentence())
                .RuleFor(c => c.SiteCode, f => "TEST")
                .Generate(count);

                dbContext.Category.AddRange(fakeCategories);

                // Example: Add fake products
                var fakeProducts = new Faker<Product>()
                    .RuleFor(p => p.Name, f => f.Commerce.ProductName())
                    .RuleFor(p => p.Description, f => f.Commerce.ProductDescription())
                    .RuleFor(p => p.Price, f => f.Random.Decimal(10, 1000))
                    .RuleFor(p => p.Status, f => f.Random.Bool())
                    .RuleFor(p => p.Quantity, f => f.Random.Int(1, 100))
                    .RuleFor(p => p.SiteCode, f => "TEST")
                    .Generate(count);

                dbContext.Product.AddRange(fakeProducts);

                // Example: Add fake customers
                var fakeCustomers = new Faker<Customer>()
                    .RuleFor(c => c.FirstName, f => f.Name.FirstName())
                    .RuleFor(c => c.LastName, f => f.Name.LastName())
                    .RuleFor(c => c.Email, f => f.Internet.Email())
                    .RuleFor(c => c.CompanyName, f => f.Company.CompanyName())
                    .RuleFor(c => c.SiteCode, f => "TEST")
                    .Generate(count);

                dbContext.Customers.AddRange(fakeCustomers);

                // Add more fake data as needed...


                saved = await dbContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {

                throw ex;
            }
            return Ok($"Fake data added. Total records saved: {saved}");
        }

        [HttpPost("AddFakeDataToDbAll")]
        public async Task<IActionResult> AddFakeDataToDbAll(int count = 5)
        {
            // Category
            var fakeCategories = new Faker<Category>()
                .RuleFor(c => c.Name, f => f.Commerce.Categories(1)[0])
                .RuleFor(c => c.Description, f => f.Lorem.Sentence())
                .Generate(count);
            dbContext.Category.AddRange(fakeCategories);

            // SubCategory
            var fakeSubCategories = new Faker<SubCategory>()
                .RuleFor(s => s.Name, f => f.Commerce.ProductAdjective())
                .RuleFor(s => s.Description, f => f.Lorem.Sentence())
                .Generate(count);
            dbContext.SubCategory.AddRange(fakeSubCategories);

            // User
            var fakeUsers = new Faker<User>()
                .RuleFor(u => u.UserName, f => f.Internet.UserName())
                .RuleFor(u => u.Email, f => f.Internet.Email())
                .Generate(count);
            dbContext.User.AddRange(fakeUsers);

            // UserProfile
            var fakeUserProfiles = new Faker<UserProfile>()
                .RuleFor(u => u.FirstName, f => f.Name.FirstName())
                .RuleFor(u => u.LastName, f => f.Name.LastName())
                .Generate(count);
            dbContext.UserProfile.AddRange(fakeUserProfiles);

            // CommonGroup
            var fakeCommonGroups = new Faker<CommonGroup>()
                .RuleFor(c => c.GroupDetails, f => f.Lorem.Word())
                .RuleFor(c => c.name, f => f.Commerce.Department())
                .RuleFor(c => c.description, f => f.Lorem.Sentence())
                .Generate(count);
            dbContext.CommonGroup.AddRange(fakeCommonGroups);

            // Customers
            var fakeCustomers = new Faker<Customer>()
                .RuleFor(c => c.FirstName, f => f.Name.FirstName())
                .RuleFor(c => c.LastName, f => f.Name.LastName())
                .RuleFor(c => c.Email, f => f.Internet.Email())
                .RuleFor(c => c.CompanyName, f => f.Company.CompanyName())
                .Generate(count);
            dbContext.Customers.AddRange(fakeCustomers);

            // Setting
            var fakeSettings = new Faker<Setting>()
                .RuleFor(s => s.SettingType, f => f.Lorem.Word())
                .RuleFor(s => s.DisplayName, f => f.Lorem.Word())
                .RuleFor(s => s.Description, f => f.Lorem.Sentence())
                .Generate(count);
            dbContext.Setting.AddRange(fakeSettings);

            // QuizSetup
            var fakeQuizSetups = new Faker<QuizSetup>()
                .RuleFor(q => q.Title, f => f.Lorem.Sentence(3))
                .RuleFor(q => q.Description, f => f.Lorem.Sentence())
                .Generate(count);
            dbContext.QuizSetups.AddRange(fakeQuizSetups);

            // QuestionSetup
            var fakeQuestionSetups = new Faker<QuestionSetup>()
                .RuleFor(q => q.Text, f => f.Lorem.Sentence())
                .RuleFor(q => q.Description, f => f.Lorem.Sentence())
                .RuleFor(q => q.CorrectAnswerId, f => Guid.NewGuid())
                .Generate(count);
            dbContext.QuestionSetup.AddRange(fakeQuestionSetups);

            // AnswerSetup
            var fakeAnswerSetups = new Faker<AnswerSetup>()
                .RuleFor(a => a.Text, f => f.Lorem.Word())
                .RuleFor(a => a.Description, f => f.Lorem.Sentence())
                .RuleFor(a => a.QuestionSetupID, f => Guid.NewGuid())
                .RuleFor(a => a.IsCorrect, f => f.Random.Bool())
                .Generate(count);
            dbContext.AnswerSetup.AddRange(fakeAnswerSetups);

            // QuizDescription
            var fakeQuizDescriptions = new Faker<QuizDescription>()
                .RuleFor(q => q.QuizSetupID, f => Guid.NewGuid())
                .RuleFor(q => q.QuestionSetupID, f => Guid.NewGuid())
                .Generate(count);
            dbContext.QuizDescription.AddRange(fakeQuizDescriptions);

            // CompleteUserDetails
            var fakeCompleteUserDetails = new Faker<CompleteUserDet>()
                .RuleFor(c => c.FirstName, f => f.Name.FirstName())
                .RuleFor(c => c.LastName, f => f.Name.LastName())
                .RuleFor(c => c.Email, f => f.Internet.Email())
                .RuleFor(c => c.UserName, f => f.Internet.UserName())
                .RuleFor(c => c.IsActive, f => f.Random.Bool())
                .Generate(count);
            dbContext.CompleteUserDetails.AddRange(fakeCompleteUserDetails);

            // Address
            var fakeAddresses = new Faker<Address>()
                .RuleFor(a => a.Street, f => f.Address.StreetAddress())
                .RuleFor(a => a.City, f => f.Address.City())
                .RuleFor(a => a.State, f => f.Address.State())
                .RuleFor(a => a.ZipCode, f => f.Address.ZipCode())
                .RuleFor(a => a.Country, f => f.Address.Country())
                .Generate(count);
            dbContext.Address.AddRange(fakeAddresses);

            // Product
            var fakeProducts = new Faker<Product>()
                .RuleFor(p => p.Name, f => f.Commerce.ProductName())
                .RuleFor(p => p.Description, f => f.Commerce.ProductDescription())
                .RuleFor(p => p.Price, f => f.Random.Decimal(10, 1000))
                .RuleFor(p => p.Status, f => f.Random.Bool())
                .RuleFor(p => p.Quantity, f => f.Random.Int(1, 100))
                .Generate(count);
            dbContext.Product.AddRange(fakeProducts);

            // SalesPerson
            var fakeSalesPersons = new Faker<SalesPerson>()
                .RuleFor(s => s.FirstName, f => f.Name.FirstName())
                .RuleFor(s => s.LastName, f => f.Name.LastName())
                .RuleFor(s => s.EmployeeId, f => f.Random.Int(1000, 9999))
                .RuleFor(s => s.Email, f => f.Internet.Email())
                .RuleFor(s => s.PhoneNumber, f => f.Phone.PhoneNumber())
                .Generate(count);
            dbContext.SalesPerson.AddRange(fakeSalesPersons);

            // Invoice
            var fakeInvoices = new Faker<Invoice>()
                .RuleFor(i => i.InvoiceNumber, f => f.Random.Int(10000, 99999))
                .RuleFor(i => i.InvoiceDate, f => f.Date.Past())
                .RuleFor(i => i.TotalAmount, f => f.Random.Decimal(100, 10000))
                .Generate(count);
            dbContext.Invoice.AddRange(fakeInvoices);

            // Company
            var fakeCompanies = new Faker<Company>()
                .RuleFor(c => c.CompanyName, f => f.Company.CompanyName())
                .RuleFor(c => c.CompanyAddress, f => new Address { Street = f.Address.StreetAddress(), City = f.Address.City(), State = f.Address.State(), ZipCode = f.Address.ZipCode(), Country = f.Address.Country() })
                .Generate(count);
            dbContext.Company.AddRange(fakeCompanies);

            // Contact
            var fakeContacts = new Faker<Contact>()
                .RuleFor(c => c.PhoneNumber, f => f.Phone.PhoneNumber())
                .RuleFor(c => c.Email, f => f.Internet.Email())
                .RuleFor(c => c.Fax, f => f.Phone.PhoneNumber())
                .Generate(count);
            dbContext.Contact.AddRange(fakeContacts);

            // BankDetails
            var fakeBankDetails = new Faker<BankDetails>()
                .RuleFor(b => b.BankName, f => f.Company.CompanyName())
                .RuleFor(b => b.AccountHolderName, f => f.Name.FullName())
                .RuleFor(b => b.AccountNumber, f => f.Finance.Account())
                .RuleFor(b => b.RoutingNumber, f => f.Random.Replace("#########"))
                .RuleFor(b => b.AccountType, f => f.PickRandom<AccountType>())
                .RuleFor(b => b.BranchName, f => f.Company.CompanyName())
                .RuleFor(b => b.BranchAddress, f => f.Address.FullAddress())
                .RuleFor(b => b.SwiftCode, f => f.Random.AlphaNumeric(8))
                .Generate(count);
            dbContext.BankDetails.AddRange(fakeBankDetails);

            // InvoiceNew
            var fakeInvoiceNew = new Faker<InvoiceNew>()
                .RuleFor(i => i.InvoiceNumber, f => f.Random.Int(10000, 99999))
                .RuleFor(i => i.InvoiceDate, f => f.Date.Past())
                .RuleFor(i => i.TotalAmount, f => f.Random.Decimal(100, 10000))
                .Generate(count);
            dbContext.InvoiceNew.AddRange(fakeInvoiceNew);

            // RandomGenHelp
            var fakeRandomGenHelp = new Faker<RandomGenHelp>()
                .RuleFor(r => r.Type, f => f.Random.Int(1, 10))
                .RuleFor(r => r.RandomValue, f => f.Random.Word())
                .Generate(count);
            dbContext.RandomGenHelp.AddRange(fakeRandomGenHelp);

            // ValidationRule
            var fakeValidationRules = new Faker<ValidationRule>()
                .RuleFor(v => v.ModelName, f => f.Lorem.Word())
                .RuleFor(v => v.PropertyName, f => f.Lorem.Word())
                .RuleFor(v => v.RuleType, f => f.PickRandom(new[] { "Error", "Warning" }))
                .RuleFor(v => v.Rule, f => f.Lorem.Sentence())
                .Generate(count);
            dbContext.ValidationRule.AddRange(fakeValidationRules);

            // ImportProducts
            var fakeImportProducts = new Faker<ImportProduct>()
                .RuleFor(i => i.category, f => f.Commerce.Categories(1)[0])
                .RuleFor(i => i.Out_of_Stock, f => f.Random.Bool() ? "Yes" : "No")
                .RuleFor(i => i.sku, f => f.Commerce.Ean13())
                .RuleFor(i => i.price, f => f.Random.Decimal(10, 1000))
                .RuleFor(i => i.qty, f => f.Random.Int(1, 100))
                .Generate(count);
            dbContext.ImportProducts.AddRange(fakeImportProducts);

            // LogEntries (disambiguate)
            var fakeLogEntries = new Faker<Ioc.Core.DbModel.LogEntry>()
                .RuleFor(l => l.Timestamp, f => f.Date.Recent())
                .RuleFor(l => l.Level, f => f.PickRandom(new[] { "Info", "Warn", "Error" }))
                .RuleFor(l => l.Message, f => f.Lorem.Sentence())
                .RuleFor(l => l.User, f => f.Internet.UserName())
                .RuleFor(l => l.Source, f => f.System.FileName())
                .Generate(count);
            dbContext.LogEntries.AddRange(fakeLogEntries);

            // SiteSetup
            var fakeSiteSetups = new Faker<SiteSetup>()
                .RuleFor(s => s.SiteName, f => f.Company.CompanyName())
                .RuleFor(s => s.SiteCode, f => f.Random.AlphaNumeric(6))
                .Generate(count);
            dbContext.SiteSetup.AddRange(fakeSiteSetups);

            var saved = await dbContext.SaveChangesAsync();
            return Ok($"Fake data added for all tables. Total records saved: {saved}");
        }

        [HttpPost("SaveFakeCategory/{count}")]
        public async Task<IActionResult> SaveFakeCategory(int count = 5)
        {
            var fakeCategories = new Faker<Category>()
                .RuleFor(c => c.Name, f => f.Commerce.Categories(1)[0])
                .RuleFor(c => c.Description, f => f.Lorem.Sentence())
                .RuleFor(c => c.SiteCode, f => "TEST")
                .Generate(count);
            foreach (var cat in fakeCategories)
            {
               if( !await _categoryService.CheckExistByName(cat?.Name))
                await _categoryService.Create(cat);
            }
            return Ok($"{count} fake categories created.");
        }

        [HttpPost("SaveFakeProduct/{count}")]
        public async Task<IActionResult> SaveFakeProduct(int count = 5)
        {
            // Generate products (no category FK in Product model)
            var fakeProducts = new Faker<Product>()
                .RuleFor(p => p.Name, f => f.Commerce.ProductName())
                .RuleFor(p => p.Description, f => f.Commerce.ProductDescription())
                .RuleFor(p => p.Price, f => f.Random.Decimal(10, 1000))
                .RuleFor(p => p.Status, f => f.Random.Bool())
                .RuleFor(p => p.Quantity, f => f.Random.Int(1, 100))
                .RuleFor(p => p.SiteCode, f => "TEST")
                .Generate(count);
            foreach (var prod in fakeProducts)
            {
                await _productService.Create(prod);
            }
            return Ok($"{count} fake products created.");
        }

        [HttpPost("SaveFakeCustomer/{count}")]
        public async Task<IActionResult> SaveFakeCustomer(int count = 5)
        {
            var fakeCustomers = new Faker<Customer>()
                .RuleFor(c => c.FirstName, f => f.Name.FirstName())
                .RuleFor(c => c.LastName, f => f.Name.LastName())
                .RuleFor(c => c.Email, f => f.Internet.Email())
                .RuleFor(c => c.CompanyName, f => f.Company.CompanyName())
                .RuleFor(c => c.SiteCode, f => "TEST")
                .Generate(count);
            foreach (var cust in fakeCustomers)
            {
                await _customerService.Create(cust);
            }
            return Ok($"{count} fake customers created.");
        }

        [HttpPost("SaveFakeSubCategory/{count}")]
        public async Task<IActionResult> SaveFakeSubCategory(int count = 5)
        {
            // Get a random existing category
            var categories = _categoryService.GetAll().ToList();
            if (!categories.Any())
                return BadRequest("No categories exist. Please create categories first.");
            var random = new Random();
            var fakeSubCategories = new Faker<SubCategory>()
                .RuleFor(s => s.Name, f => f.Commerce.ProductAdjective())
                .RuleFor(s => s.Description, f => f.Lorem.Sentence())
                .RuleFor(s => s.CategoryId, f => categories[random.Next(categories.Count)].ID)
                .Generate(count);
                await _subCategoryService.CreateInRange(fakeSubCategories); // Replace with correct subcategory service
            return Ok($"{count} fake subcategories created.");
        }

        [HttpPost("SaveFakeInvoiceNew/{count}")]
        public async Task<IActionResult> SaveFakeInvoiceNew(int count = 5)
        {
            // Get random existing company, salesperson, customer
            var companies = _repositoryCompany.GetAll().ToList();
            if (!companies.Any())
            {
                var dataToCreate = _mapper.Map<List<Company>>(GetFakeDataCompany(5));
                await _repositoryCompany.CreateInRange(dataToCreate);
                companies = _repositoryCompany.GetAll().ToList();

            }
            var salesPeople = _SalesPersonService.GetAll().ToList(); // Replace with correct sales person service
            if (!salesPeople.Any())
            {
                var dataToCreate = _mapper.Map<List<SalesPerson>>(GetFakerSalesPerson(5));
                await _SalesPersonService.CreateInRange(dataToCreate);
                salesPeople = _SalesPersonService.GetAll().ToList();

            }
            var customers = _customerService.GetAll().ToList();
            if (!customers.Any())
            {
                var dataToCreate = _mapper.Map<List<Customer>>(GetFakerCustomers(5));
                await _customerService.CreateInRange(dataToCreate);
                customers = _customerService.GetAll().ToList();

            }
            var random = new Random();
            var fakeInvoices = new Faker<InvoiceNew>()
                .RuleFor(i => i.InvoiceNumber, f => f.Random.Int(10000, 99999))
                .RuleFor(i => i.InvoiceDate, f => f.Date.Past())
                .RuleFor(i => i.TotalAmount, f => f.Random.Decimal(100, 10000))
                .RuleFor(i => i.CompanyId, f => companies[random.Next(companies.Count)].ID)
                .RuleFor(i => i.SalespersonId, f => salesPeople[random.Next(salesPeople.Count)].ID)
                .RuleFor(i => i.CustomerId, f => customers[random.Next(customers.Count)].ID)
                .Generate(count);
                await _invoiceService.CreateInRange(fakeInvoices); // Replace with correct invoice service
            return Ok($"{count} fake invoices created.");
        }

        private List<SalesPersonModel> GetFakerSalesPerson(int recordsToInsert)
        {
            var fakeSalesPersons = new Faker<SalesPersonModel>()
               .RuleFor(s => s.FirstName, f => f.Name.FirstName())
               .RuleFor(s => s.LastName, f => f.Name.LastName())
               .RuleFor(s => s.EmployeeId, f => f.Random.Int(1000, 9999))
               .RuleFor(s => s.Email, f => f.Internet.Email())
               .RuleFor(s => s.PhoneNumber, f => f.Phone.PhoneNumber())
               .Generate(recordsToInsert);
            return fakeSalesPersons;
        }

        private List<CustomerModel> GetFakerCustomers(int recordsToInsert)
        {
            var fakeCustomers = new Faker<CustomerModel>()
                  .RuleFor(c => c.FirstName, f => f.Name.FirstName())
                  .RuleFor(c => c.LastName, f => f.Name.LastName())
                  .RuleFor(c => c.Email, f => f.Internet.Email())
                  .RuleFor(c => c.CompanyName, f => f.Company.CompanyName())
                  .RuleFor(c => c.SiteCode, f => "TEST")
                  .Generate(recordsToInsert);
            return fakeCustomers;
        }


    }
}
