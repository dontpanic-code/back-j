using System;
using System.Collections.Generic;
using System.IO.Compression;
using System.Linq;
using System.Net;
using System.Net.WebSockets;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.AspNetCore.SpaServices.AngularCli;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json.Serialization;
using Reenbit.HireMe.API.Extensions;
//using Microsoft.AspNetCore.Authentication.JwtBearer;
//using Microsoft.AspNetCore.Authentication.AzureAD.UI;
//using Microsoft.AspNetCore.Authentication;

namespace Reenbit.HireMe.API
{
    public class Startup
    {
        public Startup(IConfiguration configuration, IWebHostEnvironment environment)
        {
            this.configuration = configuration;
            this.environment = environment;
        }

        private IConfiguration configuration { get; }

        private IWebHostEnvironment environment { get; set; }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddApplicationInsightsTelemetry();
            services.RegisterDependencies();
            services.AddControllers();

            services.AddSignalR();

            this.ConfigureAuth(services);

            //services.AddMvc();
            //services.AddSingleton<SocketManager>();

            services.AddCors(options =>
                options.AddPolicy(
                "HireMeCorsPolicy",
                builder =>
                {
                    builder = environment.IsProduction()
                        ? builder.WithOrigins(configuration["UiAppUrl"])
                        : builder.AllowAnyOrigin();
                    
                    builder
                        .AllowAnyOrigin()
                        .AllowAnyMethod()
                        .AllowAnyHeader()
                        .WithExposedHeaders("Operation-Location");
                }));

            services.AddSpaStaticFiles(configuration =>
            {
                configuration.RootPath = "wwwroot";
                //configuration.RootPath = "ClientApp/dist/MScore";
            });

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "HireMe API", Version = "v1" });
            });

            // Configure Compression level
            services.Configure<GzipCompressionProviderOptions>(options => options.Level = CompressionLevel.Fastest);

            // Add Response compression services
            services.AddResponseCompression(options =>
            {
                options.Providers.Add<GzipCompressionProvider>();
                options.EnableForHttps = true;
            });

            services
                .AddSignalR()
                .AddJsonProtocol();

          //  services.AddAuthentication(AzureADDefaults.JwtBearerAuthenticationScheme)
          //.AddAzureADBearer(options => this.configuration.Bind("AzureAd", options));

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            //app.UseWebSockets();
            //app.UseMiddleware<SocketMiddleware>();
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c =>
                {
                    c.SwaggerEndpoint("/swagger/v1/swagger.json", "HireMe API V1");
                });
            }
            
            app.UseHttpsRedirection();
            
            app.UseRouting();

            app.UseStaticFiles();

            app.UseCors("HireMeCorsPolicy");

            app.UseResponseCompression();

            app.UseAuthentication();
            app.UseAuthorization();
            

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapHub<ChatHub>("/chat");
                endpoints.MapHub<GroupChatHub>("/groupchat");
            });

            
            app.UseSpaStaticFiles();

            app.UseSpa(spa =>
            {
                spa.Options.SourcePath = "wwwroot";
            });

        }

        private void ConfigureAuth(IServiceCollection services)
        {
            services
                .AddAuthentication(options =>
                {
                    options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;

                })
                .AddCookie(options =>
                {
                    options.LoginPath = "/signin";
                    options.LogoutPath = "/signout";
                })
                .AddFacebook(facebookOptions =>
                {
                    facebookOptions.AppId = this.configuration["Authentication:Facebook:AppId"];
                    facebookOptions.AppSecret = this.configuration["Authentication:Facebook:AppSecret"];
                    //facebookOptions.Scope.Add("user:email");
                })
                .AddGoogle(options =>
                {
                    IConfigurationSection googleAuthNSection =
                        configuration.GetSection("Authentication:Google");

                    options.ClientId = googleAuthNSection["ClientId"];
                    options.ClientSecret = googleAuthNSection["ClientSecret"];
                })
                .AddLinkedIn(options =>
                {
                    IConfigurationSection linkedinAuthNSection =
                    configuration.GetSection("Authentication:Linkedin");

                    options.ClientId = linkedinAuthNSection["ClientId"];
                    options.ClientSecret = linkedinAuthNSection["ClientSecret"];

                });
        }

    }
}
