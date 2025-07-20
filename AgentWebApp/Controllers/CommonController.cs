using AutoMapper;
using Ioc.Service.Interfaces;
using Ioc.Service.Interfaces.Common;
using Ioc.Service.Services.Common;
using Microsoft.AspNetCore.Mvc;

namespace AgentWebApp.Controllers
{
    public class CommonController : Controller
    {
        private readonly ISettingService _repository;
        private readonly IMapper _mapper;

        public CommonController(ISettingService repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult UpdateDB()
        {
            _repository.UpdateDB();
            return Ok();
        }
    }
}
