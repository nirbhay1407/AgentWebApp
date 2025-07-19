using Ioc.Data.Data;

namespace Ioc.Data.JwnToken
{
    public class AuthenticateService : IAuthenticateService
    {
        private readonly IAccessTokenService _accessTokenService;
        private readonly IRefreshTokenService _refreshTokenService;
        private readonly IocDbContext _context;
        public AuthenticateService(IAccessTokenService accessTokenService, IRefreshTokenService refreshTokenService, IocDbContext context)
        {
            _accessTokenService = accessTokenService;
            _refreshTokenService = refreshTokenService;
            _context = context;
        }

        /* public async Task<AuthenticateResponse> Authenticate(User user, CancellationToken cancellationToken)
         {
             var refreshToken = _refreshTokenService.Generate(user);
             await _context.RefreshToken.AddAsync(new RefreshToken(user.ID, refreshToken), cancellationToken);
             await _context.SaveChangesAsync(cancellationToken);
             return new AuthenticateResponse
             {
                 AccessToken = _accessTokenService.Generate(user),
                 RefreshToken = refreshToken
             };
         }*/

    }
}
