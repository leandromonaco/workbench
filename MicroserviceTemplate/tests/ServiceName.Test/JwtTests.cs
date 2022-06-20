using System.Text;
using System.Text.Json;
using Amazon;
using Amazon.KeyManagementService;
using Amazon.KeyManagementService.Model;
using Microsoft.Extensions.Configuration;
using Moq;
using Moq.AutoMock;
using Serilog;
using ServiceName.Core.Model;
using ServiceName.Infrastructure.Authentication;
using ServiceName.Test.Helpers;

namespace ServiceName.Test
{
    public class JwtTests
    {
        AssymetricKmsJwtService _assymetricKmsJwtService;
        ModuleIdentity _moduleIdentity;
        IConfiguration _configurationMock;

        public JwtTests()
        {
            var _autoMocker = new AutoMocker();
            _configurationMock = TestHelper.GetConfigurationMock();

            _moduleIdentity = new()
            {
                UserGuid = "UserGuid",
                InstanceGuid = "InstanceGuid",
                UserName = "UserName"
            };

            //SignResponse mockSignResponse = JsonSerializer.Deserialize<SignResponse>(@"{""KeyId"":""arn:aws:Kms:eu-west-2:111122223333:key/6732c7ca-6ec9-4b96-9711-fd1c7d637c8e"",""Signature"":null,""SigningAlgorithm"":{""Value"":""RSASSA_PKCS1_V1_5_SHA_256""},""ResponseMetadata"":{""RequestId"":"""",""Metadata"":{},""ChecksumAlgorithm"":0,""ChecksumValidationStatus"":0},""ContentLength"":493,""HttpStatusCode"":200}");
            //byte[] byteArray = Encoding.UTF8.GetBytes("hzxyCYR5Zse5pzb49qr9ydvusAiPkCCYlr961/orhNUYLo0oOyLeBcW6rIlaI8id7TeIHtENCOrPGc8aUXxLjsWW4KuKthPaU/1LC3lBBaEzA1gs2VpRZajzWbCCPHhwcI522dypVi4TwabMgmlRh8iPD6QOxPexvtPnibetIcBwTZx6viLdepyz1mdd9RKAQprjSvI4K9Lm84NRaUXs969qfXlfKSRUVpDpWxWQ2pDnPt847WbDQZM8AR2U3aEfVN+56gilzOSE4LAlXPqgfRmzdtJzZA3Lv3wULBS96Eq1LkPfaXovk2yzU/dQL6/T3X/azDenl5kyymDovuCmvw==");
            //mockSignResponse.Signature = new MemoryStream(byteArray);
            //_autoMocker.GetMock<IAmazonKeyManagementService>().Setup(x => x.SignAsync(It.IsAny<SignRequest>(), default)).ReturnsAsync(mockSignResponse);

            _autoMocker.Use<IAmazonKeyManagementService>(GetAmazonKms()); //This should be replaced with a SignResponse mock as response of the SignAsync method
            _autoMocker.Use<IConfiguration>(_configurationMock);
            _assymetricKmsJwtService = _autoMocker.CreateInstance<AssymetricKmsJwtService>();
        }

        /// <summary>
        /// TODO: Move to Helper class
        /// </summary>
        /// <returns></returns>
        private  IAmazonKeyManagementService GetAmazonKms()
        {
            var accessKey = _configurationMock["ModuleConfiguration:Infrastructure:Kms:AccessKey"];
            var secretKey = _configurationMock["ModuleConfiguration:Infrastructure:Kms:SecretKey"];
            var regionEndpoint = RegionEndpoint.GetBySystemName(_configurationMock["ModuleConfiguration:Infrastructure:Kms:RegionEndpoint"]);
            var localTestEndpoint = _configurationMock["ModuleConfiguration:Infrastructure:Kms:LocalTestEndpoint"];

            AmazonKeyManagementServiceConfig amazonKeyManagementServiceConfig = new()
            {
                RegionEndpoint = regionEndpoint,
            };

            if (!string.IsNullOrEmpty(localTestEndpoint))
            {
                amazonKeyManagementServiceConfig.UseHttp = true;
                amazonKeyManagementServiceConfig.ServiceURL = localTestEndpoint;
            }

            if (!string.IsNullOrEmpty(accessKey) && !string.IsNullOrEmpty(secretKey))
            {
                return new AmazonKeyManagementServiceClient(accessKey, secretKey, amazonKeyManagementServiceConfig);
            }

            return new AmazonKeyManagementServiceClient(amazonKeyManagementServiceConfig);
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
