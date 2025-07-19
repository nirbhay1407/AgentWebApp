using Ioc.Core;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Admin")]
    public class LoginController : ControllerBase
    {
        public SignInManager<ApplicationUser> _signInManager;
        public UserManager<ApplicationUser> _userManager;
        public LoginController(SignInManager<ApplicationUser> signInManager, UserManager<ApplicationUser> userManager)
        {
            _signInManager = signInManager;
            _userManager = userManager;
        }

        /*[HttpPost]
        [SwaggerOperation(Description = "Signs in provided user and return token",
    Summary = "Sign In",
    OperationId = "Auth.Login",
    Tags = new[] { "Auth" })]
        [SwaggerResponse(200, "User logged in successfully", typeof(string))]
        [SwaggerResponse(400, "User with provided email can not be found", typeof(string))]
        [Produces("application/json")]
        [Consumes("application/json")]
        public override async Task<ActionResult<string>> HandleAsync(
    [SwaggerRequestBody("User login payload", Required = true)] LoginUserRequest loginUserRequest,
    CancellationToken cancellationToken = new())
        {
            return Ok(await _mediator.Send(new LoginUserCommand(loginUserRequest), cancellationToken));
        }*/


        [HttpGet]
        public async Task<IActionResult?> Get()
        {
            ClaimsPrincipal currentUser = User;
            var abc = currentUser.FindFirst(ClaimTypes.Name)?.Value;
            return Ok(new { user = _userManager.FindByNameAsync(abc).Result });
        }

        [HttpGet("getCustom")]
        public async Task<ApplicationUser?> GetCus()
        {
            var userID = HttpContext?.User?.Identity?.Name;
            return await _userManager.FindByNameAsync(userID);
        }
    }
}
