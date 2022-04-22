using IdentityServer;
using Serilog;
using TeamHub.Core;

namespace TeamHub.Identity
{
    internal static class HostingExtensions
    {
        public static WebApplication ConfigureServices(this WebApplicationBuilder builder)
        {
            // Configuring IdentityServer
            var x509 = Security.LoadCertificate();

            builder.Services.AddIdentityServer()
                            .AddInMemoryApiScopes(Config.ApiScopes)
                            .AddInMemoryClients(Config.Clients)
                            .AddSigningCredential(x509)
                            .AddValidationKey(x509);

            return builder.Build();
        }

      

        public static WebApplication ConfigurePipeline(this WebApplication app)
        {
            app.UseSerilogRequestLogging();

            if (app.Environment.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            // uncomment if you want to add a UI
            //app.UseStaticFiles();
            //app.UseRouting();

            app.UseIdentityServer();

            // uncomment if you want to add a UI
            //app.UseAuthorization();
            //app.MapRazorPages().RequireAuthorization();

            return app;
        }
    }
}