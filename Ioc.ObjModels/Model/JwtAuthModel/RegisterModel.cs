using System.ComponentModel.DataAnnotations;

namespace Ioc.ObjModels.Model.JwtAuthModel
{
    public class RegisterModel
    {
        [Required(ErrorMessage = "User Name is required")]
        public string? Username { get; set; }

        [EmailAddress]
        [Required(ErrorMessage = "Email is required")]
        public string? Email { get; set; }

        [Required(ErrorMessage = "Password is required")]
        public string? Password { get; set; }
        public string RoleName { get; set; }
    }
}
