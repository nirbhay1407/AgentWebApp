using System.ComponentModel.DataAnnotations;

namespace Ioc.Core.DbModel.Models
{
    public class BankDetails : PublicBaseEntity
    {
        public string BankName { get; set; }

        [Required]
        [StringLength(50)]
        public string AccountHolderName { get; set; }

        [Required]
        [StringLength(20)]
        [DataType(DataType.CreditCard)] // Although not a credit card, this ensures numeric validation
        public string AccountNumber { get; set; }

        [Required]
        [StringLength(9, MinimumLength = 9)]
        public string RoutingNumber { get; set; }

        [Required]
        public AccountType AccountType { get; set; }

        [StringLength(100)]
        public string BranchName { get; set; }

        [StringLength(100)]
        public string BranchAddress { get; set; }

        [StringLength(50)]
        public string SwiftCode { get; set; }
    }

    public enum AccountType
    {
        Checking,
        Savings,
        Business,
        Salary,
        Other
    }
}
