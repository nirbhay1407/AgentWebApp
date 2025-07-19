using Ioc.ObjModels.Model.SiteInfo;
using System.ComponentModel.DataAnnotations;

namespace Ioc.ObjModels.Model
{
    public class InvoiceModel : PublicBaseModel
    {
        public int InvoiceNumber { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateTime InvoiceDate { get; set; }

        public Guid? CompanyId { get; set; }
        public CompanyModel? Company { get; set; }


        public Guid? SalespersonId { get; set; }
        public SalesPersonModel Salesperson { get; set; }


        public Guid? CustomerId { get; set; }

        public CustomerModel Customer { get; set; }

        public List< ProductModel> Items { get; set; }

        [DataType(DataType.Currency)]
        public decimal TotalAmount { get; set; }

        public InvoiceModel()
        {
            Items = new List<ProductModel>();
        }

        public InvoiceModel(int invoiceNumber, DateTime invoiceDate, SalesPersonModel salesperson, CustomerModel customer, List<ProductModel> items)
        {
            InvoiceNumber = invoiceNumber;
            InvoiceDate = invoiceDate;
            Salesperson = salesperson;
            Customer = customer;
            Items = items;
            TotalAmount = CalculateTotalAmount();
        }

        private decimal CalculateTotalAmount()
        {
            decimal total = 0;
            foreach (var item in Items)
            {
                total += item.Price;
            }
            return total;
        }

        public override string ToString()
        {
            return $"Invoice #{InvoiceNumber} - Date: {InvoiceDate.ToShortDateString()}, Salesperson: {Salesperson.FirstName} {Salesperson.LastName}, Customer: {Customer.FirstName} {Customer.LastName}, Total Amount: {TotalAmount:C}";
        }
    }
}
