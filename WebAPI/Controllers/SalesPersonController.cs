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
    public class SalesPersonController : ControllerBase
    {
        private readonly ISalesPersonService _repository;
        private readonly IMapper _mapper;
        private readonly ILogger<SalesPersonModel> _logger;

        public SalesPersonController(ISalesPersonService repository, IMapper mapper, ILogger<SalesPersonModel> logger)
        {
            _repository = repository;
            _mapper = mapper;
            _logger = logger;
        }
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            try
            {
                var coolestCategory = _repository.GetAll();
                var data = _mapper.Map<IEnumerable<SalesPersonModel>>(coolestCategory);
                return Ok(data);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        [HttpGet("SaveFakeData/{howMuch}")]
        public async Task<IActionResult> SaveFakeData(int howMuch)
        {
            var dataToCreate = _mapper.Map<List<SalesPerson>>(GetFakeData(howMuch));
            await _repository.CreateInRange(dataToCreate);
            return Ok($"{howMuch} TIMES DATA IS CREATED");
        }


        private List<SalesPersonModel> GetFakeData(int recordsToInsert)
        {
            var bogus = new Faker<SalesPersonModel>()
                // .RuleFor(p => p.ID, f => Guid.NewGuid())
                .RuleFor(p => p.FirstName, f => f.Person.FirstName)
                .RuleFor(p => p.LastName, f => f.Person.LastName)
                .RuleFor(p => p.EmployeeId, f => f.Person.Random.Number(1000, 2000))
                .RuleFor(p => p.Email, f => f.Person.Email)
                .RuleFor(p => p.PhoneNumber, f => f.Person.Phone);

            var SalesPersons = bogus.Generate(recordsToInsert);

            return SalesPersons;
        }
    }
}
