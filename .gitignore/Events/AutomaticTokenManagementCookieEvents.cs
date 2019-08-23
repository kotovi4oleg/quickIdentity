using System;
using System.Globalization;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using IdentityModel.Client;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;

namespace WebMvc.Events
{
    public class AutomaticTokenManagementCookieEvents : CookieAuthenticationEvents
    {
        public override async Task ValidatePrincipal(CookieValidatePrincipalContext context) {
            var tokens = context.Properties.GetTokens();
            var refreshToken = tokens.SingleOrDefault(token => token.Name == "refresh_token");
            var expires = tokens.SingleOrDefault(token => token.Name == "expires_at");
            var dtExpires = DateTimeOffset.Parse(expires.Value, CultureInfo.InvariantCulture);
            if (dtExpires < DateTimeOffset.UtcNow) {
                var client = new HttpClient();
                var discovery = client.GetDiscoveryDocumentAsync("http://localhost:5000").Result;

                var result = await client.RequestRefreshTokenAsync(new RefreshTokenRequest {
                    Address = discovery.TokenEndpoint,
                    ClientId = "mvc",
                    ClientSecret = "secret",
                    RefreshToken = refreshToken?.Value
                });

                var clock = DateTime.UtcNow.AddSeconds(result.ExpiresIn);

                context.Properties.UpdateTokenValue("access_token", result.AccessToken);
                context.Properties.UpdateTokenValue("id_token", result.IdentityToken);
                context.Properties.UpdateTokenValue("refresh_token", result.RefreshToken);
                context.Properties.UpdateTokenValue("expires_at", clock.ToString());
                await context.HttpContext.SignInAsync(context.Principal, context.Properties);
            }

            await base.ValidatePrincipal(context);
        }
    }
}