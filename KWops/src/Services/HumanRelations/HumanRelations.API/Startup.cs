using Api;
using HumanRelations.API.Filters;
using HumanRelations.Domain;
using HumanRelations.Infrastructure;
using HumanRelations.Logic;
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
using System.Threading.Tasks;

namespace HumanRelations.API
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
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "HumanRelations.API", Version = "v1" });
            });

            services.AddDbContext<HumanRelationsContext>(options =>
            {
                string connectionString = Configuration["ConnectionString"];
                options.UseSqlServer(connectionString);
#if DEBUG
                options.UseLoggerFactory(LoggerFactory.Create(loggingBuilder => loggingBuilder.AddDebug()));
                options.EnableSensitiveDataLogging();
#endif

                options.UseSqlServer(connectionString, sqlOptions =>
                {
                    sqlOptions.EnableRetryOnFailure(
                        maxRetryCount: 15,
                        maxRetryDelay: TimeSpan.FromSeconds(30),
                        errorNumbersToAdd: null);
                });
            });

            services.AddScoped<HumanRelationsDbInitializer>();
            services.AddScoped<IEmployeeRepository, EmployeeDbRepository>();
            services.AddScoped<IEmployeeService, EmployeeService>();
            services.AddSingleton<IEmployeeFactory, Employee.Factory>();

            services.AddSingleton(provider => new ApplicationExceptionFilterAttribute(provider.GetRequiredService<ILogger<ApplicationExceptionFilterAttribute>>()));
            services.AddControllers(options => { options.Filters.AddService<ApplicationExceptionFilterAttribute>(); });
            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

            services.AddRabbitMQEventBus(Configuration);

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, 
                options =>
                {
                string identityUrl = Configuration.GetValue<string>("Urls:IdentityUrl");
                options.Authority = identityUrl;
                options.Audience = "hr";
                options.RequireHttpsMetadata = false;
                options.TokenValidationParameters = 
                    new TokenValidationParameters
                    {
                        ValidateIssuer = false
                    };
                });

            var readPolicy = new AuthorizationPolicyBuilder().AddAuthenticationSchemes(JwtBearerDefaults.AuthenticationScheme)
                .RequireAuthenticatedUser()
                .RequireClaim("scope", "hr.read")
                .Build();
            services.AddSingleton(provider => newApplicationExceptionFilterAttribute(provider.GetRequiredService <ILogger<ApplicationExceptionFilterAttribute>>()));
            services.AddControllers(options =>
            {
                options.Filters.AddService<ApplicationExceptionFilterAttribute>();
                options.Filters.Add(new AuthorizeFilter(readPolicy));
            });

            var writePolicy = new AuthorizationPolicyBuilder().AddAuthenticationSchemes(JwtBearerDefaults.AuthenticationScheme)
                .RequireAuthenticatedUser()
                .RequireClaim("scope", "manage")
                .Build();
            services.AddAuthorization(options =>
            {
                options.AddPolicy("write", writePolicy);
            });
        }

        private object newApplicationExceptionFilterAttribute(ILogger<ApplicationExceptionFilterAttribute> logger)
        {
            throw new NotImplementedException();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "HumanRelations.API v1"));
            }

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
