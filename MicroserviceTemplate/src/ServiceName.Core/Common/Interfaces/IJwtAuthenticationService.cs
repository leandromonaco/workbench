using ServiceName.Core.Model;

namespace ServiceName.Core.Common.Interfaces
{
    public interface IJwtAuthenticationService
    {
        Task<string> GenerateTokenAsync(ModuleIdentity identity, int lifetimeSeconds, string issuer, string audience);

        Task<bool> ValidateTokenAsync(string token);
    }
}