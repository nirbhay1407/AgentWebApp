using Ioc.Core;
using Ioc.Core.DbModel.JwtTokenC;
using Ioc.ObjModels.Model.CommonModel;
using Ioc.ObjModels.Model.JwtAuthModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticateController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IConfiguration _configuration;
        private readonly ILogger<LoginModel> _logger;

        public AuthenticateController(
            UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> roleManager,
            IConfiguration configuration, ILogger<LoginModel> logger)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _configuration = configuration;
            _logger = logger;
        }

        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> Login([FromBody] LoginModel model)
        {
            _logger.LogInformation("About page visited at {DT}",
            DateTime.UtcNow.ToLongTimeString());
            _logger.LogError("Login-------------------------------------------");
            var user = await _userManager.FindByNameAsync(model.Username);
            if (user != null && await _userManager.CheckPasswordAsync(user, model.Password))
            {
                var userRoles = await _userManager.GetRolesAsync(user);

                var authClaims = new List<Claim>
                {
                    new Claim(ClaimTypes.Sid, user.Id),
                    new Claim(ClaimTypes.Name, user.UserName),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                };

                foreach (var userRole in userRoles)
                {
                    authClaims.Add(new Claim(ClaimTypes.Role, userRole));
                }
                authClaims.Add(new Claim(ClaimTypes.Email, user.Email));

                var claimLists = _userManager.GetClaimsAsync(user).Result;

                foreach (var claimList in claimLists)
                {
                    authClaims.Add(new Claim(claimList.Type, claimList.Value));
                }
                //var claimList1 = claimList.Select(p => p.Type);
                /*if (!claimList1.Contains("IsAdmin"))
                {
                    await _userManager.AddClaimAsync(user2, new Claim("IsAdmin", "false"));
                }
                if (!claimList1.Contains("DateOfJoining"))
                {
                    await UserManager.AddClaimAsync(user2, new Claim("DateOfJoining", "09/01/2018"));
                }*/
                /* if (!claimList1.Contains("IsHR"))
                 {
                     await _userManager.AddClaimAsync(user, new Claim("IsHR", claimList!.Where(x=>x.Type== "IsHR").Select(x=>x.Value).FirstOrDefault().ToString()));
                 }*/



                var token = CreateToken(authClaims);
                var refreshToken = GenerateRefreshToken();

                _ = int.TryParse(_configuration["JWT:RefreshTokenValidityInDays"], out int refreshTokenValidityInDays);

                user.RefreshToken = refreshToken;
                user.RefreshTokenExpiryTime = DateTime.Now.AddDays(refreshTokenValidityInDays);

                await _userManager.UpdateAsync(user);

                return Ok(new
                {
                    Token = new JwtSecurityTokenHandler().WriteToken(token),
                    RefreshToken = refreshToken,
                    Role = userRoles,
                    Id = user.Id,
                    Expiration = token.ValidTo,
                    Claims = _userManager.GetClaimsAsync(user).Result
                });
            }
            return Unauthorized();
        }

        [HttpPost]
        [Route("register")]
        public async Task<IActionResult> Register([FromBody] RegisterModel model)
        {
            if (!_roleManager.Roles.Any())
            {
                await CreateRolesandUsers();
            }
            var roleDetails = _roleManager.FindByNameAsync(model.RoleName).Result;
            if (roleDetails == null && roleDetails.Name == null)
                return StatusCode(StatusCodes.Status404NotFound, new ResponseModel { Status = 404, Message = "Role Is not Available!" });
            var userExists = await _userManager.FindByNameAsync(model.Username);
            if (userExists != null)
                return StatusCode(StatusCodes.Status500InternalServerError, new ResponseModel { Status = 404, Message = "User already exists!" });

            ApplicationUser user = new()
            {
                Email = model.Email,
                SecurityStamp = Guid.NewGuid().ToString(),
                UserName = model.Username
            };
            var result = await _userManager.CreateAsync(user, model.Password);
            await _userManager.AddToRoleAsync(user, roleDetails.Name);
            if (!result.Succeeded)
                return StatusCode(StatusCodes.Status500InternalServerError, new ResponseModel { Status = 404, Message = "User creation failed! Please check user details and try again." });

            return Ok(new ResponseModel { Status = 200, Message = "User created successfully!" });
        }

        [HttpPost]
        [Route("register-admin")]
        public async Task<IActionResult> RegisterAdmin([FromBody] RegisterModel model)
        {
            var userExists = await _userManager.FindByNameAsync(model.Username);
            if (userExists != null)
                return StatusCode(StatusCodes.Status500InternalServerError, new ResponseModel { Status = 404, Message = "User already exists!" });

            ApplicationUser user = new()
            {
                Email = model.Email,
                SecurityStamp = Guid.NewGuid().ToString(),
                UserName = model.Username
            };
            var result = await _userManager.CreateAsync(user, model.Password);
            if (!result.Succeeded)
                return StatusCode(StatusCodes.Status500InternalServerError, new ResponseModel { Status = 404, Message = "User creation failed! Please check user details and try again." });

            if (!await _roleManager.RoleExistsAsync(UserRoles.Admin))
                await _roleManager.CreateAsync(new IdentityRole(UserRoles.Admin));
            if (!await _roleManager.RoleExistsAsync(UserRoles.User))
                await _roleManager.CreateAsync(new IdentityRole(UserRoles.User));

            if (await _roleManager.RoleExistsAsync(UserRoles.Admin))
            {
                await _userManager.AddToRoleAsync(user, UserRoles.Admin);
            }
            if (await _roleManager.RoleExistsAsync(UserRoles.Admin))
            {
                await _userManager.AddToRoleAsync(user, UserRoles.User);
            }
            return Ok(new ResponseModel { Status = 200, Message = "User created successfully!" });
        }

        [HttpPost]
        [Route("refresh-token")]
        public async Task<IActionResult> RefreshToken(TokenModel tokenModel)
        {
            if (tokenModel is null)
            {
                return BadRequest("Invalid client request");
            }

            string? accessToken = tokenModel.AccessToken;
            string? refreshToken = tokenModel.RefreshToken;

            var principal = GetPrincipalFromExpiredToken(accessToken);
            if (principal == null)
            {
                return BadRequest("Invalid access token or refresh token");
            }

#pragma warning disable CS8600 // Converting null literal or possible null value to non-nullable type.
#pragma warning disable CS8602 // Dereference of a possibly null reference.
            string username = principal.Identity.Name;
#pragma warning restore CS8602 // Dereference of a possibly null reference.
#pragma warning restore CS8600 // Converting null literal or possible null value to non-nullable type.

            var user = await _userManager.FindByNameAsync(username);

            if (user == null || user.RefreshToken != refreshToken || user.RefreshTokenExpiryTime <= DateTime.Now)
            {
                return BadRequest("Invalid access token or refresh token");
            }

            var newAccessToken = CreateToken(principal.Claims.ToList());
            var newRefreshToken = GenerateRefreshToken();

            user.RefreshToken = newRefreshToken;
            await _userManager.UpdateAsync(user);

            return new ObjectResult(new
            {
                accessToken = new JwtSecurityTokenHandler().WriteToken(newAccessToken),
                refreshToken = newRefreshToken
            });
        }

        [Authorize]
        [HttpPost]
        [Route("revoke/{username}")]
        public async Task<IActionResult> Revoke(string username)
        {
            var user = await _userManager.FindByNameAsync(username);
            if (user == null) return BadRequest("Invalid user name");

            user.RefreshToken = null;
            await _userManager.UpdateAsync(user);

            return NoContent();
        }

        [Authorize]
        [HttpPost]
        [Route("revoke-all")]
        public async Task<IActionResult> RevokeAll()
        {
            var users = _userManager.Users.ToList();
            foreach (var user in users)
            {
                user.RefreshToken = null;
                await _userManager.UpdateAsync(user);
            }

            return NoContent();
        }

        [Authorize]
        [HttpPut]
        [Route("role-assign")]
        public async Task<IActionResult> UpdateRoleAssign(string userId, String role)
        {
            var users = await _userManager.FindByIdAsync(userId);

            if (users != null && await _roleManager.RoleExistsAsync(role))
                await _userManager.AddToRoleAsync(users, role);
            else
                return BadRequest();

            return Ok(new ResponseModel { Status = 200, Message = "Role Assigned", Data = _userManager.FindByIdAsync(userId) });

        }

        [Authorize]
        [HttpGet]
        [Route("user-by-token")]
        public async Task<IActionResult> GetUserByToken()
        {
            ClaimsPrincipal currentUser = User;
            var abc = currentUser?.Claims?.FirstOrDefault(x => x.Type == ClaimTypes.Sid)?.Value;
            var abcd = User?.FindFirst(ClaimTypes.Sid)?.Value;
            var user = await _userManager.FindByIdAsync(abc);
            return Ok(new { user = user, role = await _userManager.GetRolesAsync(user) });
            //return new ApplicationUser();
        }


        [HttpGet]
        [Route("all-role")]
        public async Task<IActionResult> GetAllRole()
        {
            var role = _roleManager.Roles.ToList();
            var abc = _userManager.Options.User;
            await Task.Delay(0);
            //return Ok(new { Users = users, Role = _userManager.GetUsersInRoleAsync(users.First()) });
            return Ok(role);
        }

        [Authorize]
        [HttpGet]
        [Route("all-aspnetuser")]
        public async Task<IActionResult> GetAllAspNetUser()
        {

            var users = _userManager.Users.ToList();
            foreach (var obj in users)
            {
                obj.IdentityRole = string.Join(",", _userManager.GetRolesAsync(obj).Result);
                obj.UserClaim = string.Join(",", _userManager.GetClaimsAsync(obj).Result);
            }
            var abc = _userManager.Options.User;
            await Task.Delay(0);
            //return Ok(new { Users = users, Role = _userManager.GetUsersInRoleAsync(users.First()) });
            return Ok(users);
        }

        [Authorize]
        [HttpPost]
        [Route("add-user-claim")]
        public async Task<IActionResult> AddUserClaim(string id, String claimname)
        {
            var user = await _userManager.FindByIdAsync(id);

            if (user != null)
            {
                // Add the claim to the user
                var claim = new Claim(claimname, "true", "bool");
                var result = await _userManager.AddClaimAsync(user, claim);

                if (result.Succeeded)
                {
                    // Claim added successfully
                    // ...
                    return Ok(user);
                }
                else
                {
                    // Failed to add claim
                    // ...
                    return NotFound();
                }
            }
            else
            {
                // User not found
                // ...
                return NotFound();
            }

        }

        private JwtSecurityToken CreateToken(List<Claim> authClaims)
        {
            var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Secret"]));
            _ = int.TryParse(_configuration["JWT:TokenValidityInMinutes"], out int tokenValidityInMinutes);

            var token = new JwtSecurityToken(
                issuer: _configuration["JWT:ValidIssuer"],
                audience: _configuration["JWT:ValidAudience"],
                expires: DateTime.Now.AddMinutes(tokenValidityInMinutes),
                claims: authClaims,
                signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
                );

            return token;
        }

        private static string GenerateRefreshToken()
        {
            var randomNumber = new byte[64];
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(randomNumber);
            return Convert.ToBase64String(randomNumber);
        }

        private ClaimsPrincipal? GetPrincipalFromExpiredToken(string? token)
        {
            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateAudience = false,
                ValidateIssuer = false,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Secret"])),
                ValidateLifetime = false
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out SecurityToken securityToken);
            if (securityToken is not JwtSecurityToken jwtSecurityToken || !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
                throw new SecurityTokenException("Invalid token");

            return principal;

        }

        private async Task CreateRolesandUsers()
        {
            bool x = await _roleManager.RoleExistsAsync("SuperAdmin");
            if (!x)
            {
                // first we create Admin rool    
                var role = new IdentityRole();
                role.Name = "SuperAdmin";
                await _roleManager.CreateAsync(role);

                //Here we create a Admin super user who will maintain the website                   

                var user = CreateUser();
                user.UserName = "nirbhayk";
                user.Email = "nirbhay@chetu.com";
                user.EmailConfirmed = true;
                string userPWD = "Nirbhay@123";

                IdentityResult chkUser = await _userManager.CreateAsync(user, userPWD);

                //Add default User to Role Admin    
                if (chkUser.Succeeded)
                {
                    var result1 = await _userManager.AddToRoleAsync(user, "SuperAdmin");
                }
            }

            // creating Creating Manager role     
            x = await _roleManager.RoleExistsAsync("Admin");
            if (!x)
            {
                var role = new IdentityRole();
                role.Name = "Admin";
                await _roleManager.CreateAsync(role);
            }

            // creating Creating Manager role     
            x = await _roleManager.RoleExistsAsync("Manager");
            if (!x)
            {
                var role = new IdentityRole();
                role.Name = "Manager";
                await _roleManager.CreateAsync(role);
            }

            // creating Creating Employee role     
            x = await _roleManager.RoleExistsAsync("User");
            if (!x)
            {
                var role = new IdentityRole();
                role.Name = "User";
                await _roleManager.CreateAsync(role);
            }
        }

        private ApplicationUser CreateUser()
        {
            try
            {
                return Activator.CreateInstance<ApplicationUser>();
            }
            catch
            {
                throw new InvalidOperationException($"Can't create an instance of '{nameof(ApplicationUser)}'. " +
                    $"Ensure that '{nameof(ApplicationUser)}' is not an abstract class and has a parameterless constructor, or alternatively " +
                    $"override the register page in /Areas/Identity/Pages/Account/Register.cshtml");
            }
        }
    }
}
