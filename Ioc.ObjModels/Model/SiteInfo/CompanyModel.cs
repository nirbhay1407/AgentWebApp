using Ioc.Core;
using Ioc.ObjModels.Model.CommonModel;
using System.ComponentModel.DataAnnotations;

namespace Ioc.ObjModels.Model.SiteInfo
{
    public class CompanyModel : PublicBaseEntity
    {
        [Required]
        [StringLength(100)]
        public string CompanyName { get; set; }

        [Required]
        public AddressModel CompanyAddress { get; set; }

        [Required]
        public ContactDetailsModel ContactDetails { get; set; }
        public BankDetailsModel BankDetails { get; set; }

        public CompanyModel() { }

        public CompanyModel(string companyName, AddressModel companyAddress, ContactDetailsModel contactDetails)
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
