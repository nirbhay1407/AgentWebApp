using AutoMapper;
using Ioc.Core.DbModel;
using Ioc.ObjModels.Model;
using Ioc.Service.Interfaces.Common;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class CommonCategoryController : ControllerBase
    {
        private readonly ICommonCategory _commonCategory;
        private readonly IMapper _mapper;

        public CommonCategoryController(ICommonCategory commonCategory, IMapper mapper)
        {
            _commonCategory = commonCategory;
            _mapper = mapper;
        }
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var data = _mapper.Map<IEnumerable<CommonGroupModel>>(_commonCategory.GetAll());
            return Ok(data);
        }


        [HttpPost("Save")]
        public async Task<IActionResult> Save(CommonGroupModel model)
        {

            var data = _mapper.Map<CommonGroup>(model);
            await _commonCategory.Create(data);
            return Ok();
        }
    }
}
