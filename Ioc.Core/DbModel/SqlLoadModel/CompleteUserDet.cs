namespace Ioc.Core.DbModel.SqlLoadModel
{
    public class CompleteUserDet : PublicBaseEntity
    {
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public string? City { get; set; }
        public string? State { get; set; }
        public string? Country { get; set; }
        public string? PostalCode { get; set; }
        public string? PhoneNumber { get; set; }
        public string? Email { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public string? JobTitle { get; set; }
        public string? Company { get; set; }
        public string? Department { get; set; }
        public string? ManagerName { get; set; }
        public DateTime? HireDate { get; set; }
        public double Salary { get; set; }
        public string? OfficeLocation { get; set; }
        public int EmployeeNumber { get; set; }
        public bool IsActive { get; set; }
        public DateTime? LastLoginDate { get; set; }
        public string? LastLoginIP { get; set; }
        public bool IsEmailVerified { get; set; }
        public string? ProfilePictureUrl { get; set; }
        public string? LinkedInProfile { get; set; }
        public string? TwitterProfile { get; set; }
        public string? FacebookProfile { get; set; }
        public string? InstagramProfile { get; set; }
        public string? PersonalWebsite { get; set; }
        public string? BlogUrl { get; set; }
        public string? GitHubProfile { get; set; }
        public string? StackOverflowProfile { get; set; }
        public string? Bio { get; set; }
        public string? Interests { get; set; }
        public string? Skills { get; set; }
        public string? Certifications { get; set; }
        public string? Languages { get; set; }
        public string? Education { get; set; }
        public string? WorkExperience { get; set; }
        public string? Projects { get; set; }
        public string? Publications { get; set; }
        public string? Awards { get; set; }
        public string? Memberships { get; set; }
        public string? Hobbies { get; set; }
        public string? FavoriteBooks { get; set; }
        public string? FavoriteMovies { get; set; }
        public string? FavoriteMusic { get; set; }
        public string? FavoriteQuotes { get; set; }
        public string? FavoritePlaces { get; set; }
        public string? AddressLine1 { get; set; }
        public string? AddressLine2 { get; set; }
        public string? AlternatePhoneNumber { get; set; }
        public string? EmergencyContactName { get; set; }
        public string? EmergencyContactPhone { get; set; }
        public string? EmergencyContactRelation { get; set; }
        public string? BankName { get; set; }
        public string? BankAccountNumber { get; set; }
        public string? BankBranch { get; set; }
        public string? BankIFSC { get; set; }
        public string? PAN { get; set; }
        public string? PassportNumber { get; set; }
        public DateTime? PassportExpiryDate { get; set; }
        public string? VisaNumber { get; set; }
        public DateTime? VisaExpiryDate { get; set; }
        public string? InsuranceProvider { get; set; }
        public string? InsurancePolicyNumber { get; set; }
        public DateTime? InsuranceExpiryDate { get; set; }
        public string? SocialSecurityNumber { get; set; }
        public string? DrivingLicenseNumber { get; set; }
        public DateTime? DrivingLicenseExpiryDate { get; set; }
        public string? VehicleRegistrationNumber { get; set; }
        public string? VehicleMake { get; set; }
        public string? VehicleModel { get; set; }
        public string? VehicleColor { get; set; }
        public DateTime? VehiclePurchaseDate { get; set; }
        public double VehiclePrice { get; set; }
        public string? CreditCardNumber { get; set; }
        public string? CreditCardType { get; set; }
        public DateTime? CreditCardExpiryDate { get; set; }
        public string? CreditCardCVV { get; set; }
        public string? UserName { get; set; }
        public string? PasswordHash { get; set; }
        public DateTime? PasswordLastChanged { get; set; }
        public bool TwoFactorEnabled { get; set; }
        public string? SecurityQuestion { get; set; }
        public string? SecurityAnswerHash { get; set; }
        public string? UserPreferencesJson { get; set; }
        public DateTime? AccountCreatedDate { get; set; }
        public DateTime? AccountLastUpdatedDate { get; set; }
        public bool IsAccountLocked { get; set; }
    }
}
