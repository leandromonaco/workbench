using Amazon.DynamoDBv2.DataModel;
using Moq;
using Moq.AutoMock;
using ServiceName.Infrastructure.Repositories;
using ServiceName.Infrastructure.Repositories.DynamoDBModel;
using ServiceName.Test.Helpers;

namespace ServiceName.Test
{
    public class RepositoryTests
    {
        readonly AutoMocker _autoMocker;
        readonly SettingsRepositoryService _mockSettingsRepository;

        public RepositoryTests()
        {
            _autoMocker = new AutoMocker();
            _mockSettingsRepository = _autoMocker.CreateInstance<SettingsRepositoryService>();
        }
        
        [Fact]
        public async Task SettingRepositoryTestWhenTenantIdExists()
        {
            _autoMocker.GetMock<IDynamoDBContext>().Setup(x => x.LoadAsync<SettingDbRecord>("53a13ec4-fde8-4087-8e2a-5fb6b1fbc062", default)).ReturnsAsync(TestHelper.GetSettingsRecordObject());
            
            var result = await _mockSettingsRepository.GetByIdAsync(Guid.Parse("53a13ec4-fde8-4087-8e2a-5fb6b1fbc062"));

            Assert.NotNull(result.CategoryA);
            Assert.True(result.CategoryA.IsSettingAEnabled);
            Assert.True(result.CategoryA.IsSettingBEnabled);
        }

        [Fact]
        public async Task SettingRepositoryTestWhenTenantIdDoesNotExist()
        {
            _autoMocker.GetMock<IDynamoDBContext>().Setup(x => x.LoadAsync<SettingDbRecord>("296b73d9-692a-42bf-9ccb-ff41ca256722", default)).ReturnsAsync((SettingDbRecord)null);
            
            var result = await _mockSettingsRepository.GetByIdAsync(Guid.Parse("296b73d9-692a-42bf-9ccb-ff41ca256722"));

            Assert.NotNull(result.CategoryA);
            Assert.False(result.CategoryA.IsSettingAEnabled);
            Assert.False(result.CategoryA.IsSettingBEnabled);
        }
    }
}