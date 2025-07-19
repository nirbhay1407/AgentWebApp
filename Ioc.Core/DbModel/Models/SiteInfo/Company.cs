using Ioc.Core;
using Ioc.Core.DbModel;
using System.ComponentModel.DataAnnotations;

namespace Ioc.Core.DbModel.Models.SiteInfo
{
    public class Company : PublicBaseEntity
    {
        [Required]
        [StringLength(100)]
        public string CompanyName { get; set; }

        [Required]
        public Address CompanyAddress { get; set; }

        [Required]
        public Contact ContactDetails { get; set; }
        public BankDetails BankDetails { get; set; }

        public Company() { }

        public Company(string companyName, Address companyAddress, Contact contactDetails)
        {
            CompanyName = companyName;
            CompanyAddress = companyAddress;
            ContactDetails = contactDetails;
        }

        public override string ToString()
        {
            return $"{CompanyName}, Address: {CompanyAddress}, Contact: {ContactDetails}";
        }
    }
}
