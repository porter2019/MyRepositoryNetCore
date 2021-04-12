using JWT;
using JWT.Algorithms;
using JWT.Exceptions;
using JWT.Serializers;
using System;

namespace MyNetCore.Common.Helper
{
    public class JWTHelper
    {
        private static readonly string secret = AppSettings.Get("Jwt:SecurityKey");

        /// <summary>
		/// 返回token
		/// </summary>
		/// <param name="payload"></param>
		/// <returns></returns>
		public static string GetToken<T>(T payload)
        {
            IJwtAlgorithm algorithm = new HMACSHA256Algorithm();
            IJsonSerializer serializer = new JsonNetSerializer();
            IBase64UrlEncoder urlEncoder = new JwtBase64UrlEncoder();
            IJwtEncoder encoder = new JwtEncoder(algorithm, serializer, urlEncoder);
            var token = encoder.Encode(payload, secret);

            return token;
        }

        /// <summary>
        /// 根据jwtToken获取payload
        /// </summary>
        /// <param name="token">jwtToken</param>
        /// <returns></returns>
        public static T ParseToken<T>(string token)
        {
            try
            {
                IJsonSerializer serializer = new JsonNetSerializer();
                var provider = new UtcDateTimeProvider();
                IJwtValidator validator = new JwtValidator(serializer, provider);
                IBase64UrlEncoder urlEncoder = new JwtBase64UrlEncoder();
                IJwtAlgorithm algorithm = new HMACSHA256Algorithm(); // symmetric
                IJwtDecoder decoder = new JwtDecoder(serializer, validator, urlEncoder, algorithm);

                return decoder.DecodeToObject<T>(token, secret, verify: true);
            }
            catch (TokenExpiredException ex)
            {
                //Token过期了
            }
            catch (Exception ex)
            {
                
            }
            return default(T);
        }

    }
}
