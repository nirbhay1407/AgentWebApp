using AutoMapper;
using Ioc.ObjModels.Model.SqlLoadCheckModel;
using Ioc.Service.Interfaces;
using Ioc.Service.Interfaces.Common;
using AgentWebApp.Models;
using Microsoft.AspNetCore.Mvc;
using System.Drawing.Printing;

namespace AgentWebApp.Controllers
{
    public class CompleteUserDetailController : Controller
    {
        private readonly ICompleteUserDetailsService _repository;
        private readonly ICommonCategory _commonCategory;
        private readonly IMapper _mapper;
        private readonly ILogger<CompleteUserDetails> _logger;
        private readonly IWebHostEnvironment _hostingEnvironment;

        public CompleteUserDetailController(ICompleteUserDetailsService repository, ICommonCategory commonCategory, IMapper mapper,
            ILogger<CompleteUserDetails> logger, IWebHostEnvironment hostingEnvironment)
        {
            _repository = repository;
            _commonCategory = commonCategory;
            _mapper = mapper;
            _logger = logger;
            _hostingEnvironment = hostingEnvironment;
        }

        // GET: CategoryController
        public async Task<ActionResult> Index(int pageNumber = 1, int pageSize = 10)
        {
            try
            {
                var totalItems = await _repository.GetCount();

                var coolestCategory =await _repository.GetPagedData(pageNumber, pageSize);
                var data = _mapper.Map<IEnumerable<CompleteUserDetails>>(coolestCategory);
                _logger.LogInformation($"Returning CompleteUserDetails.");
                var viewModel = new PaginatedList<CompleteUserDetails>(data, totalItems, pageNumber, pageSize);
                return View(viewModel);
            }
            catch (Exception ex)
            {
                throw;
            }
        }


    }
}
