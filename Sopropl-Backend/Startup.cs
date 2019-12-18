using System;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNet.OData.Extensions;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Primitives;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using Sopropl_Backend.Data;
using Sopropl_Backend.Helpers;
using Sopropl_Backend.Repositories;
using Sopropl_Backend.SignalRHubs;

namespace Sopropl_Backend
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        [Obsolete]
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddCors(options =>
            {
                {
                    options.AddPolicy("CorsPolicy",
                        builder => builder.WithOrigins("http://localhost:4200")
                        .AllowAnyMethod()
                        .AllowAnyHeader()
                        .AllowCredentials());
                }
            });
            services.Configure<CloudinarySettings>(Configuration.GetSection("CloudinarySettings"));

            services.AddDbContext<SoproplDbContext>(options =>
            {
                options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection"));
            });
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(Configuration.GetSection("AppSettings:Token").Value)),
                    ValidateAudience = false,
                    ValidateIssuer = false
                };
                options.Events = new JwtBearerEvents
                {
                    OnMessageReceived = context =>
                    {
                        var accessToken = context.Request.Query["access_token"];

                        if (!string.IsNullOrEmpty(accessToken) &&
                            (context.HttpContext.WebSockets.IsWebSocketRequest || context.Request.Headers["Accept"] == "text/event-stream"))
                        {
                            context.Token = accessToken;
                        }
                        return Task.CompletedTask;
                    }
                };
            });
            services.AddSignalR();
            services.AddScoped<IAuthManager, AuthManager>();
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IPhotoRepository, PhotoRepository>();
            services.AddScoped<IOrganizationRepository, OrganizationRepository>();
            services.AddScoped<IProjectRepository, ProjectRepository>();
            services.AddScoped<IMemberRepository, MemberRepository>();
            services.AddScoped<INotificationRepository, NotificationRepository>();
            services.AddScoped<IInvitationRepository, InvitationRepository>();
            services.AddScoped<ITeamRepository, TeamRepository>();

            services.AddScoped<ISmtpClientRepository, SmtpClientRespository>();
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2).AddJsonOptions(
                options =>
                {
                    options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
                    options.SerializerSettings.NullValueHandling = NullValueHandling.Ignore;
                    // options.SerializerSettings.PreserveReferencesHandling = PreserveReferencesHandling.All;
                }
            );
            services.AddOData();
            services.AddAutoMapper();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler(builder =>
                {
                    builder.Run(async context =>
                    {
                        context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                        var exceptionHandlerFeature = context.Features.Get<IExceptionHandlerFeature>();
                        if (exceptionHandlerFeature != null)
                        {
                            context.Response.AddApplicationError(exceptionHandlerFeature.Error.Message);
                            await context.Response.WriteAsync(exceptionHandlerFeature.Error.Message);
                        }
                    });
                });
            }
            app.UseAuthentication();

            app.UseCors("CorsPolicy");
            // app.UseCors(x => x.AllowAnyHeader().AllowAnyMethod().WithOrigins("http://localhost:4200").AllowCredentials());
            // app.UseHttpsRedirection();

            app.UseSignalR(routes =>
            {
                routes.MapHub<NotificationHub>("/api/Notification");
            });
            // app.UseMvc();
            app.UseMvc(routeBuilder =>
            {
                // routeBuilder.EnableDependencyInjection();
                // routeBuilder.Expand().Filter().Count().OrderBy().Select().MaxTop(10);
                routeBuilder.MapSpaFallbackRoute(
                    name: "spa-fallback",
                    defaults: new { controller = "FallBack", action = "Index" }
                    );
            });


            app.UseStaticFiles();
            app.seed();
        }
    }
}
