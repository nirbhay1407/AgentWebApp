using AutoMapper;
using Bogus;
using Ioc.Core.DbModel.Models;
using Ioc.ObjModels.Model;
using Ioc.Service.Interfaces;
using Microsoft.AspNetCore.Mvc;
using WebAPI.Models;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    //[Authorize]
    public class ProductController : ControllerBase
    {
        private readonly IProductService _repository;
        private readonly IMapper _mapper;
        private readonly ILogger<ProductModel> _logger;

        public ProductController(IProductService repository, IMapper mapper, ILogger<ProductModel> logger)
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
                var data = _mapper.Map<IEnumerable<ProductModel>>(coolestCategory);
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
            var dataToCreate = _mapper.Map<List<Product>>(GetFakeData(howMuch));
            await _repository.CreateInRange(dataToCreate);
            return Ok($"{howMuch} TIMES DATA IS CREATED");
        }


        private List<ProductModel> GetFakeData(int recordsToInsert)
        {
            var bogus = new Faker<ProductModel>()
                // .RuleFor(p => p.ID, f => Guid.NewGuid())
                .RuleFor(p => p.Name, f => f.Commerce.ProductName())
                .RuleFor(p => p.Description, f => f.Commerce.ProductDescription())
                .RuleFor(p => p.Price, f => Convert.ToDecimal(f.Commerce.Price(99, 2500)))
                .RuleFor(p => p.Status, f => f.Random.Bool())
                .RuleFor(p => p.Quantity, f => f.Commerce.Random.Number(1, 12));

            var products = bogus.Generate(recordsToInsert);

            return products;
        }
    }
}
