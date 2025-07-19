namespace Ioc.Core.DbModel.Models
{
    public class Invoice : PublicBaseEntity
    {
        public int InvoiceNumber { get; set; }
        public DateTime InvoiceDate { get; set; }
        //public Guid? SalespersonId { get; set; }
        public SalesPerson? Salesperson { get; set; }
        //public Guid? CustomerId { get; set; }
        public Customer? Customer { get; set; }
        public List<Product>? Items { get; set; }
        public decimal TotalAmount { get; set; }

    }
}
