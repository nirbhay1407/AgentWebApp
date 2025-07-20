using AutoMapper;
using CommonHelper.Validation;
using Ioc.Core.DbModel.Models;
using Ioc.ObjModels.Model;
using Ioc.Service.Interfaces;
using Ioc.Service.Interfaces.Validation;
using Microsoft.AspNetCore.Mvc;
using OfficeOpenXml;

namespace AgentWebApp.Controllers
{
    public class CustomerController : Controller
    {
        private readonly ICustomerService _repository;
        private readonly IMapper _mapper;
        private readonly IValidationRuleRepository _validationRuleRepository;

        public CustomerController(ICustomerService repository, IMapper mapper, IValidationRuleRepository validationRuleRepository)
        {
            _repository = repository;
            _mapper = mapper;
            _validationRuleRepository = validationRuleRepository;

        }

        private static List<CustomerModel> _excelData = new List<CustomerModel>();

        public async Task<ActionResult> Index()
        {
            _excelData.Clear();
            return View(_mapper.Map<IReadOnlyList<CustomerModel>>(await _repository.GetAllAsync()));
        }

        public IActionResult UploadData()
        {
           
            return View(_excelData);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Upload(IFormFile file)
        {
            if (file == null || file.Length == 0)
            {
                //ModelState.AddModelError(string.Empty, "File not found or empty.");
                return View("UploadData", _excelData);
            }
            if (file != null && file.Length > 0)
            {
                using (var stream = new MemoryStream())
                {
                    await file.CopyToAsync(stream);
                    ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
                    using (var package = new ExcelPackage(stream))
                    {
                        var worksheet = package.Workbook.Worksheets[0];
                        var rowCount = worksheet.Dimension.Rows;
                        var colCount = worksheet.Dimension.Columns;
                        DateTime dob = new DateTime();
                        _excelData.Clear();
                        for (int row = 2; row <= rowCount; row++)
                        {
                            var data = new CustomerModel
                            {
                                //ID = new Guid(worksheet?.Cells[row, 1].Text),
                                ID = Guid.NewGuid(),
                                FirstName = worksheet.Cells[row, 2].Text,
                                LastName = worksheet.Cells[row, 3].Text,
                                Contact = worksheet.Cells[row, 4].Text,
                                Email = worksheet.Cells[row, 5].Text,
                                DateOfBirth = DateTime.TryParse(worksheet.Cells[row, 6].Text, out dob) ? dob.ToString() : "",
                                CompanyName = worksheet.Cells[row, 14].Text,
                                Mobile = worksheet.Cells[row, 15].Text
                            };

                            var (isValid, validationMsg, warningMsg) = await ValidationHelper.ValidateAsync(data, _validationRuleRepository);

                            data.IsValid = isValid;
                            data.ValidationMsg = validationMsg;
                            data.WarningMsg = warningMsg;

                            _excelData.Add(data);
                            /*
                                                        if (ValidationHelper.TryValidate(data, out var validationResults))
                                                        {
                                                            data.IsValid = true;
                                                            data.ValidationMsg = string.Empty;
                                                            _excelData.Add(data);
                                                        }
                                                        else
                                                        {
                                                            data.IsValid = false;
                                                            data.ValidationMsg = string.Join("; ", validationResults.Select(vr => vr.ErrorMessage));
                                                            _excelData.Add(data);
                                                        }*/

                        }
                    }
                }
            }
            return RedirectToAction(nameof(UploadData));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Save()
        {
            // Save _excelData to the database
            // Implement your database save logic here

            // Example:
             await _repository.CreateInRange(_mapper.Map<List<Customer>>(_excelData));

            return RedirectToAction(nameof(Index));
        }


        /*[HttpGet("GetWithoutCache")]
        public async Task<IEnumerable<Customer>> GetWithoutCache()
        {
            return _repository.GetAll();
        }

        [HttpGet]
        public async Task<ActionResult> Index()
        {
            return View(_mapper.Map<IReadOnlyList<CustomerModel>>(await _repository.GetAllAsync()));
        }
        [HttpGet("GetById/{id}")]
        public async Task<ActionResult<Customer>> Get(Guid id)
        {
            var customer = await _repository.GetById(id);
            if (customer == null)
            {
                return NotFound();
            }
            return customer;
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(Guid id, Customer customer)
        {
            if (id != customer.ID)
            {
                return BadRequest();
            }
            await _repository.Update(id, customer);
            return NoContent();
        }
        [HttpPost]
        public async Task<ActionResult<Customer>> Post(Customer customer)
        {
            await _repository.Create(customer);
            return CreatedAtAction("Get", new { id = customer.ID }, customer);
        }
        [HttpDelete("{id}")]
        public async Task<ActionResult<Customer>> Delete(Guid id)
        {
            var customer = await _repository.GetById(id);
            if (customer == null)
            {
                return NotFound();
            }
            await _repository.Delete(id);
            return customer;
        }*/
    }
}
