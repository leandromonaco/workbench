using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text.Json;
using System.Text.Json.Serialization;
using TeamHub.API.Controllers;
using TeamHub.API.Database;
using TeamHub.Core;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

//Configure Swagger with Bearer Token authentication
builder.Services.AddSwaggerGen(o =>
{
    //o.SwaggerDoc("v1", info);
    o.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
    {
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "JSON Web Token based security",
    });
    o.AddSecurityRequirement(new OpenApiSecurityRequirement()
                                    {
                                        {
                                            new OpenApiSecurityScheme
                                            {
                                            Reference = new OpenApiReference
                                            {
                                            Type = ReferenceType.SecurityScheme,
                                            Id = "Bearer"
                                            }
                                            },
                                            new string[] {}
                                        }
                                    });
});

//CORS Support
builder.Services.AddCors(options =>
                                    options.AddPolicy("MyAllowSpecificOrigins",
                                              builder =>
                                              {
                                                  builder.WithOrigins("https://localhost:4200")
                                                                      .AllowAnyMethod()
                                                                      .AllowAnyHeader()
                                                                      .AllowCredentials();
                                              }));

//Authentication with Identity Server
builder.Services.AddAuthentication("Bearer")
    .AddJwtBearer("Bearer", options =>
    {
        options.Authority = "https://localhost:5001";

        //https://devblogs.microsoft.com/dotnet/jwt-validation-and-authorization-in-asp-net-core/
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            ValidateIssuer = true,
            ValidIssuer = "http://localhost:5000/",
            ValidateAudience = false,
            IssuerSigningKey = new X509SecurityKey(Security.LoadCertificate())
        };
    });

//Authorization with Identity Server
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("ApiScope", policy =>
    {
        policy.RequireAuthenticatedUser();
        policy.RequireClaim("scope", "api1");
    });
});

var app = builder.Build();

app.UseAuthentication();
app.UseAuthorization();

//https://github.com/dotnet/aspnetcore/issues/35904
var jsonSerializerOptions = new JsonSerializerOptions()
{
    DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
    PropertyNameCaseInsensitive = true,
    WriteIndented = true,
    ReferenceHandler = ReferenceHandler.IgnoreCycles
};

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("MyAllowSpecificOrigins");

app.UseHttpsRedirection();

TeamHubDatabaseContext dataContext = new TeamHubDatabaseContext();

//Controllers
app.MapEmployeesControllerEndpoints(dataContext, jsonSerializerOptions);
app.MapIdentityControllerEndpoints();

app.Run();

