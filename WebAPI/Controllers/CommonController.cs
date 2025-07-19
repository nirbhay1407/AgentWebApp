using AutoMapper;
using Bogus;
using Ioc.Core.DbModel;
using Ioc.Core.DbModel.Models;
using Ioc.Core.DbModel.Models.SiteInfo;
using Ioc.ObjModels.Model;
using Ioc.ObjModels.Model.CommonModel;
using Ioc.ObjModels.Model.SiteInfo;
using Ioc.Service.Interfaces;
using Ioc.Service.Interfaces.Common;
using Ioc.Service.Interfaces.FakeDataHelp;
using Ioc.Service.Services;
using Ioc.Service.Services.FakeDataHelp;
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

        public CommonController(IAddressService repository, IContactService repositoryContact,
            ICompanyService repositoryCompany, IBankDetailsService repositoryBankDetails, IRandomGenHelpService repositoryRandomGenHelp, IMapper mapper, ILogger<InvoiceModel> logger)
        {
            _repository = repository;
            _repositoryContact = repositoryContact;
            _repositoryCompany = repositoryCompany;
            _repositoryBankDetails = repositoryBankDetails;
            _repositoryRandomGenHelp = repositoryRandomGenHelp;
            _mapper = mapper;
            _logger = logger;
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
    }
}
