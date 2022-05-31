using MediatR;
using Microsoft.OpenApi.Models;
using Settings.Application;
using Settings.Application.Common.Interfaces;
using Settings.Application.Interfaces;
using Settings.Application.SettingItems.Queries;
using Settings.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();

// Add AWS Lambda support. When application is run in Lambda Kestrel is swapped out as the web server with Amazon.Lambda.AspNetCoreServer. This
// package will act as the webserver translating request and responses between the Lambda event source and ASP.NET Core.
builder.Services.AddAWSLambdaHosting(LambdaEventSource.RestApi);

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

//Configure Swagger with Bearer Token authentication
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Microservice API", Version = "v1" });

    c.AddSecurityDefinition("basic", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "basic",
        In = ParameterLocation.Header,
        Description = "Basic Authorization header using the Bearer scheme."
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                          new OpenApiSecurityScheme
                            {
                                Reference = new OpenApiReference
                                {
                                    Type = ReferenceType.SecurityScheme,
                                    Id = "basic"
                                }
                            },
                            new string[] {}
                    }
                });
});

//Clean Architecute: Injects Services

builder.Services.AddApplicationServices(builder.Configuration, builder.Logging);
builder.Services.AddInfrastructureServices(builder.Configuration, builder.Logging);


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthentication();
app.UseAuthorization();
app.UseHttpsRedirection();

app.MapControllers();

app.MapGet("/settings", (IMediator mediator) => mediator.Send(new GetSettingsQueryRequest()));
//app.MapGet("/settings", (ISettingsService settingsService) => settingsService.GetSettings());
//app.MapGet("/token", (IAuthenticationService authenticationService) => authenticationService.GenerateToken(1));

app.Run();

