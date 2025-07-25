using AutoMapper;
using Bogus;
using Ioc.Core.DbModel.Models;
using Ioc.ObjModels.Model;
using Ioc.Service.Interfaces;
using Microsoft.AspNetCore.Mvc;
using WebAPI.FakerData;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomerController : ControllerBase
    {
        private readonly ICustomerService _repository;
        private readonly IMapper _mapper;

        public CustomerController(ICustomerService repository, IMapper mapper)
        {
            this._repository = repository;
            _mapper = mapper;
        }
        [HttpGet("GetWithoutCache")]
        public async Task<IEnumerable<Customer>> GetWithoutCache()
        {
            return _repository.GetAllWithInclude("Address");
        }

        [HttpGet]
        public async Task<IReadOnlyList<CustomerModel>> Get()
        {
            return _mapper.Map<IReadOnlyList<CustomerModel>>(await _repository.GetAllAsync());
        }
        [HttpGet("{id}")]
        public async Task<ActionResult<Customer>> Get(Guid id)
        {
            var customer = await _repository.GetById(id);
            if (customer == null)
            {
                return NotFound();
            }
            return customer;
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(Guid id, Customer customer)
        {
            if (id != customer.ID)
            {
                return BadRequest();
            }
            await _repository.Update(id, customer);
            return NoContent();
        }
        [HttpPost]
        public async Task<ActionResult<Customer>> Post(Customer customer)
        {
            await _repository.Create(customer);
            return CreatedAtAction("Get", new { id = customer.ID }, customer);
        }
        [HttpDelete("{id}")]
        public async Task<ActionResult<Customer>> Delete(Guid id)
        {
            var customer = await _repository.GetById(id);
            if (customer == null)
            {
                return NotFound();
            }
            await _repository.Delete(id);
            return customer;
        }


        [HttpGet("SaveFakeData/{howMuch}")]
        public async Task<IActionResult> SaveFakeData(int howMuch)
        {
            var dataToCreate = _mapper.Map<List<Customer>>(GetFakeData(howMuch));
            await _repository.CreateInRange(dataToCreate);
            return Ok($"{howMuch} TIMES DATA IS CREATED");
        }


        private List<CustomerModel> GetFakeData(int recordsToInsert)
        {
            var bogus = new Faker<CustomerModel>()
                // .RuleFor(p => p.ID, f => Guid.NewGuid())
                .RuleFor(p => p.FirstName, f => f.Name.FirstName())
                .RuleFor(p => p.LastName, f => f.Name.LastName())
                .RuleFor(p => p.Contact, f => f.Phone.Random.Int(10).ToString())
                .RuleFor(p => p.Email, f => f.Person.Email)
                .RuleFor(p => p.Mobile, f => f.Person.Phone)
                .RuleFor(p => p.Address, FakerDataClass.GetFakeDataAddress(1).FirstOrDefault())
                .RuleFor(p => p.CompanyName, f => f.Company.CompanyName())
                .RuleFor(p => p.DateOfBirth, f => f.Date.Past(20).ToShortDateString());

            var products = bogus.Generate(recordsToInsert);

            return products;
        }
    }
}
