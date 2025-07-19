using Ioc.Core;

namespace IocContainer.Models
{
    public class ImportProduct: PublicBaseEntity
    {
        public string?category { get; set; }
        public string? Out_of_Stock { get; set; }
        public string? sku { get; set; }
        public decimal price { get; set; }
        public int qty { get; set; }
    }
}
