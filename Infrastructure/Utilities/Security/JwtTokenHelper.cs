using System;
using System.Collections.Generic;
using System.Linq;
using JWT;
using JWT.Algorithms;
using JWT.Serializers;

namespace CourseStudio.Lib.Utilities.Security
{
	public static class JwtTokenHelper
	{
		static readonly string _expiresStr = "exp";

		public static string GenerateToken(IList<object> payloads, DateTimeOffset? expiredDateTime, string secretKey)
        {
			Dictionary<string, object> claims = payloads.Select((p, i) => new { Item = p, Index = i }).ToDictionary(c => "claim" + c.Index, c => c.Item);
			if (expiredDateTime != null)
			{
				claims.Add(_expiresStr, expiredDateTime.Value.ToUnixTimeSeconds().ToString());
			}

            IJwtAlgorithm algorithm = new HMACSHA256Algorithm();
            IJsonSerializer serializer = new JsonNetSerializer();
            IBase64UrlEncoder urlEncoder = new JwtBase64UrlEncoder();
            IJwtEncoder encoder = new JwtEncoder(algorithm, serializer, urlEncoder);

            return encoder.Encode(claims, secretKey);
        }

		public static bool IsValid(string token, IList<object> payloads, string secretKey) 
		{
			IJsonSerializer serializer = new JsonNetSerializer();
            IDateTimeProvider provider = new UtcDateTimeProvider();
            IJwtValidator validator = new JwtValidator(serializer, provider);
            IBase64UrlEncoder urlEncoder = new JwtBase64UrlEncoder();
            IJwtDecoder decoder = new JwtDecoder(serializer, validator, urlEncoder);
            
			var claims = decoder.DecodeToObject<IDictionary<string, object>>(token, secretKey, verify: true);
            var tokenPayloads = claims.Where(c => !c.Key.Equals(_expiresStr)).Select(c => c.Value);
            if (tokenPayloads.Intersect(payloads).Count() != tokenPayloads.Count())
            {
                return false;
            }
            return true;
   
		}
	}
}