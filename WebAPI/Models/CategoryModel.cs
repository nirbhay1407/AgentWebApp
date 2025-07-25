using Ioc.ObjModels;
using System.ComponentModel.DataAnnotations;

namespace WebAPI.Models
{
    public class CategoryModel : PublicBaseModel
    {
        [Required(ErrorMessage = "THis field is required")]
        public string Name { get; set; }
        public string? Description { get; set; }
        public string? FromDate { get; set; }
        public string? ToDate { get; set; }
        public IFormFile? File { get; set; }
    }
}
