﻿using System.IdentityModel.Tokens.Jwt;
using System.Text;
using System.Text.Json;
using Amazon.KeyManagementService;
using Amazon.KeyManagementService.Model;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Serilog;
using ServiceName.Core.Common.Interfaces;
using ServiceName.Core.Common.Security;
using ServiceName.Core.Model;
using ServiceName.Infrastructure.Authentication.JWT;

namespace ServiceName.Infrastructure.Authentication
{
    /// <summary>
    ///
    /// aws --endpoint-url=http://localhost:52002 kms --region ap-southeast-2 create-key --key-spec RSA_2048 --key-usage SIGN_VERIFY
    /// aws --endpoint-url=http://localhost:52002 kms --region ap-southeast-2 list-keys
    /// aws --endpoint-url=http://localhost:52002 kms --region ap-southeast-2 get-public-key --key-id 6732c7ca-6ec9-4b96-9711-fd1c7d637c8e
    /// </summary>
    public class AssymetricKmsJwtService : IJwtAuthenticationService
    {
        private readonly IConfiguration _configuration;
        private readonly IAmazonKeyManagementService _amazonKms;
        private readonly ILogger _logger;

        public AssymetricKmsJwtService(IConfiguration configuration, IAmazonKeyManagementService amazonKms, ILogger logger)
        {
            _configuration = configuration;
            _amazonKms = amazonKms;
            _logger = logger;
        }

        public async Task<string> GenerateTokenAsync(ModuleIdentity identity, int lifetimeSeconds, string issuer, string audience)
        {
            var signingKeyId = _configuration["ModuleConfiguration:Infrastructure:Kms:SigningKeyId"];

            if (string.IsNullOrWhiteSpace(signingKeyId))
            {
                throw new InvalidOperationException("The amazonKmsSigningKeyId is not defined.");
            }

            var header = Base64UrlEncoder.Encode(JsonSerializer.Serialize(new CustomJwtHeader()
            {
                Algorithm = "RS256",
                KeyId = signingKeyId
            }));

            var time = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
            var payload = Base64UrlEncoder.Encode(JsonSerializer.Serialize(new CustomJwtPayload()
            {
                IssuedAt = time,
                ExpirationTime = time + lifetimeSeconds,
                Issuer = issuer,
                Audience = audience,
                Subject = identity.UserGuid,
                TenantId = identity.InstanceGuid,
                Username = identity.UserName
            }));

            string signature;

            using (var ms = new MemoryStream(Encoding.UTF8.GetBytes($"{header}.{payload}")))
            {
                var request = new SignRequest
                {
                    KeyId = signingKeyId,
                    SigningAlgorithm = SigningAlgorithmSpec.RSASSA_PKCS1_V1_5_SHA_256,
                    MessageType = MessageType.RAW,
                    Message = ms
                };

                var signResult = await _amazonKms.SignAsync(request);
                signature = Base64UrlEncoder.Encode(signResult.Signature.ToArray());
            }

            return $"{header}.{payload}.{signature}";
        }

        public async Task<bool> ValidateTokenAsync(string token)
        {
            RsaSecurityKey rsaSecurityKey = SecurityHelper.GetRsaSecurityKey(_configuration["ModuleConfiguration:Infrastructure:Kms:PublicKey"]);

            var issuer = _configuration["ModuleConfiguration:Jwt:Issuer"];
            var audience = _configuration["ModuleConfiguration:Jwt:Audience"];
            var tokenHandler = new JwtSecurityTokenHandler();
            try
            {
                tokenHandler.ValidateToken(token, new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidIssuer = issuer,
                    ValidAudience = audience,
                    IssuerSigningKey = rsaSecurityKey
                }, out SecurityToken validatedToken);
            }
            catch
            {
                return await Task.FromResult(false);
            }
            return await Task.FromResult(true);
        }
    }
}