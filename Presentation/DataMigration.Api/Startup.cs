using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Autofac;
using CourseStudio.Application.Common.Helpers;
using CourseStudio.Doamin.Models.Users;
using CourseStudio.Domain.Persistence;
using CourseStudio.Domain.TraversalModel.Identities;
using CourseStudio.Lib.Configs;
using CourseStudio.Lib.Utilities.Database;
using CourseStudio.Lib.BlobStorage;

namespace DataMigration.Api
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
			// Setting up CORS
            services.AddCors();

            // IoC registrate
            services.AddMvc();

            // remove file upload limit for 134217728 bytes
            services.Configure<FormOptions>(x =>
            {
                x.ValueLengthLimit = int.MaxValue;
                x.MultipartBodyLengthLimit = int.MaxValue; // In case of multipart
            });

            // Application Configs
            services.Configure<AppSettings>(Configuration.GetSection("AppSettings"));
			services.Configure<LocalConfigs>(Configuration.GetSection("Local"));
            services.Configure<ConnectionStrings>(Configuration.GetSection("ConnectionStrings"));
            services.Configure<StorageConnectionConfig>(Configuration.GetSection("StorageConnection"));
            services.Configure<TokenConfig>(Configuration.GetSection("Token"));
            services.Configure<PaymentProcessConfig>(Configuration.GetSection("PaymentProcess"));
            services.Configure<EmailConfigs>(Configuration.GetSection("Email"));

            // TODO : Best to move this in to Autofac IOC, and decouple the Application form Domain Model 
            // Identity services
            services.AddIdentity<ApplicationUser, IdentityRole>()
                .AddEntityFrameworkStores<CourseContext>()
                .AddDefaultTokenProviders();
            /*
             * Authentication Service
             * Two AuthorizationSchemes: Cookie-base & Token-base
             */
            services.AddAuthentication(options =>
            {
                // change defaut Authentication Method from Cookie-based to JWT Token-based 
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                // For stopping Token-based error 401/403 redirect to 404 by ASP.Net automaticly
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
                .AddCookie()
                .AddJwtBearer(cfg =>
                {
                    cfg.RequireHttpsMetadata = false;
                    cfg.SaveToken = true;
                    cfg.TokenValidationParameters = TokenHelper.GenerateTokenValidation(Configuration["Token:Key"], Configuration["Token:Issuer"], Configuration["Token:Audience"]);
                });

            // Authorization Service
            services.AddAuthorization(options =>
            {
              
                // setup JWT Token-base policy
                options.AddPolicy(ApplicationPolicies.AuthMethod.JwtTokenBase, policy =>
                {
                    policy.AddAuthenticationSchemes
                    (
                        JwtBearerDefaults.AuthenticationScheme,
                        CookieAuthenticationDefaults.AuthenticationScheme
                    ).RequireAuthenticatedUser()
                    .Build();
                });

                // setup JWT Claims policy
                // Roles
                // Roles
                options.AddPolicy(ApplicationPolicies.DefaultRoleRequirements.Student, policy => policy.RequireRole(ApplicationPolicies.DefaultRoles.Student));
                options.AddPolicy(ApplicationPolicies.DefaultRoleRequirements.Tutor, policy => policy.RequireRole(ApplicationPolicies.DefaultRoles.Tutor));
                options.AddPolicy(ApplicationPolicies.DefaultRoleRequirements.Staff, policy => policy.RequireRole(ApplicationPolicies.DefaultRoles.Staff));
                options.AddPolicy(ApplicationPolicies.DefaultRoleRequirements.Root, policy => policy.RequireRole(ApplicationPolicies.DefaultRoles.Root));
            });

            // For stopping 401/403 redirect to 404 by ASP.Net automaticly
            services.ConfigureApplicationCookie(options =>
            {
                options.Cookie.Name = Configuration["AppSettings:Title"];
                options.Events.OnRedirectToLogin = context =>
                {
                    context.Response.StatusCode = 401;
                    return Task.CompletedTask;
                };
                options.Events.OnRedirectToAccessDenied = context =>
                {
                    context.Response.StatusCode = 403;
                    return Task.CompletedTask;
                };
            });

            // register UrlHelper into IOC
            // 1.Action方法通过提供Controller,Action和各种参数生成一个URL，
            // 2.Content方法是将一个虚拟的，相对的路径转换到应用程序的绝对路径，
            // 3.Encode方法是对URL地址进行加密，与Server.Encode方法一样。
            // 4.方法是提供在当前应用程序中规定的路由规则中匹配出URL。
            services.AddSingleton<IActionContextAccessor, ActionContextAccessor>();
            services.AddScoped<IUrlHelper, UrlHelper>(implementationFactory =>
            {
                var actionContext = implementationFactory.GetService<IActionContextAccessor>().ActionContext;
                return new UrlHelper(actionContext);
            });
        }

		public void ConfigureContainer(ContainerBuilder builder)
        {
            builder.Register(r => new MssqlDbConnectionHelper(Configuration["ConnectionStrings:CourseDB"])).As<IDatabaseConnectionHelper>();
            builder.RegisterModule(new RegisterModule());
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
                app.UseExceptionHandler();
            }

            // Allow Cors for all
            // TODO: Security problem, limit the cors to certain domain in the future
            app.UseCors(builder => builder.AllowAnyOrigin()
                        .AllowAnyMethod()
                        .AllowAnyHeader()
                        .AllowCredentials()
                       );

            /* 
             * invoke IdentityUser 
             * must invoke before "app.UseMvc()"
             */
            app.UseAuthentication();

            /*
             * StatusCodePagesMiddleware: for Error Handling
             * for providing a rich status code page for HTTP status codes such as 500 (Internal Server Error) or 404 (Not Found)
            */
            app.UseStatusCodePages();

            // ASP.Net Routing Middleware
            app.UseMvc();
        }
    }
}
