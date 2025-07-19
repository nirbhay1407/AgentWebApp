namespace Ioc.Core.DbModel
{
    public class Setting : PublicBaseEntity
    {
        public string SettingType { get; set; }
        public string DisplayName { get; set; }
        public string Description { get; set; }
    }
}
