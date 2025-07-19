using Microsoft.AspNetCore.Mvc.Rendering;

namespace Ioc.ObjModels.Model
{
    public class ProductList
    {
        public List<ProductModel>? Products { get; set; }
        public ProductItem? Item { get; set; }
    }

    public class ProductModel : PublicBaseModel
    {
        public int product_id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public bool Status { get; set; }
        public int Quantity { get; set; }
    }
    public class ProductItem
    {
        public List<SelectListItem>? product_id { get; set; }
    }
}