﻿using Ioc.Core.DbModel.Models;
using Ioc.Core.Identitymodel;
using System.Security.Claims;

namespace Ioc.Data.JwnToken
{
    public class AccessTokenService : IAccessTokenService
    {
        private readonly ITokenGenerator _tokenGenerator;
        private readonly JwtSettings? _jwtSettings;

        public AccessTokenService(ITokenGenerator tokenGenerator, JwtSettings jwtSettings) =>
            (_tokenGenerator, _jwtSettings) = (tokenGenerator, jwtSettings);

        public string Generate(User user)
        {
            List<Claim> claims = new()
            {
                new Claim("id", user.ID.ToString()),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Name, user.UserName)
            };
            return _tokenGenerator.Generate(_jwtSettings?.AccessTokenSecret, _jwtSettings?.Issuer, _jwtSettings?.Audience,
                _jwtSettings.AccessTokenExpirationMinutes, claims);
        }
    }
}
