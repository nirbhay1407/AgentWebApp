using Ioc.Core.DbModel.Models;
using System.ComponentModel.DataAnnotations;

namespace Ioc.ObjModels.Model.CommonModel
{
    public class BankDetailsModel
    {
        [Required]
        [StringLength(100)]
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

        public BankDetailsModel() { }

        public BankDetailsModel(string bankName, string accountHolderName, string accountNumber, string routingNumber, AccountType accountType, string branchName = null, string branchAddress = null, string swiftCode = null)
        {
            BankName = bankName;
            AccountHolderName = accountHolderName;
            AccountNumber = accountNumber;
            RoutingNumber = routingNumber;
            AccountType = accountType;
            BranchName = branchName;
            BranchAddress = branchAddress;
            SwiftCode = swiftCode;
        }

        public override string ToString()
        {
            return $"Bank: {BankName}, Account Holder: {AccountHolderName}, Account Number: {AccountNumber}, Routing Number: {RoutingNumber}, Account Type: {AccountType}, Branch: {BranchName}, Branch Address: {BranchAddress}, SWIFT: {SwiftCode}";
        }
    }


}
