using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using ServiceName.API.Extensions.Swagger;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace ServiceName.API.Extensions
{
    public static class SwaggerExtensions
    {
        public static void AddSwaggerSupport(this IServiceCollection services)
        {
            services.AddTransient<IConfigureOptions<SwaggerGenOptions>, ConfigureSwaggerOptions>();            
            
            services.AddSwaggerGen(options =>
            {
                options.OperationFilter<SwaggerDefaultValues>();

                options.AddSecurityDefinition("basic", new OpenApiSecurityScheme
                {
                    Name = "Authorization",
                    Type = SecuritySchemeType.Http,
                    Scheme = "basic",
                    In = ParameterLocation.Header,
                    Description = "Basic Authorization header using the Bearer scheme."
                });

                options.AddSecurityRequirement(new OpenApiSecurityRequirement
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
        }
        
        public static void ConfigureSwaggerUI(this WebApplication app)
        {
            app.UseSwagger();
            app.UseSwaggerUI(options =>
            {
                options.RoutePrefix = string.Empty;
                
                var descriptions = app.DescribeApiVersions();

                // build a swagger endpoint for each discovered API version
                foreach (var description in descriptions)
                {
                    var url = $"./swagger/{description.GroupName}/swagger.json";
                    var name = description.GroupName.ToUpperInvariant();
                    options.SwaggerEndpoint(url, name);
                }
            });
        }
    }
}
