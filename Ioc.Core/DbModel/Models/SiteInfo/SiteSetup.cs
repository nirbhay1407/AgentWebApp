using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ioc.Core.DbModel.Models.SiteInfo
{
    public class SiteSetup : PublicBaseEntity 
    {
        public string? SiteName { get; set; }
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
