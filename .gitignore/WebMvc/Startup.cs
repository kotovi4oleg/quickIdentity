using AuthLib;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using WebMvc.Events;

namespace WebMvc
{
    public class Startup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc();
            //services.AddDataProtection(options =>
            //{
            //    options.ApplicationDiscriminator = "trololo";
            //});
            services.AddAuthentication(options => {
                options.DefaultScheme = "MyCookie";
                options.DefaultChallengeScheme = "oidc";
            })
            .AddCookie("MyCookie", options => {
                    //options.ExpireTimeSpan = TimeSpan.FromSeconds(60);
                    options.EventsType = typeof(AutomaticTokenManagementCookieEvents);
                    //options.CookieManager = new MyChunkingCookieManager();
                    options.TicketDataFormat = new OwnSecureDataFormat();
                    
                })
            .AddOpenIdConnect("oidc", options => {
                options.Authority = "http://localhost:5000";
                options.RequireHttpsMetadata = false;

                options.SignInScheme = "MyCookie";

                options.ClientId = "mvc";
                options.ClientSecret = "secret";
                options.ResponseType = "code id_token";
                options.GetClaimsFromUserInfoEndpoint = true;


                options.CallbackPath = new PathString("/home/signin-oidc");
                //options.BackchannelHttpHandler = new BackChannelHttpMessageHandler();


                options.Scope.Add("offline_access");
                options.Scope.Add("afcpayroll");
                options.Scope.Add("afcpayroll2");
                options.Scope.Add("identityprovider");

                options.SaveTokens = true;
            });

            services.AddScoped<AutomaticTokenManagementCookieEvents>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseAuthentication();
            app.UseStaticFiles();
            app.UseMvcWithDefaultRoute();
        }
    }
}
