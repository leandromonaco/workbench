using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using Mock.API.Model;
using Mock.API.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerSupport();
builder.Services.AddApiVersioningSupport();

// Add services to the container.
builder.Services.AddControllers();

// Add AWS Lambda support. When application is run in Lambda Kestrel is swapped out as the web server with Amazon.Lambda.AspNetCoreServer. This
// package will act as the webserver translating request and responses between the Lambda event source and ASP.NET Core.
builder.Services.AddAWSLambdaHosting(LambdaEventSource.HttpApi);

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.ConfigureSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.MapGet("/resources", ([FromHeader] int delayInMilliseconds) => GetMocks(delayInMilliseconds));
app.MapGet("/resources/{id}", (string id, [FromHeader] int delayInMilliseconds) => GetMock(id, delayInMilliseconds));

app.Run();


ResourceMock? GetMock(string id, int delayInMilliseconds)
{
    var mockJson = File.ReadAllText(Path.Combine(Directory.GetCurrentDirectory(), "Files", "Mock.json"));
    var mockFile = JsonSerializer.Deserialize<MockFile>(mockJson);

    Thread.Sleep(delayInMilliseconds);

    return mockFile?.Resources?.FirstOrDefault(m => m.Id.Equals(id));
}

List<ResourceMock?> GetMocks(int delayInMilliseconds)
{
    var mockJson = File.ReadAllText(Path.Combine(Directory.GetCurrentDirectory(), "Files", "Mock.json"));
    var mockFile = JsonSerializer.Deserialize<MockFile>(mockJson);

    Thread.Sleep(delayInMilliseconds);

    return mockFile?.Resources!;
}