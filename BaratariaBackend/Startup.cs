using BaratariaBackend.Models.Context;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using System.Linq;

namespace BaratariaBackend
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {

                options.Authority = Configuration["Authentication:Authority"];
                options.Audience = Configuration["Authentication:Audience"];
                options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
                {
                    ValidateAudience = false
                };

                options.Events = new JwtBearerEvents()
                {
                    OnAuthenticationFailed = c =>
                    {
                        c.NoResult();

                        c.Response.StatusCode = 500;
                        c.Response.ContentType = "text/plain";
                        return c.Response.WriteAsync(c.Exception.ToString());
                    }
                };

                options.RequireHttpsMetadata = false; //for test only!
                options.SaveToken = true;
                options.Validate();

            });

            services.AddAuthorization(options =>
            {
                options.AddPolicy("Administrador", policy => policy.RequireClaim("member_of", "administrador"));
            });

            services.AddDbContext<ApplicationDbContext>(options =>
                    options.UseNpgsql(Configuration.GetConnectionString("DevConnection")));
            //services.AddEntityFrameworkNpgsql().AddDbContext<ApplicationDbContext>(options =>
            //{
            //    options.UseNpgsql(Configuration.GetConnectionString("DevConnection"));
            //});

            services.AddCors(options =>
            {
                options.AddDefaultPolicy(builder => builder
                    .AllowAnyOrigin()
                    .AllowAnyMethod()
                    .AllowAnyHeader()
                );
            });

            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.ResolveConflictingActions(apiDescriptions =>
                {
                    var descriptions = apiDescriptions as ApiDescription[] ?? apiDescriptions.ToArray();
                    var first = descriptions.First(); // build relative to the 1st method
                    var parameters = descriptions.SelectMany(d => d.ParameterDescriptions).ToList();

                    first.ParameterDescriptions.Clear();
                    // add parameters and make them optional
                    foreach (var parameter in parameters)
                        if (first.ParameterDescriptions.All(x => x.Name != parameter.Name))
                        {
                            first.ParameterDescriptions.Add(new ApiParameterDescription
                            {
                                ModelMetadata = parameter.ModelMetadata,
                                Name = parameter.Name,
                                ParameterDescriptor = parameter.ParameterDescriptor,
                                Source = parameter.Source,
                                IsRequired = false,
                                DefaultValue = null
                            });
                        }
                    return first;
                });
            });

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();
            app.UseCors();
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {

                c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
                c.SupportedSubmitMethods();

                //Para servir la interfaz de usuario de Swagger en la raíz de la aplicación
                c.RoutePrefix = string.Empty;
            });

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

        }
    }
}
