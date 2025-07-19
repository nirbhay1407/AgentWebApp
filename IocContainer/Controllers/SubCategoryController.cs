using AutoMapper;
using Ioc.Core.DbModel.Models;
using Ioc.Data.UnitOfWorkRepo;
using Ioc.Service.Interfaces;
using IocContainer.Data;
using IocContainer.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using OfficeOpenXml;
using System.Data;
using System.Linq;
using System.Reflection;

namespace IocContainer.Controllers
{
    public class SubCategoryController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public SubCategoryController(IMapper mapper, IUnitOfWork unitOfWork)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }
        public IActionResult Index()
        {
            var data = _unitOfWork.GetRepository<SubCategory>().GetAllWithInclude("Category").ToList();
            var result = _mapper.Map<IEnumerable<SubCategoryModel>>(data);
            return View(result);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(SubCategoryModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    if (model.ID != Guid.Empty && model.ID != null)
                    {
                        List<string> differences;
                        var dec = JsonConvert.DeserializeObject<SubCategoryModel>(model?.OldValue);
                        AreEqual(model, dec,out differences);
                        var data = await _unitOfWork.GetRepository<SubCategory>().GetById((Guid)model.ID);
                        data.Name = model!.Name;
                        data.Description = model?.Description;
                        await _unitOfWork.GetRepository<SubCategory>().Update((Guid)model!.ID, data);
                        return RedirectToAction(nameof(Index));
                    }
                    else
                    {
                        var data = _mapper.Map<SubCategory>(model);
                        await _unitOfWork.GetRepository<SubCategory>().Create(data);
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

        // GET: CategoryController/Edit/5
        public async Task<ActionResult> Create(Guid? id)
        {
            ViewBag.Category = new SelectList(_unitOfWork.GetRepository<Category>().GetAll().ToList(), "ID", "Name");
            if (id == null)
                return View();

            var data = await _unitOfWork.GetRepository<SubCategory>().GetById((Guid)id);
            var result = _mapper.Map<SubCategoryModel>(data);
            result.OldValue = JsonConvert.SerializeObject(result);
            return View(result);
        }

        public IActionResult Download()
        {

            var data = _unitOfWork.GetRepository<SubCategory>().GetAllWithInclude("Category").AsEnumerable();
            if (data == null)
            {
                return View();
            }
            else
            {
                DataTable dt = new DataTable();
                var data2 = CustomLINQtoDataSetMethods.CopyToDataTable(data);
                ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
                MemoryStream content = new MemoryStream(); // Gets disposed by FileStreamResult.        
                using (ExcelPackage package = new ExcelPackage(content))
                {
                    ExcelWorksheet ws = package.Workbook.Worksheets.Add("Accounts");
                    ws.Cells["A1"].LoadFromDataTable(data2, true);
                    package.Save();
                }

                content.Position = 0;

                return new FileStreamResult(content, "application/octet-stream")
                {
                    FileDownloadName = "test.xlsx"
                };
            }
        }


        public  bool AreEqual<T>(T obj1, T obj2, out List<string> differences)
        {
            differences = new List<string>();
            var ignoreThis = new List<string>();
            ignoreThis.Add("OldValue");
            if (obj1 == null || obj2 == null)
            {
                differences.Add("One of the objects is null.");
                return false;
            }

            Type type = typeof(T);
            PropertyInfo[] properties = type.GetProperties(BindingFlags.Public | BindingFlags.Instance);

            foreach (PropertyInfo property in properties)
            {
                if (ignoreThis.Contains(property.Name)) continue;
                object value1 = property.GetValue(obj1);
                object value2 = property.GetValue(obj2);

                if (!Equals(value1, value2))
                {
                    differences.Add($"Property {property.Name} differs. {value1} != {value2}");
                }
            }

            return !differences.Any();
        }

    }
}
