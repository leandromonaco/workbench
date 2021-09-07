using IntegrationConnectors.Common;
using IntegrationConnectors.Confluence;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConfluenceKB
{
    public class Startup
    {
        public Startup()
        {
            Configuration = new ConfigurationBuilder()
                                        .AddJsonFile("appsettings.json", optional: true)
                                        .AddUserSecrets("cc850d98-20e6-4d48-ac6c-40b7611f4c5b")
                                        .AddEnvironmentVariables()
                                        .Build(); 
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddRazorPages();
            services.AddServerSideBlazor();

            var key = Convert.ToBase64String(Encoding.UTF8.GetBytes($"{Configuration["Confluence:User"]}:{Configuration["Confluence:Password"]}"));
            var confluenceConnector = new ConfluenceConnector(Configuration["Confluence:RestUrl"], key, AuthenticationType.Basic);
            services.AddSingleton(confluenceConnector);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapBlazorHub();
                endpoints.MapFallbackToPage("/_Host");
            });
        }
    }
}
