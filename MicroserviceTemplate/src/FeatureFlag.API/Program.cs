using Microsoft.FeatureManagement;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Configuration.SetBasePath(Environment.CurrentDirectory)
                     .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                     .AddSystemsManager("/AppConfigApplicationId/", TimeSpan.FromMinutes(5))
                     //.AddSystemsManager("/kbt546s/", TimeSpan.FromMinutes(5))
                     //.AddAppConfigUsingLambdaExtension("AppConfigApplicationId", "AppConfigEnvironmentId", "AppConfigConfigurationProfileId")
                    .AddAppConfigUsingLambdaExtension("kbt546s", "bcre59f", "gdx1prc")
                    .Build();

builder.Services.AddFeatureManagement();

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

