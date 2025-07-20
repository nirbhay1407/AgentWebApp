using AutoMapper;
using Ioc.Service.Interfaces.Common;
using Ioc.Service.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Ioc.ObjModels.Model.SiteInfo;

namespace AgentWebApp.Controllers
{
    public class CompanyController : Controller
    {
        private readonly ICompanyService _repository;
        private readonly ICommonCategory _commonCategory;
        private readonly IMapper _mapper;
        private readonly ILogger<CompanyModel> _logger;
        private readonly IWebHostEnvironment _hostingEnvironment;

        public CompanyController(ICompanyService repository, ICommonCategory commonCategory, IMapper mapper,
            ILogger<CompanyModel> logger, IWebHostEnvironment hostingEnvironment)
        {
            _repository = repository;
            _commonCategory = commonCategory;
            _mapper = mapper;
            _logger = logger;
            _hostingEnvironment = hostingEnvironment;
        }

        // GET: CompanyController
        public async Task<ActionResult> Index()
        {
            return View(_mapper.Map<List<CompanyModel>>(await _repository.GetAllAsync(100)));
        }

        // GET: CompanyController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: CompanyController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: CompanyController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: CompanyController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: CompanyController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: CompanyController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: CompanyController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
