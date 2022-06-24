using Amazon.DynamoDBv2.DataModel;
using Moq;
using Moq.AutoMock;
using ServiceName.Infrastructure.Repositories;
using ServiceName.Infrastructure.Repositories.DynamoDBModel;
using ServiceName.Test.Helpers;

namespace ServiceName.Test.UnitTesting
{
    public class RepositoryTests
    {
        private readonly AutoMocker _autoMocker;
        private readonly SettingsRepositoryService _mockSettingsRepository;
        private TestHelper _testHelper;

        public RepositoryTests()
        {
            _autoMocker = new AutoMocker();
            _mockSettingsRepository = _autoMocker.CreateInstance<SettingsRepositoryService>();
            _testHelper = new TestHelper();
        }

        [Fact]
        public async Task SettingRepositoryTestWhenTenantIdExists()
        {
            var tenantId = "53a13ec4-fde8-4087-8e2a-5fb6b1fbc062";

            _autoMocker.GetMock<IDynamoDBContext>().Setup(x => x.LoadAsync<SettingDbRecord>(tenantId, default)).ReturnsAsync(_testHelper.GetDynamoDBRecord(tenantId));

            var result = await _mockSettingsRepository.GetByIdAsync(Guid.Parse(tenantId));

            Assert.NotNull(result.CategoryA);
            Assert.True(result.CategoryA.IsSettingAEnabled);
            Assert.True(result.CategoryA.IsSettingBEnabled);
        }

        [Fact]
        public async Task SettingRepositoryTestWhenTenantIdDoesNotExist()
        {
            var tenantId = "296b73d9-692a-42bf-9ccb-ff41ca256722";

            _autoMocker.GetMock<IDynamoDBContext>().Setup(x => x.LoadAsync<SettingDbRecord>(tenantId, default)).ReturnsAsync(_testHelper.GetDynamoDBRecord(tenantId));

            var result = await _mockSettingsRepository.GetByIdAsync(Guid.Parse(tenantId));

            Assert.NotNull(result.CategoryA);
            Assert.False(result.CategoryA.IsSettingAEnabled);
            Assert.False(result.CategoryA.IsSettingBEnabled);
        }

        [Fact]
        public async Task SettingRepositoryTestWhenSettingsAreEmpty()
        {
            var tenantId = "e2c92679-5309-438b-8efc-64054a7babc2";

            _autoMocker.GetMock<IDynamoDBContext>().Setup(x => x.LoadAsync<SettingDbRecord>(tenantId, default)).ReturnsAsync(_testHelper.GetDynamoDBRecord(tenantId));

            var result = await _mockSettingsRepository.GetByIdAsync(Guid.Parse(tenantId));

            Assert.NotNull(result.CategoryA);
            Assert.False(result.CategoryA.IsSettingAEnabled);
            Assert.False(result.CategoryA.IsSettingBEnabled);
        }
    }
}