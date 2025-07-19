namespace Ioc.ObjModels.Model
{
    public class UserProfileModel : PublicBaseModel
    {
        public string? FirstName { get; set; } = string.Empty;
        public string? LastName { get; set; } = string.Empty;
        public string? Address { get; set; } = string.Empty;
    }
}
