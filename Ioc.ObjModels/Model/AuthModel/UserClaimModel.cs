using Ioc.ObjModels.Model;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Ioc.ObjModels.Model.AuthModel
{
    public class UserClaimModel : PublicBaseModel
    {
        public string ClaimName { get; set; } = string.Empty;
        public bool ClaimValue { get; set; }
    }


    public class UserClaimlist
    {
        public List<UserClaimModel> userClaims { get; set; }

        public List<SelectListItem> ClaimsName { get; set; }
    }
}
