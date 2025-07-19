using System.ComponentModel.DataAnnotations;

namespace Ioc.ObjModels.Model.CommonModel
{
    public class AddressModel
    {
        [Required]
        [StringLength(100)]
        public string? Street { get; set; }

        [Required]
        [StringLength(50)]
        public string? City { get; set; }

        [Required]
        [StringLength(50)]
        public string? State { get; set; }

        [Required]
        [StringLength(20)]
        [DataType(DataType.PostalCode)]
        public string? ZipCode { get; set; }

        [StringLength(50)]
        public string? Country { get; set; }

        public AddressModel() { }

        public AddressModel(string street, string city, string state, string zipCode, string country = "USA")
        {
            Street = street;
            City = city;
            State = state;
            ZipCode = zipCode;
            Country = country;
        }

        public override string? ToString()
        {
            return $"{Street}, {City}, {State}, {ZipCode}, {Country}";
        }
    }
}
