using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using ServiceName.Core.Common.Interfaces;
using ServiceName.Core.Model;

namespace ServiceName.Infrastructure.Authentication
{
    /// <summary>
    /// https://dotnetcoretutorials.com/2020/01/15/creating-and-validating-jwt-tokens-in-asp-net-core/
    /// </summary>
    public class SymmetricJwtAuthenticationService : IJwtAuthenticationService
    {
        public Task<string> GenerateTokenAsync(ModuleIdentity identity, int lifetimeSeconds, string issuer, string audience)
        {
            var mySecret = "asdv234235^&%&^%&^hjsdfb2%%%";
            var mySecurityKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(mySecret));
            var myIssuer = "http://mysite.com";
            var myAudience = "http://myaudience.com";
            var tokenHandler = new JwtSecurityTokenHandler();
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.NameIdentifier, identity.UserGuid),
                }),
                Expires = DateTime.UtcNow.AddDays(7),
                Issuer = myIssuer,
                Audience = myAudience,
                SigningCredentials = new SigningCredentials(mySecurityKey, SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return Task.FromResult(tokenHandler.WriteToken(token));
        }
        
        public async Task<bool> ValidateTokenAsync(string token)
        {
            var mySecret = "asdv234234^&%&^%&^hjsdfb2%%%";
            var mySecurityKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(mySecret));
            var myIssuer = "http://mysite.com";
            var myAudience = "http://myaudience.com";
            var tokenHandler = new JwtSecurityTokenHandler();
            try
            {
                tokenHandler.ValidateToken(token, new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidIssuer = myIssuer,
                    ValidAudience = myAudience,
                    IssuerSigningKey = mySecurityKey
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
