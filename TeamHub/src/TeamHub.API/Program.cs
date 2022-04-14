using System.Text.Json;
using System.Text.Json.Serialization;
using TeamHub.API.Controllers;
using TeamHub.API.Database;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

//https://github.com/dotnet/aspnetcore/issues/35904
var jsonSerializerOptions = new JsonSerializerOptions()
{
    DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
    PropertyNameCaseInsensitive = true,
    ReferenceHandler = ReferenceHandler.Preserve,
    //MaxDepth = 1
};

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

TeamHubDatabaseContext dataContext = new TeamHubDatabaseContext();
app.MapEmployeesControllerEndpoints(dataContext, jsonSerializerOptions);

app.Run();

