﻿using Ioc.Core.DbModel.Models;
using Ioc.Core.Identitymodel;

namespace Ioc.Data.JwnToken
{
    public class RefreshTokenService : IRefreshTokenService
    {
        private readonly ITokenGenerator _tokenGenerator;
        private readonly JwtSettings _jwtSettings;

        public RefreshTokenService(ITokenGenerator tokenGenerator, JwtSettings jwtSettings) =>
            (_tokenGenerator, _jwtSettings) = (tokenGenerator, jwtSettings);

        public string Generate(User user) => _tokenGenerator.Generate(_jwtSettings.RefreshTokenSecret,
            _jwtSettings.Issuer, _jwtSettings.Audience,
            _jwtSettings.RefreshTokenExpirationMinutes, null);
    }
}
