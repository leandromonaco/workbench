using Moq.AutoMock;
using ServiceName.Core.Model;
using ServiceName.Infrastructure.Authentication;
using ServiceName.Test.Helpers;

namespace ServiceName.Test.IntegrationTesting
{
    public class JwtTests
    {
        private AssymetricKmsJwtService _assymetricKmsJwtService;
        private ModuleIdentity _moduleIdentity;
        private TestHelper _testHelper;

        public JwtTests()
        {
            var _autoMocker = new AutoMocker();

            _moduleIdentity = new()
            {
                UserGuid = "UserGuid",
                InstanceGuid = "InstanceGuid",
                UserName = "UserName"
            };

            _testHelper = new TestHelper();

            _autoMocker.Use(_testHelper.GetAmazonKms());
            _autoMocker.Use(_testHelper.Configuration);
            _assymetricKmsJwtService = _autoMocker.CreateInstance<AssymetricKmsJwtService>();
        }

        [Fact]
        public async Task JwtValidationOK()
        {
            var token = await _assymetricKmsJwtService.GenerateTokenAsync(_moduleIdentity, 60, "issuer", "audience");
            var isValid = await _assymetricKmsJwtService.ValidateTokenAsync(token);
            Assert.True(isValid);
        }

        [Fact]
        public async Task JwtWithIssuerMismatch()
        {
            var token = await _assymetricKmsJwtService.GenerateTokenAsync(_moduleIdentity, 60, "Issuer", "Audience");
            var isValid = await _assymetricKmsJwtService.ValidateTokenAsync(token);
            Assert.False(isValid);
        }

        [Fact]
        public async Task JwtWithAudienceMismatch()
        {
            var token = await _assymetricKmsJwtService.GenerateTokenAsync(_moduleIdentity, 60, "Issuer2", "Audience2");
            var isValid = await _assymetricKmsJwtService.ValidateTokenAsync(token);
            Assert.False(isValid);
        }
    }
}