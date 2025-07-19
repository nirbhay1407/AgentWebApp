using Ioc.ObjModels.Model.CommonModel;
using System.ComponentModel.DataAnnotations;

namespace Ioc.ObjModels.Model
{
    public class CustomerModel : PublicBaseModel
    {

        [Required]
        [StringLength(50)]
        public string? FirstName { get; set; }

        [Required]
        [StringLength(50)]
        public string? LastName { get; set; }

        [Required]
        [StringLength(20)]
        public string? Contact { get; set; }

        [Required]
        [EmailAddress]
        public string? Email { get; set; }

        [Required]
        public string? DateOfBirth { get; set; }
        public AddressModel? Address { get; set; }


        [StringLength(100)]
        public string? CompanyName { get; set; }

        [Required]
        [StringLength(20)]
        public string? Mobile { get; set; }

        public bool IsValid { get; set; }
        public string? ValidationMsg { get; set; }
        public string? WarningMsg { get; set; }
    }
}
