using ServiceName.API.BackgroundTasks;
using ServiceName.API.Extensions;
using ServiceName.Core;
using ServiceName.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();

// Add AWS Lambda support. When application is run in Lambda Kestrel is swapped out as the web server with Amazon.Lambda.AspNetCoreServer. This
// package will act as the webserver translating request and responses between the Lambda event source and ASP.NET Core.
// https://aws.amazon.com/blogs/compute/introducing-the-net-6-runtime-for-aws-lambda/

builder.Services.AddAWSLambdaHosting(LambdaEventSource.HttpApi);

builder.Services.AddInfrastructureServices(builder.Configuration);

builder.Services.AddApplicationServices();

builder.Services.AddApiVersioningSupport();

builder.Services.AddSwaggerSupport();

builder.Services.AddHealthCheckSupport(builder.Configuration);

builder.Services.AddAuthSupport(builder.Configuration);

builder.Services.AddEndpointsApiExplorer();

//builder.Services.AddHostedService<RepeatingTask>();

var app = builder.Build();

app.MapControllers();

app.MapEndpoints(builder.Configuration);

app.ConfigureExceptionHandler();

app.ConfigureHealthCheck();

if (bool.Parse(builder.Configuration["ModuleConfiguration:IsSwaggerUIEnabled"]))
{
    app.ConfigureSwaggerUI();
}

app.UseAuth();

app.UseHttpsRedirection();

app.Run();