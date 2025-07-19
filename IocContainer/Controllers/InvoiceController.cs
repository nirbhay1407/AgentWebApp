using AutoMapper;
using Ioc.Core.DbModel.Models;
using Ioc.Core.DbModel.Models.SiteInfo;
using Ioc.Data.UnitOfWorkRepo;
using Ioc.ObjModels.Model;
using Ioc.Service.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace IocContainer.Controllers
{
    public class InvoiceController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        IInvoiceService _repository;
        private readonly IMapper _mapper;
        private readonly ILogger<InvoiceModel> _logger;
        private readonly IWebHostEnvironment _hostingEnvironment;
        /*IInvoiceService repository, IProductService repositoryPro*/
        public InvoiceController(IInvoiceService repository, IUnitOfWork unitOfWork, IMapper mapper,
            ILogger<InvoiceModel> logger, IWebHostEnvironment hostingEnvironment)
        {
            _repository = repository;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _logger = logger;
            _hostingEnvironment = hostingEnvironment;
        }

        // GET: CategoryController
        public ActionResult Index()
        {
            try
            {
                var coolest = _repository.GetAllInc();
                var coolestCategory = _unitOfWork.GetRepository<InvoiceNew>().GetAll().ToList();
                var data = _mapper.Map<IEnumerable<InvoiceModel>>(coolestCategory);
                _logger.LogInformation($"Returning Category.");
                return View(data);
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        public ActionResult Create()
        {
            ViewBag.Companys = new SelectList(_unitOfWork.GetRepository<Company>().GetAll().ToList(), "ID", "CompanyName");
            ViewBag.SalesPersons = new SelectList(_unitOfWork.GetRepository<SalesPerson>().GetAll().Select(sp => new
            {
                ID = sp.ID,
                FullName = sp.FirstName + " " + sp.LastName
            }).ToList(), "ID", "FullName");

            ViewBag.Customers = new SelectList(_unitOfWork.GetRepository<Customer>().GetAll().Select(sp => new
            {
                ID = sp.ID,
                FullName = sp.FirstName + " " + sp.LastName
            }).ToList(), "ID", "FullName");

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(InvoiceModel model)
        {

            if (ModelState.IsValid)
            {
                try
                {
                    if (model.ID != Guid.Empty && model.ID != null)
                    {
                        var data = await _unitOfWork.GetRepository<Invoice>().GetById((Guid)model.ID);
                        data.InvoiceDate = model.InvoiceDate;
                        data.TotalAmount = model.TotalAmount;
                        await _unitOfWork.GetRepository<Invoice>().Update((Guid)model!.ID, data);
                        return RedirectToAction(nameof(Index));
                    }
                    else
                    {
                        var data = _mapper.Map<Invoice>(model);
                        await _unitOfWork.GetRepository<Invoice>().Create(data);
                        return RedirectToAction(nameof(Index));
                    }

                }
                catch
                {
                    return View();
                }
            }
            return View();
        }
        public async Task<ActionResult> Details(Guid ID)
        {
            try
            {

                var coolestCasd = await _repository.GetWithAllOfData(ID);
                //var coolestCasd = _repository.GetByIdWithInclude(ID, "Customer").Result;
                coolestCasd.Items = _unitOfWork.GetRepository<Product>().GetAll(5).ToList();
                //var coolestCasd = _repository.GetByIdWithInclude(ID, lstOfInc).Result;
                //var coolestCasd = _repository.GetById(ID).Result;
                var data = _mapper.Map<InvoiceModel>(coolestCasd);
                _logger.LogInformation($"Returning Category.");
                return View(data);
            }
            catch (Exception ex)
            {
                //ex.ManualDBLog(nameof(InvoiceController), nameof(Details));
                throw;
            }
        }
    }
}
