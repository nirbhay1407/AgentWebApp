namespace Ioc.Core.DbModel
{
    public class Setting : PublicBaseEntity
    {
        public string SettingType { get; set; } = string.Empty; // e.g., "SiteName", "AdminEmail"
        public string DisplayName { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
    }
}
