using ClosedXML.Excel;
using Ioc.Core.DbModel.Models;
using Microsoft.AspNetCore.Mvc;
using OfficeOpenXml;
using System.Data;

namespace AgentWebApp.Controllers
{
    public class ImportProductController : Controller
    {

        public IActionResult Index()
        {
            return View(); // Return your view
        }

        [HttpPost]
        public IActionResult LoadExcelData(IFormFile file)
        {
            if (file == null || file.Length == 0)
            {
                return BadRequest("Please upload a valid Excel file.");
            }

            try
            {
                // Load Excel data into a DataTable
                DataTable dataTable = LoadExcelDataFromClosedXML(file);

                // Optionally: Map to strongly-typed model
                List<ImportProduct> products = MapToImportProduct(dataTable);

                // Return data as JSON (for frontend display or API response)
                return Json(products);
            }
            catch (Exception ex)
            {
                return BadRequest($"Error reading Excel file: {ex.Message}");
            }
        }

        private DataTable LoadExcelDataFromClosedXML(IFormFile file)
        {
            DataTable dataTable = new DataTable();

            using (var stream = file.OpenReadStream())
            using (var workbook = new XLWorkbook(stream))
            {
                var worksheet = workbook.Worksheet(1); // Load the first worksheet
                var range = worksheet.RangeUsed();

                // Add columns to the DataTable
                foreach (var cell in range.FirstRow().Cells())
                {
                    dataTable.Columns.Add(cell.Value.ToString());
                }

                // Add rows to the DataTable
                foreach (var row in range.RowsUsed().Skip(1)) // Skip header row
                {
                    DataRow dataRow = dataTable.NewRow();
                    int colIndex = 0;

                    foreach (var cell in row.Cells())
                    {
                        dataRow[colIndex++] = cell.Value.ToString();
                    }

                    dataTable.Rows.Add(dataRow);
                }
            }

            /*using (var stream = new MemoryStream())
            {
                file.CopyTo(stream);
                ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
                using (var package = new ExcelPackage(stream))
                {
                    var worksheet = package.Workbook.Worksheets[0];
                    var rowCount = worksheet.Dimension.Rows;
                    var colCount = worksheet.Dimension.Columns;
                    for (int row = 2; row <= rowCount; row++)
                    {
                        DataRow dataRow = dataTable.NewRow();
                        int colIndex = 0;

                        foreach (var cell in worksheet[row])
                        {
                            dataRow[colIndex++] = cell.Value.ToString();
                        }

                        dataTable.Rows.Add(dataRow);
                    }
                }
            }*/

            return dataTable;
        }

        private List<ImportProduct> MapToImportProduct(DataTable dataTable)
        {
            var products = new List<ImportProduct>();

            foreach (DataRow row in dataTable.Rows)
            {
                products.Add(new ImportProduct
                {
                    category = row["category"]?.ToString(),
                    Out_of_Stock = row["Out_of_Stock"]?.ToString(),
                    sku = row["sku"]?.ToString(),
                    price = decimal.TryParse(row["price"]?.ToString(), out var price) ? price : 0,
                    qty = int.TryParse(row["qty"]?.ToString(), out var qty) ? qty : 0
                });
            }

            return products;
        }
    }
}


