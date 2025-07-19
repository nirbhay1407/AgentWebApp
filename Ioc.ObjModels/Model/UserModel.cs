using System.ComponentModel.DataAnnotations;

namespace Ioc.ObjModels.Model
{
    public class UserModel : PublicBaseModel
    {
        [Display(Name = "First Name")]
        public string? FirstName { get; set; }
        [Display(Name = "Last Name")]
        public string? LastName { get; set; }
        public string? Address { get; set; }
        [Display(Name = "User Name")]
        public string? UserName { get; set; }
        public string? Email { get; set; }
        public string? Password { get; set; }
        public UserProfileModel? userProfile { get; set; }
    }
}
