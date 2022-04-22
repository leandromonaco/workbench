using IdentityModel.Client;
using Microsoft.AspNetCore.Authorization;

namespace TeamHub.API.Controllers
{
    public static class IdentityController
    {
        public static void MapIdentityControllerEndpoints(this WebApplication app)
        {
            //All
            app.MapGet("/token", [AllowAnonymous] async () =>
            {
                // discover endpoints from metadata
                var client = new HttpClient();
                var disco = await client.GetDiscoveryDocumentAsync("https://localhost:5001");
                //if (disco.IsError)
                //{
                //    Console.WriteLine(disco.Error);
                //    return;
                //}

                // request token
                var tokenResponse = await client.RequestClientCredentialsTokenAsync(new ClientCredentialsTokenRequest
                {
                    Address = disco.TokenEndpoint,

                    ClientId = "client",
                    ClientSecret = "secret",
                    Scope = "api1"
                });

                //if (tokenResponse.IsError)
                //{
                //    Console.WriteLine(tokenResponse.Error);
                //    return;
                //}

                return tokenResponse.AccessToken;
            }).RequireCors("MyAllowSpecificOrigins");

            

        }
    }
}
