using AutoMapper;
using Bogus;
using Ioc.Core.DbModel.SqlLoadModel;
using Ioc.ObjModels.Model.SqlLoadCheckModel;
using Ioc.Service.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers
{
    public class CompleteUserDetailController : Controller
    {
        private readonly ICompleteUserDetailsService _repository;
        private readonly IMapper _mapper;
        private readonly ILogger<CompleteUserDet> _logger;



        public CompleteUserDetailController(ICompleteUserDetailsService repository, IMapper mapper, ILogger<CompleteUserDet> logger)
        {
            this._repository = repository;
            _mapper = mapper;
            _logger = logger;
        }
        [HttpGet("GetWithoutCache")]
        public async Task<IEnumerable<CompleteUserDet>> GetWithoutCache()
        {
            _logger.LogInformation("Getting Data started GetWithoutCache");
            return _repository.GetAll(10000);

        }

        [HttpGet("GetWithCache")]
        public async Task<IReadOnlyList<CompleteUserDet>> Get()
        {
            _logger.LogInformation("Getting Data started GetWithCache");
            return _mapper.Map<IReadOnlyList<CompleteUserDet>>(await _repository.GetAllAsync(10000));
        }

        [HttpGet("SaveFakeData/{howMuch}")]
        public async Task<IActionResult> SaveFakeData(int howMuch)
        {
            var dataToCreate = _mapper.Map<List<CompleteUserDet>>(GetFakeData(howMuch));
            await _repository.CreateInRange(dataToCreate);
            return Ok($"{howMuch} TIMES DATA IS CREATED");
        }


        private List<CompleteUserDetails> GetFakeData(int recordsToInsert)
        {
            var bogus = new Faker<CompleteUserDetails>()
        .RuleFor(p => p.FirstName, f => f.Name.FirstName())
        .RuleFor(p => p.LastName, f => f.Name.LastName())
        .RuleFor(p => p.Address, f => f.Address.StreetAddress())
        .RuleFor(p => p.City, f => f.Address.City())
        .RuleFor(p => p.State, f => f.Address.State())
        .RuleFor(p => p.Country, f => f.Address.Country())
        .RuleFor(p => p.PostalCode, f => f.Address.ZipCode())
        .RuleFor(p => p.PhoneNumber, f => f.Phone.PhoneNumber())
        .RuleFor(p => p.Email, f => f.Internet.Email())
        .RuleFor(p => p.DateOfBirth, f => f.Date.Past(30))
        .RuleFor(p => p.JobTitle, f => f.Name.JobTitle())
        .RuleFor(p => p.Company, f => f.Company.CompanyName())
        .RuleFor(p => p.Department, f => f.Commerce.Department())
        .RuleFor(p => p.ManagerName, f => f.Name.FullName())
        .RuleFor(p => p.HireDate, f => f.Date.Past(10))
        .RuleFor(p => p.Salary, f => (double)f.Finance.Amount(30000, 100000))
        .RuleFor(p => p.OfficeLocation, f => f.Address.City())
        .RuleFor(p => p.EmployeeNumber, f => f.Random.Int(1000, 9999))
        .RuleFor(p => p.IsActive, f => f.Random.Bool())
        .RuleFor(p => p.LastLoginDate, f => f.Date.Recent())
        .RuleFor(p => p.LastLoginIP, f => f.Internet.Ip())
        .RuleFor(p => p.IsEmailVerified, f => f.Random.Bool())
        .RuleFor(p => p.ProfilePictureUrl, f => f.Internet.Avatar())
        .RuleFor(p => p.LinkedInProfile, f => f.Internet.Url())
        .RuleFor(p => p.TwitterProfile, f => f.Internet.Url())
        .RuleFor(p => p.FacebookProfile, f => f.Internet.Url())
        .RuleFor(p => p.InstagramProfile, f => f.Internet.Url())
        .RuleFor(p => p.PersonalWebsite, f => f.Internet.Url())
        .RuleFor(p => p.BlogUrl, f => f.Internet.Url())
        .RuleFor(p => p.GitHubProfile, f => f.Internet.Url())
        .RuleFor(p => p.StackOverflowProfile, f => f.Internet.Url())
        .RuleFor(p => p.Bio, f => f.Lorem.Paragraph())
        .RuleFor(p => p.Interests, f => f.Lorem.Word())
        .RuleFor(p => p.Skills, f => f.Lorem.Word())
        .RuleFor(p => p.Certifications, f => f.Lorem.Word())
        .RuleFor(p => p.Languages, f => f.Lorem.Word())
        .RuleFor(p => p.Education, f => f.Lorem.Word())
        .RuleFor(p => p.WorkExperience, f => f.Lorem.Word())
        .RuleFor(p => p.Projects, f => f.Lorem.Word())
        .RuleFor(p => p.Publications, f => f.Lorem.Word())
        .RuleFor(p => p.Awards, f => f.Lorem.Word())
        .RuleFor(p => p.Memberships, f => f.Lorem.Word())
        .RuleFor(p => p.Hobbies, f => f.Lorem.Word())
        .RuleFor(p => p.FavoriteBooks, f => f.Lorem.Word())
        .RuleFor(p => p.FavoriteMovies, f => f.Lorem.Word())
        .RuleFor(p => p.FavoriteMusic, f => f.Lorem.Word())
        .RuleFor(p => p.FavoriteQuotes, f => f.Lorem.Sentence())
        .RuleFor(p => p.FavoritePlaces, f => f.Address.City())
        .RuleFor(p => p.AddressLine1, f => f.Address.StreetAddress())
        .RuleFor(p => p.AddressLine2, f => f.Address.StreetAddress())
        .RuleFor(p => p.AlternatePhoneNumber, f => f.Phone.PhoneNumber())
        .RuleFor(p => p.EmergencyContactName, f => f.Name.FullName())
        .RuleFor(p => p.EmergencyContactPhone, f => f.Phone.PhoneNumber())
        .RuleFor(p => p.EmergencyContactRelation, f => f.PickRandom(new[] { "Parent", "Sibling", "Friend", "Spouse" }))
        .RuleFor(p => p.BankName, f => f.Finance.AccountName())
        .RuleFor(p => p.BankAccountNumber, f => f.Finance.Account())
        .RuleFor(p => p.BankBranch, f => f.Address.StreetAddress())
        .RuleFor(p => p.BankIFSC, f => f.Random.AlphaNumeric(11))
        .RuleFor(p => p.PAN, f => f.Random.AlphaNumeric(10))
        .RuleFor(p => p.PassportNumber, f => f.Random.AlphaNumeric(9))
        .RuleFor(p => p.PassportExpiryDate, f => f.Date.Future())
        .RuleFor(p => p.VisaNumber, f => f.Random.AlphaNumeric(12))
        .RuleFor(p => p.VisaExpiryDate, f => f.Date.Future())
        .RuleFor(p => p.InsuranceProvider, f => f.Company.CompanyName())
        .RuleFor(p => p.InsurancePolicyNumber, f => f.Random.AlphaNumeric(15))
        .RuleFor(p => p.InsuranceExpiryDate, f => f.Date.Future())
        .RuleFor(p => p.SocialSecurityNumber, f => f.Random.Replace("###-##-####"))
        .RuleFor(p => p.DrivingLicenseNumber, f => f.Random.AlphaNumeric(12))
        .RuleFor(p => p.DrivingLicenseExpiryDate, f => f.Date.Future())
        .RuleFor(p => p.VehicleRegistrationNumber, f => f.Random.AlphaNumeric(10))
        .RuleFor(p => p.VehicleMake, f => f.Vehicle.Manufacturer())
        .RuleFor(p => p.VehicleModel, f => f.Vehicle.Model())
        .RuleFor(p => p.VehicleColor, f => f.Commerce.Color())
        .RuleFor(p => p.VehiclePurchaseDate, f => f.Date.Past())
        .RuleFor(p => p.VehiclePrice, f => (double)f.Finance.Amount(10000, 50000))
        .RuleFor(p => p.CreditCardNumber, f => f.Finance.CreditCardNumber())
        .RuleFor(p => p.CreditCardType, f => f.Random.AlphaNumeric(2))
        .RuleFor(p => p.CreditCardExpiryDate, f => f.Date.Future())
        .RuleFor(p => p.CreditCardCVV, f => f.Random.Replace("###"))
        .RuleFor(p => p.UserName, f => f.Internet.UserName())
        .RuleFor(p => p.PasswordHash, f => f.Internet.Password())
        .RuleFor(p => p.PasswordLastChanged, f => f.Date.Recent())
        .RuleFor(p => p.TwoFactorEnabled, f => f.Random.Bool())
        .RuleFor(p => p.SecurityQuestion, f => f.Lorem.Sentence())
        .RuleFor(p => p.SecurityAnswerHash, f => f.Lorem.Word())
        .RuleFor(p => p.UserPreferencesJson, f => f.Lorem.Paragraph())
        .RuleFor(p => p.AccountCreatedDate, f => f.Date.Past())
        .RuleFor(p => p.AccountLastUpdatedDate, f => f.Date.Recent())
        .RuleFor(p => p.IsAccountLocked, f => f.Random.Bool());

            var products = bogus.Generate(recordsToInsert);

            return products;
        }
    }
}
