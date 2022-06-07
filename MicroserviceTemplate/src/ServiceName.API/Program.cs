using ServiceName.API.Extensions;
using ServiceName.Core;
using ServiceName.Infrastructure;

var builder = WebApplication.CreateBuilder(args);
builder.Logging.ClearProviders();

// Add services to the container.
builder.Services.AddControllers();

// Add AWS Lambda support. When application is run in Lambda Kestrel is swapped out as the web server with Amazon.Lambda.AspNetCoreServer. This
// package will act as the webserver translating request and responses between the Lambda event source and ASP.NET Core.
// https://aws.amazon.com/blogs/compute/introducing-the-net-6-runtime-for-aws-lambda/
builder.Services.AddAWSLambdaHosting(LambdaEventSource.RestApi);

builder.Services.AddEndpointsApiExplorer();

//Configure Swagger with Bearer Token authentication
builder.Services.ConfigureSwaggerServices();

//Clean Architecute: Service Injection
builder.Services.AddApplicationServices();
builder.Services.AddInfrastructureServices(options => options.IsDevelopment = true);

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.ConfigureSwaggerUI();
}

app.UseAuthentication();
app.UseAuthorization();
app.UseHttpsRedirection();

app.MapControllers();
app.MapEndpoints();

app.Run();

