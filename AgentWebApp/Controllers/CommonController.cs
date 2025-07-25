using AutoMapper;
using DocumentFormat.OpenXml.Vml.Office;
using Ioc.Core.DbModel.Models;
using Ioc.Core.DbModel.Models.SiteInfo;
using Ioc.ObjModels.Model;
using Ioc.ObjModels.Model.SiteInfo;
using Ioc.Service.Interfaces;
using Ioc.Service.Interfaces.Common;
using Ioc.Service.Services.Common;
using Microsoft.AspNetCore.Mvc;

namespace AgentWebApp.Controllers
{
    public class CommonController : Controller
    {
        private readonly ISettingService _repository;
        private readonly ISiteSetupService _repositorySs;
        private readonly IMapper _mapper;
        private readonly ILogger<SiteSetupModel> _logger;


        public CommonController(ISettingService repository, IMapper mapper, ISiteSetupService repositorySs, ILogger<SiteSetupModel> logger)
        {
            _repository = repository;
            _mapper = mapper;
            _repositorySs = repositorySs;
            _logger = logger;
        }
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult UpdateDB()
        {
            //_repository.UpdateDB();
            return Ok();
        }

        [HttpGet("/SiteSetup")]
        public IActionResult SiteSetup()
        {
            var data = _repositorySs.GetAll();
            return View(_mapper.Map<List<SiteSetupModel>>(data));
        }
        [HttpGet("/CreateSiteSetup")]
        public IActionResult CreateSiteSetup()
        {
            return View();
        }
        [HttpPost("/CreateSiteSetup")]
        public IActionResult CreateSiteSetup(SiteSetupModel entity)
        {
            /*if (_repositorySs.CheckExist(entity.SiteCode){
                return NotFound();
            }*/
            if (entity == null)
            {
                entity = new SiteSetupModel();
            }
            else
            {
                var data = _mapper.Map<SiteSetup>(entity);
                _repositorySs.Create(data);
                RedirectToAction(nameof(Index));
            }
            return View();
        }
    }
}
