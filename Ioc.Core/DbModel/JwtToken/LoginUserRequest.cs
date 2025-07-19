using Swashbuckle.AspNetCore.Annotations;

namespace Ioc.Core.DbModel.JwtToken
{
    public class LoginUserRequest
    {
        [SwaggerSchema(Required = new[] { "The User Email" })]
        public string? Email { get; set; }
        [SwaggerSchema(Required = new[] { "The User Password" })]
        public string? Password { get; set; }
    }
}
