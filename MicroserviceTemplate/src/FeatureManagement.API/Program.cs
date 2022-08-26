using FeatureFlag.API.Configuration;
using Microsoft.AspNetCore.Mvc;
using Microsoft.FeatureManagement;
using Microsoft.FeatureManagement.FeatureFilters;
using ServiceName.API.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerSupport();
builder.Services.AddApiVersioningSupport();

var dataPollFrequencyInMinutes = 5;

builder.Configuration.SetBasePath(Environment.CurrentDirectory)
                     .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                     //.AddSystemsManager($"/{builder.Configuration["AwsAppConfig:ApplicationId"]}/", TimeSpan.FromMinutes(dataPollFrequencyInMinutes))
                     //.AddAppConfigUsingLambdaExtension(builder.Configuration["AwsAppConfig:ApplicationId"],
                     //                                  builder.Configuration["AwsAppConfig:EnvironmentId"],
                     //                                  builder.Configuration["AwsAppConfig:FreeformConfigurationProfileId"])
                     //.AddAmazonFeatureFlags(c =>
                     //{
                     //    c.ApplicationId = builder.Configuration["AwsAppConfig:ApplicationId"];
                     //    c.EnvironmentId = builder.Configuration["AwsAppConfig:EnvironmentId"];
                     //    c.ConfigurationProfileId = builder.Configuration["AwsAppConfig:FeatureFlagConfigurationProfileId"];
                     //    c.DataPollFrequency = TimeSpan.FromMinutes(dataPollFrequencyInMinutes);
                     //    c.ConfigSectionNaming = "FeatureFlags";
                     //})
                     .Build();

builder.Services.AddFeatureManagement(builder.Configuration.GetSection("FeatureFlags"))
                .AddFeatureFilter<ContextualTargetingFilter>();

builder.Services.Configure<FeatureManagementOptions>(options =>
{
    options.IgnoreMissingFeatureFilters = true;
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.ConfigureSwaggerUI();
}

app.UseHttpsRedirection();

app.MapGet("feature/toggle/simple/{featureName}/enabled", async (IFeatureManager featureManager, string featureName) =>
{
    try
    {
        return await featureManager.IsEnabledAsync(featureName);
    }
    catch (Exception)
    {
        //Log Exception
        return false;
    }
});

app.MapGet("feature/toggle/targeting/{featureName}/enabled", async (IFeatureManager featureManager, string featureName, [FromQuery(Name = "instanceId")] string instanceId, [FromQuery(Name = "userId")] string userId) =>
{
    // userId and groups defined somewhere earlier in application
    var targetingContext = new TargetingContext()
    {
        UserId = $"{instanceId}:{userId}",
        Groups = new List<string>() { instanceId }
    };
    return await featureManager.IsEnabledAsync(featureName, targetingContext);
});


app.Run();

