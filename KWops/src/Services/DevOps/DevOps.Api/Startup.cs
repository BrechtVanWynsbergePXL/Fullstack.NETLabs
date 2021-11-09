using Api;
using Api.Swagger;
using DevOps.Infrastructure;
using DevOps.Logic;
using DevOps.Logic.Events;
using HumanRelations.API.Filters;
using Logic.Events;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace DevOps.Api
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

            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "DevOps.Api", Version = "v1" });
            });

            services.AddDbContext<DevOpsContext>(options =>
            {
                string connectionString = Configuration["ConnectionString"];
                options.UseSqlServer(connectionString, sqlOptions =>
                {
                    sqlOptions.MigrationsAssembly(typeof(DevOpsContext).GetTypeInfo().Assembly.GetName().Name);
                    sqlOptions.EnableRetryOnFailure(maxRetryCount: 15, maxRetryDelay: TimeSpan.FromSeconds(30), errorNumbersToAdd: null);
                });
                #if DEBUG
                options.UseLoggerFactory(LoggerFactory.Create(loggingBuilder => loggingBuilder.AddDebug()));
                options.EnableSensitiveDataLogging();
                #endif
            });

            services.AddScoped<DevOpsDbInitializer>();
            services.AddScoped<DeveloperRepository>();
            services.AddScoped<TeamRepository>();
            services.AddScoped<ITeamService>();

            services.AddSingleton(provider => new ApplicationExceptionFilterAttribute(provider.GetRequiredService<ILogger<ApplicationExceptionFilterAttribute>>()));
            services.AddControllers(options => { options.Filters.AddService<ApplicationExceptionFilterAttribute>(); });
            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

            services.AddRabbitMQEventBus(Configuration);
            services.AddScoped<EmployeeHiredEventHandler>();

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(JwtBearerDefaults.AuthenticationScheme,
                options =>
                {
                    string identityUrl = Configuration.GetValue<string>("Urls:IdentityUrl");
                    options.Authority = identityUrl;
                    options.Audience = "devops";
                    options.RequireHttpsMetadata = false;
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = false
                    };
                });

            string identityUrlExternal = Configuration.GetValue<string>("Urls:IdentityUrlExternal");
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "DevOps.Api",
                    Version = "v1"
                });
                string securityScheme = "OpenID";
                var scopes = new Dictionary<string, string>
                {
                    {"devops.read", "DevOps API - Read access"},
                    {"manage", "Write access"}
                };
                c.AddSecurityDefinition(securityScheme, new OpenApiSecurityScheme
                {
                    Type = SecuritySchemeType.OAuth2,
                    Flows = new OpenApiOAuthFlows
                    {
                        AuthorizationCode = new OpenApiOAuthFlow
                        {
                            AuthorizationUrl = new Uri($"{identityUrlExternal} / connect / authorize"),
                            TokenUrl = new Uri($"{identityUrlExternal} / connect / token"),
                            Scopes = scopes
                        }
                    }
                });
                c.OperationFilter<AlwaysAuthorizeOperationFilter>(securityScheme, scopes.Keys.ToArray());
            });

            var readPolicy = new AuthorizationPolicyBuilder()
                .AddAuthenticationSchemes(JwtBearerDefaults.AuthenticationScheme)
                .RequireAuthenticatedUser()
                .RequireClaim("scope", "devops.read")
                .Build();
            services.AddSingleton(provider => new
            ApplicationExceptionFilterAttribute(provider.GetRequiredService<ILogger<ApplicationExceptionFilterAttribute>>()));
            services.AddControllers(options =>
            {
                options.Filters.AddService<ApplicationExceptionFilterAttribute>();
                options.Filters.Add(new AuthorizeFilter(readPolicy));
            });

            var writePolicy = new AuthorizationPolicyBuilder()
                .AddAuthenticationSchemes(JwtBearerDefaults.AuthenticationScheme)
                .RequireAuthenticatedUser()
                .RequireClaim("scope", "manage")
                .Build();
            services.AddAuthorization(options =>
            {
                options.AddPolicy("write", writePolicy);
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c =>
                {
                    c.SwaggerEndpoint("/swagger/v1/swagger.json", "DevOps.Apiv1");
                    c.OAuthClientId("swagger.devops");
                    c.OAuthUsePkce();
                });
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            AddEventBusSubscriptions(app);
        }

        private void AddEventBusSubscriptions(IApplicationBuilder app)
        {
            IEventBus eventBus = app.ApplicationServices.GetRequiredService<IEventBus>();
            eventBus.Subscribe<EmployeeHiredIntegrationEvent, EmployeeHiredEventHandler>();
        }
    }
}
