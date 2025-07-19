using AutoMapper;
using Ioc.Core.DbModel.Models;
using Ioc.ObjModels.Model.CommonModel;
using Ioc.Service.Interfaces;
using Ioc.Service.Interfaces.Common;
using IocContainer.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Data.Entity;

namespace IocContainer.Controllers
{
    public class CategoryController : Controller
    {
        private readonly ICategoryRepository _repository;
        private readonly ICommonCategory _commonCategory;
        private readonly IMapper _mapper;
        private readonly ILogger<CategoryModel> _logger;
        private readonly IWebHostEnvironment _hostingEnvironment;

        public CategoryController(ICategoryRepository repository, ICommonCategory commonCategory, IMapper mapper,
            ILogger<CategoryModel> logger, IWebHostEnvironment hostingEnvironment)
        {
            _repository = repository;
            _commonCategory = commonCategory;
            _mapper = mapper;
            _logger = logger;
            _hostingEnvironment = hostingEnvironment;
        }

        // GET: CategoryController
        /*public ActionResult Index()
        {
            try
            {
                var coolestCategory = _repository.GetAll();
                var data = _mapper.Map<IEnumerable<CategoryModel>>(coolestCategory);
                _logger.LogInformation($"Returning Category.");
                return View(data);
            }
            catch (Exception ex)
            {
                throw;
            }
        }*/

        public async Task<ActionResult> Index(int pageNumber = 1, int pageSize = 10)
        {
            try
            {
                var totalItems = await _repository.GetCount();
                var categories = await _repository.GetPagedData(pageNumber, pageSize);
                var data = _mapper.Map<IEnumerable<CategoryModel>>(categories);

                var viewModel = new PaginatedList<CategoryModel>(data, totalItems, pageNumber, pageSize);

                _logger.LogInformation($"Returning Category.");
                return View(viewModel);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while fetching categories.");
                throw;
            }
        }



        // GET: CategoryController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        /*// GET: CategoryController/Create
        public ActionResult Create()
        {
            return View();
        }*/

        // POST: CategoryController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(CategoryModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {

                    // Get the path to the wwwroot folder
                    string webRootPath = _hostingEnvironment.WebRootPath;

                    // Create a unique filename for the uploaded file (e.g., using Guid)
                    if (model.File != null)
                    {
                        string uniqueFileName = Guid.NewGuid().ToString() + "_" + model.File.FileName;

                        // Combine the webRootPath with the relative path where you want to save the file
                        string filePath = Path.Combine(webRootPath, "Uploaded", uniqueFileName);

                        // Ensure the directory exists
                        Directory.CreateDirectory(Path.GetDirectoryName(filePath));

                        // Create a FileStream to write the file to the specified path
                        using (var stream = new FileStream(filePath, FileMode.Create))
                        {
                            model.File.CopyTo(stream);
                        }
                    }
                    if (await _repository.GetDynamicDataExists("Name", model.Name, model.ID))
                    {
                        ModelState.AddModelError("Name", "Name already exists");
                        return View();
                    }
                    if (model.ID != null)
                    {
                        var data = _repository.GetById((Guid)model.ID).Result;
                        data.Name = model.Name;
                        data.Description = model.Description;
                        await _repository.Update((Guid)model.ID, data);
                        return RedirectToAction(nameof(Index));
                    }
                    else
                    {
                        
                        var data = _mapper.Map<Category>(model);
                        await _repository.Create(data);
                        return RedirectToAction(nameof(Index));
                    }

                }
                catch(Exception ex)
                {
                    ModelState.AddModelError("Exception", ex.InnerException?.Message.ToString());
                    //ex.ManualDBLog(nameof(CategoryController), nameof(Create));
                    return View();
                }

            }
            return View();
        }

        // GET: CategoryController/Edit/5
        public async Task<ActionResult> Create(Guid? id)
        {
            if (id == null)
                return View();
            var data = await _repository.GetById((Guid)id);
            var result = _mapper.Map<CategoryModel>(data);
            return View(result);
        }

        // POST: CategoryController/Edit/5
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

        // GET: CategoryController/Delete/5
        public ActionResult Delete(int id)
        {

            return View();
        }

        // POST: CategoryController/Delete/5
        [HttpDelete]
        // [ValidateAntiForgeryToken]
        public async Task<ActionResult> Delete(Guid id)
        {
            try
            {
                await _repository.Delete(id);
                return Ok(new ResponseModel() { Status = 200, Message = "Deleted Sucessfully" });
            }
            catch
            {
                return Ok(new ResponseModel() { Status = 500, Message = "Error" });
            }
        }


        /* private readonly ICategoryRepository _repository;
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
         }*/

        [HttpGet]
        public List<SelectListItem> GetCountriesName(int val)
        {
            List<SelectListItem> lst = new List<SelectListItem>();
            lst.Add(new SelectListItem() { Value = "1", Text = "India" });
            lst.Add(new SelectListItem() { Value = "2", Text = "Nepal" });
            lst.Add(new SelectListItem() { Value = "3", Text = "America" });
            return lst;

        }
    }
}
