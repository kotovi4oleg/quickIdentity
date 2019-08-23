using Api.Mwr;
using AuthLib;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;

namespace Api
{
    public class Startup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            //services.AddDataProtection(options => {
            //    options.ApplicationDiscriminator = "trololo";
            //});

            services.AddMvcCore()
                .AddAuthorization()
                .AddJsonFormatters();

            services
            .AddAuthentication(options => {
                options.DefaultScheme = "MyCookie";
                options.DefaultChallengeScheme = "Bearer";
            })
            .AddCookie("MyCookie", options => {
                options.TicketDataFormat = new OwnSecureDataFormat();
            });

            services
                .AddAuthentication("Bearer")
                .AddJwtBearer("Bearer", options => {
                    options.Authority = "http://localhost:5000";
                    options.RequireHttpsMetadata = false;
                    options.Audience = "afcpayroll";
                });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            app.UseMiddleware<SecureHeaderMiddleware>();

            app.UseAuthentication();

            app.UseMvc();

            

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
        }
    }
}
