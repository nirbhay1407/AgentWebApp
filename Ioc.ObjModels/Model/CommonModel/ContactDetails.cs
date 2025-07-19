using Ioc.ObjModels.Model;
using System.ComponentModel.DataAnnotations;

namespace Ioc.ObjModels.Model.CommonModel
{
    public class ContactDetails : PublicBaseModel
    {
        [Required]
        [Phone]
        public string PhoneNumber { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        public string Fax { get; set; }

        public ContactDetails() { }

        public ContactDetails(string phoneNumber, string email, string fax = null)
        {
            PhoneNumber = phoneNumber;
            Email = email;
            Fax = fax;
        }

        public override string ToString()
        {
            return $"Phone: {PhoneNumber}, Email: {Email}, Fax: {Fax}";
        }
    }
}
