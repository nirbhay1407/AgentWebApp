namespace Ioc.Core.DbModel.Models
{
    public class Product : PublicBaseEntity
    {
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public bool Status { get; set; }
        public int Quantity { get; set; }
    }
}
