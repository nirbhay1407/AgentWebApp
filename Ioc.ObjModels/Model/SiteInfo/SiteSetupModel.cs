using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ioc.ObjModels.Model.SiteInfo
{
    public class SiteSetupModel : PublicBaseModel
    {
        public string? SiteName { get; set; }
        public string? SiteCode { get; set; }
        public string? SiteUrl { get; set; }
        public string? SiteLogo { get; set; }
        public string? SiteFavicon { get; set; }
        public string? SiteDescription { get; set; }
        public string? SiteKeywords { get; set; }
        public string? SiteEmail { get; set; }
        public string? SitePhone { get; set; }
        public string? SiteAddress { get; set; }
    }
}
