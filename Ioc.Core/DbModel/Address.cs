namespace Ioc.Core.DbModel
{
    public class Address : PublicBaseEntity
    {
        public string? Street { get; set; }

        public string? City { get; set; }

        public string? State { get; set; }

        public string? ZipCode { get; set; }

        public string? Country { get; set; }

    }
}
