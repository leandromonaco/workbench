using Microsoft.Extensions.Configuration;
using ServiceName.Core.Model;
using ServiceName.Infrastructure.Authentication;

namespace ServiceName.Test
{
    public class JwtTests
    {
        AssymetricKmsJwtService _assymetricKmsJwtService;
        ModuleIdentity _moduleIdentity;

        public JwtTests()
        {
            var configMock = new Dictionary<string, string>
                            {
                                    {"ModuleConfiguration:AwsServices:Kms:AccessKey", "test"},
                                    {"ModuleConfiguration:AwsServices:Kms:SecretKey", "test"},
                                    {"ModuleConfiguration:AwsServices:Kms:RegionEndpoint", "eu-west-2"},
                                    {"ModuleConfiguration:AwsServices:Kms:LocalTestEndpoint", "http://localhost:52002"},
                                    {"ModuleConfiguration:Jwt:SigningKeyId", "6732c7ca-6ec9-4b96-9711-fd1c7d637c8e"},
                                    {"ModuleConfiguration:Jwt:PublicKey", "MIIBIjANBgkqhkiG9w0BAQEFAAOCAQ8AMIIBCgKCAQEA56tfR6w3YJpbH5XZ6Ze2kB2evnUpbyZJiTLPKSc/VeA46m09lVB7bJRp0pKX2LusT2pccrVe5AYtbnikKqhOQWUdjLJnSONPNpd4yjEseqPblsXicA+xdP+Fk2W0yDxOc79LUAywgjV8JqNbbtVbhzqVPOLalJYnPEAVa3NQV138dnU7NzxbAjPjXINi7BBZ2OLRuocJRMfe16AUiQtH8MaWfRnnRRwdCBLJCXnZy+0hVc701SrVoTS+CA8RfGTCnzutx9MXW7t4SCEjZH0MSfhSZbKggPfi36HeUdClacgD6L0+FhSBKzd8kOC06CDf5WM9oV/XtWVXEWWGPDHv8wIDAQAB"},
                                    {"ModuleConfiguration:Jwt:Issuer", "Issuer"},
                                    {"ModuleConfiguration:Jwt:Audience", "Audience"},
                            };

            _moduleIdentity = new()
            {
                UserGuid = "UserGuid",
                InstanceGuid = "InstanceGuid",
                UserName = "UserName"
            };

            var configuration = new ConfigurationBuilder()
                        .AddInMemoryCollection(configMock)
                        .Build();

            _assymetricKmsJwtService = new(configuration);
        }
        
        [Fact]
        public async Task JwtValidationOK()
        {
            var token = await _assymetricKmsJwtService.GenerateTokenAsync(_moduleIdentity, 60, "Issuer", "Audience");
            var isValid =  await _assymetricKmsJwtService.ValidateTokenAsync(token);
            Assert.True(isValid);
        }

        [Fact]
        public async Task JwtWithIssuerMismatch()
        {
            var token = await _assymetricKmsJwtService.GenerateTokenAsync(_moduleIdentity, 60, "Issuer2", "Audience");
            var isValid = await _assymetricKmsJwtService.ValidateTokenAsync(token);
            Assert.False(isValid);
        }

        [Fact]
        public async Task JwtWithAudienceMismatch()
        {
            var token = await _assymetricKmsJwtService.GenerateTokenAsync(_moduleIdentity, 60, "Issuer", "Audience2");
            var isValid = await _assymetricKmsJwtService.ValidateTokenAsync(token);
            Assert.False(isValid);
        }
    }
}
