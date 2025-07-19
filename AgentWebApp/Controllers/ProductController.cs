using AutoMapper;
using Ioc.Core.DbModel.Models;
using Ioc.ObjModels.Model;
using Ioc.Service.Interfaces;
using Microsoft.AspNetCore.Mvc;
using NuGet.Protocol.Core.Types;

namespace IocContainer.Controllers
{
    public class ProductController : Controller
    {

        private readonly IProductService _repositoryPro;
        private readonly IMapper _mapper;
        private readonly ILogger<ProductModel> _logger;
        private readonly IWebHostEnvironment _hostingEnvironment;

        public ProductController(IProductService repositoryPro, IMapper mapper,
            ILogger<ProductModel> logger, IWebHostEnvironment hostingEnvironment)
        {
            _repositoryPro = repositoryPro;
            _mapper = mapper;
            _logger = logger;
            _hostingEnvironment = hostingEnvironment;
        }
        // GET: HomeController1
        public async Task<ActionResult> Index()
        {
            ProductList productList = new ProductList();
            productList.Products = _mapper.Map<List<ProductModel>>(await _repositoryPro.GetAllAsync(200));
            return View(productList);
        }

        // GET: HomeController1/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: HomeController1/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: HomeController1/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(ProductModel model)
        {

            if (ModelState.IsValid)
            {
                try
                {
                    if (model.ID != Guid.Empty && model.ID != null)
                    {
                        var data = await _repositoryPro.GetById((Guid)model.ID);
                        data.Name = model.Name;
                        data.Description = model?.Description;
                        await _repositoryPro.Update((Guid)model!.ID, data);
                        return RedirectToAction(nameof(Index));
                    }
                    else
                    {
                        var data = _mapper.Map<Product>(model);
                        await _repositoryPro.Create(data);
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

        // GET: HomeController1/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: HomeController1/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(int id, IFormCollection collection)
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

        // GET: HomeController1/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: HomeController1/Delete/5
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
