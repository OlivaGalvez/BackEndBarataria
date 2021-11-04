using BaratariaBackend.Models.Context;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;

namespace BaratariaBackend
{
    public class Startup
    {
        public Startup(IWebHostEnvironment env, IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc();

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                options.DefaultSignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = OpenIdConnectDefaults.AuthenticationScheme;
            })
           .AddCookie()
           .AddOpenIdConnect(options =>
           {
               options.Authority = Configuration["Authentication:oidc:Authority"];
               options.ClientId = Configuration["Authentication:oidc:ClientId"];
               options.ClientSecret = Configuration["Authentication:oidc:ClientSecret"];
               options.RequireHttpsMetadata = false;
               options.GetClaimsFromUserInfoEndpoint = true;
               options.SaveTokens = true;
               options.RemoteSignOutPath = "/SignOut";
               options.SignedOutRedirectUri = "Redirect-here";
               options.ResponseType = "code";

           });

            services.AddEntityFrameworkNpgsql().AddDbContext<ApplicationDbContext>(options =>
            {
                options.UseNpgsql(Configuration.GetConnectionString("DevConnection"));
            });

            //Habilitar CORS para controlar las solicitudes entre orígenes (necesario para el FrontEnd)
            services.AddCors(options => options.AddPolicy("AllowWebApp",
                    builder => builder.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod()));

            services.AddControllers();
            services.AddSwaggerGen();

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseCors("AllowWebApp");

            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {

                c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
                c.SupportedSubmitMethods();

                //Para servir la interfaz de usuario de Swagger en la raíz de la aplicación
                c.RoutePrefix = string.Empty;

            });


            app.UseDefaultFiles();
            app.UseStaticFiles();

            app.UseHttpsRedirection();
            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseDeveloperExceptionPage();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
