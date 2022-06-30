using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using Mock.API.Model;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();

// Add AWS Lambda support. When application is run in Lambda Kestrel is swapped out as the web server with Amazon.Lambda.AspNetCoreServer. This
// package will act as the webserver translating request and responses between the Lambda event source and ASP.NET Core.
builder.Services.AddAWSLambdaHosting(LambdaEventSource.RestApi);

var app = builder.Build();


app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.MapGet("/resources", () => GetMocks());
app.MapGet("/resources/{id}", (string id) => GetMock(id));

app.Run();


ResourceMock? GetMock(string id)
{
    var mockJson = File.ReadAllText(Path.Combine(Directory.GetCurrentDirectory(), "Files", "Mock.json"));
    var mockFile = JsonSerializer.Deserialize<MockFile>(mockJson);
    return mockFile?.Resources?.FirstOrDefault(m => m.Id.Equals(id));
}

List<ResourceMock?> GetMocks()
{
    var mockJson = File.ReadAllText(Path.Combine(Directory.GetCurrentDirectory(), "Files", "Mock.json"));
    var mockFile = JsonSerializer.Deserialize<MockFile>(mockJson);
    return mockFile?.Resources!;
}