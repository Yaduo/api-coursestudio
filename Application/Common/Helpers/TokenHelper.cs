using System;
using System.Text;
using System.Linq;
using System.Security.Claims;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using CourseStudio.Lib.Exceptions;

namespace CourseStudio.Application.Common.Helpers
{
	public static class TokenHelper
    {
        public static TokenValidationParameters GenerateTokenValidation(string key, string issuer, string audience)
        {
            return new TokenValidationParameters()
            {
                ValidateIssuer = true,
                ValidIssuer = issuer,
                ValidateAudience = true,
                ValidAudience = audience,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key)),
                RequireExpirationTime = true,
				ValidateLifetime = true,
                ClockSkew = TimeSpan.Zero
            };
        }

        public static JwtSecurityToken GenerateToken(string userName, string key, string issuer, string audience, int expiresInMins, IList<Claim> UserClaims, IList<Claim> UserRoles)
        {
            var claims = new[]
            {
                // default claims
                new Claim(JwtRegisteredClaimNames.Sub, userName),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            }
            // get user claims, including RoleClaims
            .Union(UserClaims)
            // get user Roles
            .Union(UserRoles);

            // get credentials
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));
            var creds = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            return new JwtSecurityToken(
                issuer,
                audience,
                claims,
				expires: DateTime.UtcNow.AddMinutes(expiresInMins),
                signingCredentials: creds
            );
        }
    
        public static JwtSecurityToken ParseJwtStr(string jwtStr) 
        {
            var jwtHandler = new JwtSecurityTokenHandler();
            if (!jwtHandler.CanReadToken(jwtStr))
            {
                throw new IdentityException("JWT canno read");
            }
            return jwtHandler.ReadJwtToken(jwtStr);
        }

        public static T GetJwtTokenPayload<T>(string jwtStr)
        {
            var jwtToken = ParseJwtStr(jwtStr);
            //Extract the payload of the JWT
            var claims = jwtToken.Claims;
            var jwtPayload = "{";
            foreach (Claim c in claims)
            {
                jwtPayload += '"' + c.Type + "\":\"" + c.Value + "\",";
            }
            jwtPayload += "}";
            var jsonCompactSerializedString = JToken.Parse(jwtPayload).ToString(Formatting.Indented);
            return JsonConvert.DeserializeObject<T>(jsonCompactSerializedString);
        }
    }
}
