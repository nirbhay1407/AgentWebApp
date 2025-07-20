using Ioc.ObjModels;
using System.ComponentModel.DataAnnotations;

namespace AgentWebApp.Models
{
    public class CategoryModel : PublicBaseModel
    {
        [Required(ErrorMessage = "This field is required")]
        public string Name { get; set; }
        public string? Description { get; set; }
        public string? FromDate { get; set; }
        public string? ToDate { get; set; }
        public IFormFile? File { get; set; }
    }
}
