using AutoMapper;
using Ioc.Core.DbModel;
using Ioc.ObjModels.Model.CommonModel;
using Ioc.Service.Interfaces.Common;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SettingController : ControllerBase
    {
        private readonly ISettingService _settingService;
        private readonly IMapper _mapper;

        public SettingController(ISettingService settingService, IMapper mapper)
        {
            _settingService = settingService;
            _mapper = mapper;
        }
        /*[HttpGet]
        public async Task<IActionResult> Index()
        {
            //var data = _mapper.Map<IEnumerable<CommonGroupModel>>(_commonCategory.GetAll());
            return Ok(new ResponseModel() { Status = 200, Message = "Created", Data = _settingService.GetAllAsync().Result });
        }

        [HttpPost]
        public async Task<IActionResult> Save(SettingModel? model)
        {

            var data = _mapper.Map<Setting>(model);
            if (!_settingService.CheckExist(model.SettingType))
            {
               // await _settingService.Create(data);
                return Ok(new ResponseModel() { Status = 200, Message = "Created" });
            }
            else
            {
                return BadRequest(new ResponseModel() { Status = 404, Message = "Allready Exist" });
            }



        }*/

    }
}
