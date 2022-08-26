using Microsoft.AspNetCore.Mvc;
using Segment;
using Segment.Model;
using ServiceName.API.Extensions;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerSupport();
builder.Services.AddApiVersioningSupport();

// Add services to the container.
builder.Services.AddControllers();

// Add AWS Lambda support. When application is run in Lambda Kestrel is swapped out as the web server with Amazon.Lambda.AspNetCoreServer. This
// package will act as the webserver translating request and responses between the Lambda event source and ASP.NET Core.
builder.Services.AddAWSLambdaHosting(LambdaEventSource.RestApi);

var app = builder.Build();
if (app.Environment.IsDevelopment())
{
    app.ConfigureSwaggerUI();
}


app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.MapPost("/{userId}/{action}", (string userId, string action, [FromBody] Traits traits) => 
{
    var client = new Client("your project's write key");
    client.Identify(userId, traits);
    client.Track(userId, action);
});

app.Run();
