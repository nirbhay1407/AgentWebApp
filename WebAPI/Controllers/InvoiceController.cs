using AutoMapper;
using Bogus;
using Ioc.Core.DbModel.Models;
using Ioc.ObjModels.Model;
using Ioc.Service.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InvoiceController : ControllerBase
    {
        private readonly IInvoiceService _repository;
        private readonly ISalesPersonService _repositorysms;
        private readonly ICustomerService _repositoryC;
        private readonly ICompanyService _repositoryCompany;
        private readonly IMapper _mapper;
        private readonly ILogger<InvoiceModel> _logger;

        public InvoiceController(IInvoiceService repository, ISalesPersonService repositorysms, ICustomerService repositoryC,
            ICompanyService repositoryCompany, IMapper mapper, ILogger<InvoiceModel> logger)
        {
            _repository = repository;
            _repositorysms = repositorysms;
            _repositoryC = repositoryC;
            _repositoryCompany = repositoryCompany;
            _mapper = mapper;
            _logger = logger;
        }
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            try
            {
                var coolestCategory = _repository.GetAllInc();
                var data = _mapper.Map<List<InvoiceModel>>(coolestCategory);
                return Ok(data);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        [HttpGet("SaveFakeDataInv/{howMuch}")]
        public async Task<IActionResult> SaveFakeData(int howMuch)
        {
            var dataToCreate = _mapper.Map<List<InvoiceNew>>(GetFakeData(howMuch));
            foreach (var item in dataToCreate)
            {
                item.CustomerId = (await _repositoryC.GetRnadomAny()).ID;
                item.SalespersonId = (await _repositorysms.GetRnadomAny()).ID;
                item.CompanyId = (await _repositoryCompany.GetRnadomAny()).ID;
            }
            await _repository.CreateInRange(dataToCreate);
            return Ok($"{howMuch} TIMES DATA IS CREATED");
        }





        private List<InvoiceModel> GetFakeData(int recordsToInsert)
        {

            /* var salePerson = new Faker<SalesPersonModel>()
                 .RuleFor(p => p.FirstName, f => f.Person.FirstName)
                 .RuleFor(p => p.LastName, f => f.Person.LastName)
                 .RuleFor(p => p.EmployeeId, f => f.Person.Random.Number(1000, 2000))
                 .RuleFor(p => p.Email, f => f.Person.Email)
                 .RuleFor(p => p.PhoneNumber, f => f.Person.Phone);

             var customerBg = new Faker<CustomerModel>()
                 .RuleFor(p => p.FirstName, f => f.Name.FirstName())
                 .RuleFor(p => p.LastName, f => f.Name.LastName())
                 .RuleFor(p => p.Contact, f => f.Phone.Random.Int(10).ToString())
                 .RuleFor(p => p.Email, f => f.Person.Email)
                 .RuleFor(p => p.DateOfBirth, f => f.Date.Past(20).ToShortDateString());*/

            var bogus = new Faker<InvoiceModel>()
                .RuleFor(p => p.InvoiceNumber, f => f.Random.Number(100000, 999999))
                .RuleFor(i => i.InvoiceDate, f => f.Date.Past(1))
                /*.RuleFor(i => i.Salesperson, f => salePerson.Generate())
                .RuleFor(i => i.Customer, f => customerBg.Generate())*/
                /* .RuleFor(i => i.SalespersonId, f => _mapper.Map<SalesPersonModel>(_repositorysms.GetRnadomAny()).ID)
                 .RuleFor(i => i.CustomerId, f => _mapper.Map<CustomerModel>(_repositoryC.GetRnadomAny()).ID)*/
                //.RuleFor(i => i.TotalAmount, (f, i) => i.Items.Sum(item => item.Price * item.Quantity));
                .RuleFor(i => i.TotalAmount, f => Convert.ToDecimal(f.Commerce.Price(99, 2500)));

            var Invoices = bogus.Generate(recordsToInsert);

            return Invoices;
        }

        public static int GenerateFakeInvoiceNumber()
        {
            var random = new Random();
            var prefix = new string(Enumerable.Range(0, 3).Select(_ => (char)('A' + random.Next(0, 26))).ToArray());
            var number = string.Join("", Enumerable.Range(0, 6).Select(_ => random.Next(0, 10).ToString()));

            //return $"{prefix}-{number}";
            return Convert.ToInt32(number);
        }
    }
}
