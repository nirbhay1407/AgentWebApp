using Ioc.Core;
using System.ComponentModel.DataAnnotations;

namespace Ioc.Core.DbModel.Models
{
    public class Contact : PublicBaseEntity
    {
        [Required]
        [Phone]
        public string PhoneNumber { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        public string Fax { get; set; }

        public Contact() { }

        public Contact(string phoneNumber, string email, string fax = null)
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
