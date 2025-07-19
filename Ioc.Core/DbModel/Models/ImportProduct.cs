using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ioc.Core.DbModel.Models
{
    public class ImportProduct: PublicBaseEntity
    {
        public string? category { get; set; }
        public string? Out_of_Stock { get; set; }
        public string? sku { get; set; }
        public decimal price { get; set; }
        public int qty { get; set; }
    }
}
