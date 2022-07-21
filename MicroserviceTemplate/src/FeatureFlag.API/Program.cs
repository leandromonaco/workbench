using Amazon.AppConfigData;
using Amazon.AppConfigData.Model;
using FeatureFlag.API;
using Microsoft.FeatureManagement;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var dataPollFrequencyInMinutes = 5;

builder.Configuration.SetBasePath(Environment.CurrentDirectory)
                     .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                     .AddSystemsManager($"/{builder.Configuration["AwsAppConfig:ApplicationId"]}/", TimeSpan.FromMinutes(dataPollFrequencyInMinutes))
                     .AddAppConfigUsingLambdaExtension(builder.Configuration["AwsAppConfig:ApplicationId"], builder.Configuration["AwsAppConfig:EnvironmentId"], builder.Configuration["AwsAppConfig:FreeformConfigurationProfileId"])
                     .AddAmazonFeatureFlags(c =>
                     {
                         //these can be the id and the name as well...
                         c.ApplicationId = builder.Configuration["AwsAppConfig:ApplicationId"];
                         c.EnvironmentId = builder.Configuration["AwsAppConfig:EnvironmentId"];
                         c.ConfigurationProfileId = builder.Configuration["AwsAppConfig:FeatureFlagConfigurationProfileId"];
                         c.DataPollFrequency = TimeSpan.FromMinutes(dataPollFrequencyInMinutes);
                         c.ConfigSectionNaming = "FeatureFlags"; //this is what you should use when u add the feature management service (next line), also can be empty.
                     })
                     .Build();


builder.Services.AddFeatureManagement(builder.Configuration.GetSection("FeatureFlags"));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapGet("/feature/{featureName}", async (IFeatureManager featureManager, string featureName) =>
{
    return await featureManager.IsEnabledAsync(featureName);
});

app.Run();

