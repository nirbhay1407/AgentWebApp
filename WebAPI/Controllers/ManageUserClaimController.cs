using Ioc.Core;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ManageUserClaimController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IConfiguration _configuration;
        private readonly ILogger<ManageUserClaimController> _logger;

        public ManageUserClaimController(
            UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> roleManager,
            IConfiguration configuration, ILogger<ManageUserClaimController> logger)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _configuration = configuration;
            _logger = logger;
        }


        [HttpGet]
        public IActionResult Index()
        {
            // Get the current user
            var user = HttpContext.User;

            // Retrieve all claims for the user
            var userClaims = user.Claims;

            // Process the claims
            foreach (var claim in userClaims)
            {
                var claimType = claim.Type;
                var claimValue = claim.Value;

                // Do something with the claim data
                // ...
            }

            // Return the user claims or perform further operations
            // ...

            return Ok(userClaims);
        }
    }
}
