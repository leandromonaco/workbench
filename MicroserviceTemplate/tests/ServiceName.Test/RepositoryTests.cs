using Moq;
using ServiceName.Core.Common.Interfaces;
using ServiceName.Core.Model;

namespace ServiceName.Test
{
    public class RepositoryTests
    {
        [Fact]
        public async Task SettingRepositoryTest()
        {
            var tenantId = Guid.Parse("58628ac6-eba8-4e9f-87fe-a2d1b69d0d34");
            var mock = new Mock<IRepositoryService<Settings>>();
            mock.Setup(x => x.GetByIdAsync(tenantId)).Returns(Task.FromResult(GetSettingsObject()));

            var result = await mock.Object.GetByIdAsync(tenantId);

            Assert.NotNull(result.CategoryA);
            Assert.True(result.CategoryA.IsSettingAEnabled);
            Assert.False(result.CategoryA.IsSettingBEnabled);
        }


        private Settings GetSettingsObject()
        {
            return new Settings()
            {
                CategoryA = new SettingGroup() { IsSettingAEnabled = true, 
                                                 IsSettingBEnabled = false }

            };
        }
    }
}