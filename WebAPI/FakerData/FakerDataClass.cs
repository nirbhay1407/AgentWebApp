using Bogus;
using Bogus.DataSets;
using Ioc.Core.DbModel.Models;
using Ioc.ObjModels.Model.CommonModel;

namespace WebAPI.FakerData
{
    public static class FakerDataClass
    {
        // Generic method for generating fake data
        public static List<T> GenerateFakeData<T>(int count, Action<Faker<T>> configureFaker) where T : class
        {
            var faker = new Faker<T>();
            configureFaker(faker);
            return faker.Generate(count);
        }

        #region methods
        public static List<AddressModel> GetFakeDataAddress(int recordsToInsert)
        {
            return GenerateFakeData<AddressModel>(recordsToInsert, faker =>
            {
                faker.RuleFor(a => a.Street, f => f.Address.StreetAddress())
                     .RuleFor(a => a.City, f => f.Address.City())
                      .RuleFor(a => a.State, f => f.Address.State())
                      .RuleFor(a => a.ZipCode, f => f.Address.ZipCode())
                      .RuleFor(a => a.Country, f => f.Random.Bool(0.9f) ? "IND" : f.Address.Country());
            });
        }

        public static List<BankDetailsModel> GetFakeDataBankDetails(int recordsToInsert)
        {
            return GenerateFakeData<BankDetailsModel>(recordsToInsert, faker =>
            {
                faker.RuleFor(b => b.BankName, f => f.Company.CompanyName())
                     .RuleFor(b => b.AccountHolderName, f => f.Name.FullName())
                     .RuleFor(b => b.AccountNumber, f => f.Finance.Account())
                     .RuleFor(b => b.RoutingNumber, f => f.Random.String2(9, "0123456789"))
                     .RuleFor(b => b.AccountType, f => AccountType.Savings)
                     .RuleFor(b => b.BranchName, f => f.Address.StreetName())
                     .RuleFor(b => b.BranchAddress, f => f.Address.FullAddress())
                     .RuleFor(b => b.SwiftCode, f => f.Random.String2(11, "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789"));
            });
        }




        public static List<User> GetFakeUser(int recordsToInsert)
        {
            return GenerateFakeData<User>(recordsToInsert, faker =>
            {
                faker.RuleFor(u => u.UserName, f => f.Internet.UserName())
                     .RuleFor(u => u.Email, (f, u) => f.Internet.Email(u.UserName))
                     .RuleFor(u => u.Password, f => f.Internet.Password())
                     .RuleFor(u => u.CreatedAt, f => f.Date.Past())
                     .RuleFor(u => u.ModifiedDate, f => f.Date.Recent())
                     .RuleFor(u => u.UserProfile, f => new UserProfile
                     {
                         FirstName = f.Person.FirstName,
                         LastName = f.Person.LastName,
                         Address = f.Address.FullAddress(),
                         CreatedAt = DateTime.UtcNow,
                         ModifiedDate = DateTime.UtcNow
                     });
            });
        }

        public static List<RandomGenHelp> GetFakeRandomGenHelp(RandomType type, int recordsToInsert)
        {
            var RandomGenHelpFakerFaker = new Faker<RandomGenHelp>();
            RandomGenHelpFakerFaker.RuleFor(u => u.Type, (int)type);

            switch (type)
            {
                case RandomType.Name:
                    RandomGenHelpFakerFaker.RuleFor(u => u.RandomValue, f => f.Person.FullName);
                    break;
                case RandomType.Username:
                    RandomGenHelpFakerFaker.RuleFor(u => u.RandomValue, f => f.Internet.UserName());
                    break;
                case RandomType.CompanyName:
                    RandomGenHelpFakerFaker.RuleFor(u => u.RandomValue, f => f.Company.CompanyName());
                    break;
                case RandomType.ProductName:
                    RandomGenHelpFakerFaker.RuleFor(u => u.RandomValue, f => f.Commerce.ProductName());
                    break;
                case RandomType.InVoiceNo:
                    RandomGenHelpFakerFaker.RuleFor(u => u.RandomValue, f => f.Commerce.Random.Number(111111, 999999).ToString());
                    break;
                case RandomType.ContactNo:
                    RandomGenHelpFakerFaker.RuleFor(u => u.RandomValue, f => f.Phone.PhoneNumber());

                    break;
                case RandomType.City:
                    RandomGenHelpFakerFaker.RuleFor(u => u.RandomValue, f => f.Address.City());
                    break;
                case RandomType.Street:
                    RandomGenHelpFakerFaker.RuleFor(u => u.RandomValue, f => f.Address.StreetAddress());
                    break;
                case RandomType.FullAddress:
                    RandomGenHelpFakerFaker.RuleFor(u => u.RandomValue, f => f.Address.FullAddress());
                    break;
                case RandomType.RandomString:
                    RandomGenHelpFakerFaker.RuleFor(u => u.RandomValue, f => f.Random.String2(11, "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789"));
                    break;
                case RandomType.RandomNumber:
                    RandomGenHelpFakerFaker.RuleFor(u => u.RandomValue, f => f.Random.String2(9, "0123456789"));
                    break;
                case RandomType.RandomParagraph:
                    RandomGenHelpFakerFaker.RuleFor(u => u.RandomValue, f => f.Lorem.Paragraph());
                    break;
                case RandomType.Password:
                    RandomGenHelpFakerFaker.RuleFor(u => u.RandomValue, f => f.Internet.Password());
                    break;
                default:
                    RandomGenHelpFakerFaker.RuleFor(u => u.RandomValue, f => f.Person.FullName);
                    break;
            }


            // Generate a fake User
            return RandomGenHelpFakerFaker.Generate(recordsToInsert);

        }

        #endregion
    
    
    
    }
}
