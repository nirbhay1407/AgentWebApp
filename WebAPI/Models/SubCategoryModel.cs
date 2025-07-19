using Ioc.ObjModels;
using System.ComponentModel.DataAnnotations;

namespace WebAPI.Models
{
    public class SubCategoryModel : PublicBaseModel
    {
        [Required(ErrorMessage = "This field is required")]
        public string Name { get; set; }
        public string? Description { get; set; }
        //public string? AddedDate { get; set; }
        [Required(ErrorMessage = "This field is required")]
        public Guid? CategoryId { get; set; }
        public CategoryModel? Category { get; set; }
    }
}
