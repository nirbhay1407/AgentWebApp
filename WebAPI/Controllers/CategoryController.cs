using AutoMapper;
using Ioc.Core.DbModel.Models;
using Ioc.ObjModels.Model;
using Ioc.Service.Interfaces;
using Ioc.Service.Interfaces.Common;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebAPI.Models;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Admin,SuperAdmin")]
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryRepository _repository;
        private readonly ICommonCategory _commonCategory;
        private readonly IMapper _mapper;
        private readonly ILogger<CategoryModel> _logger;

        public CategoryController(ICategoryRepository repository, ICommonCategory commonCategory, IMapper mapper, ILogger<CategoryModel> logger)
        {
            _repository = repository;
            _commonCategory = commonCategory;
            _mapper = mapper;
            _logger = logger;
        }
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            try
            {


                var coolestCategory = _repository.GetAll();
                var data = _mapper.Map<IEnumerable<CategoryModel>>(coolestCategory);
                _logger.LogInformation(data.ToMarkdownTable());
                _logger.LogInformation($"Returning students.");
                return Ok(data);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        [HttpGet("GetCategory/{GroupDetails}")]
        public async Task<IActionResult> GetCategory(string GroupDetails)
        {
            var data = _mapper.Map<IEnumerable<CommonGroupModel>>(_commonCategory.GetCommonGroupByGroup(GroupDetails));
            return Ok(data);
        }

        [HttpPost]
        public async Task<IActionResult> Save(CategoryModel model)
        {

            var data = _mapper.Map<Category>(model);
            await _repository.Create(data);
            var returnD = _mapper.Map<CategoryModel>(data);
            return Ok(returnD);
        }
        [HttpDelete]
        //[Authorize(ClaimTypes = "CanDeleteData", ClaimValue = "true")]
        [Authorize(Policy = "CanDelete")]
        public async Task<IActionResult> delete(Guid id)
        {
            await _repository.Delete(id);
            return NoContent();
        }

        [HttpGet("get-by-sp/{id}")]
        public async Task<IActionResult> getBySp(Guid id)
        {
            var data = await _repository.GetDataBySp(id);
            return Ok(data);
        }
    }
}
