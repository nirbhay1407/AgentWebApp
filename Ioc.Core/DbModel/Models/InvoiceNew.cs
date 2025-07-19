using Ioc.Core.DbModel.Models.SiteInfo;
using System.ComponentModel.DataAnnotations.Schema;

namespace Ioc.Core.DbModel.Models
{
    public class InvoiceNew : PublicBaseEntity
    {
        public int InvoiceNumber { get; set; }
        public DateTime InvoiceDate { get; set; }
        public Guid? CompanyId { get; set; }
        [ForeignKey("CompanyId")]
        public Company? Company { get; set; }
        public Guid? SalespersonId { get; set; }
        [ForeignKey("SalespersonId")]
        public SalesPerson? Salesperson { get; set; }
        public Guid? CustomerId { get; set; }
        [ForeignKey("CustomerId")]
        public Customer? Customer { get; set; }
        [NotMapped]
        public List<Product>? Items { get; set; }
        public decimal TotalAmount { get; set; }

    }
}
