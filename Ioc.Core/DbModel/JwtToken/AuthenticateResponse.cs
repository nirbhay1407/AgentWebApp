namespace Ioc.Core.DbModel.JwtToken
{
    public class AuthenticateResponse
    {
        public string? AccessToken { get; set; }
        public string? RefreshToken { get; set; }
    }
}
