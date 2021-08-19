using JWT;
using JWT.Algorithms;
using JWT.Exceptions;
using JWT.Serializers;
using Microsoft.Extensions.Configuration;
using MyNetCore.IServices;
using System;

namespace MyNetCore.Services
{
    /// <summary>
    /// JWT操作服务
    /// </summary>
    [ServiceLifetime()]
    public class JwtService : IJwtService
    {
        private readonly IConfiguration _config;
        private readonly string _securityKey;

        public JwtService(IConfiguration config)
        {
            _config = config;
            _securityKey = _config["Jwt:SecurityKey"];
            if (_securityKey.IsNull()) throw new NullReferenceException("无法从配置文件中获取JWT密钥(Jwt:SecurityKey)");
        }

        /// <summary>
        /// 生成Jwt Token
        /// </summary>
        /// <typeparam name="T">payload载体对象类型</typeparam>
        /// <param name="payload">数据</param>
        /// <returns></returns>
        public string GenerateToken<T>(T payload)
        {
            IJwtAlgorithm algorithm = new HMACSHA256Algorithm();
            IJsonSerializer serializer = new JsonNetSerializer();
            IBase64UrlEncoder urlEncoder = new JwtBase64UrlEncoder();
            IJwtEncoder encoder = new JwtEncoder(algorithm, serializer, urlEncoder);
            var token = encoder.Encode(payload, _securityKey);

            return token;
        }

        /// <summary>
        /// 根据jwtToken获取payload对象
        /// </summary>
        /// <typeparam name="T">对象类型</typeparam>
        /// <param name="token">jwt token字符串</param>
        /// <returns></returns>
        public T ParseToken<T>(string token)
        {
            try
            {
                IJsonSerializer serializer = new JsonNetSerializer();
                var provider = new UtcDateTimeProvider();
                IJwtValidator validator = new JwtValidator(serializer, provider);
                IBase64UrlEncoder urlEncoder = new JwtBase64UrlEncoder();
                IJwtAlgorithm algorithm = new HMACSHA256Algorithm(); // symmetric
                IJwtDecoder decoder = new JwtDecoder(serializer, validator, urlEncoder, algorithm);

                return decoder.DecodeToObject<T>(token, _securityKey, verify: true);
            }
            catch (TokenExpiredException)
            {
                //Token过期了
            }
            catch (Exception)
            {

            }
            return default(T);
        }
    }
}
