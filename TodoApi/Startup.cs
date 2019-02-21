using System;
using System.IO;
using System.Text;
using System.Reflection;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Swashbuckle.AspNetCore.Swagger;
using AutoMapper;
using TodoApi.Helpers;
using TodoApi.Middleware;
using TodoApi.Services;

namespace TodoApi
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
            // Adds db context to the dependency injection container
            // Specify that the db context uses a db in memory
            //services.AddDbContext<TodoContext>(opt =>
            //    opt.UseInMemoryDatabase("TodoList"));
            services.AddDbContext<DataContext>(options =>
                options.UseNpgsql(Configuration.GetConnectionString("DefaultConnection")));

            // Adds memory caching
            services.AddMemoryCache();

            services.AddResponseCompression();

            // Define .NET Core compatibility version
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);

            services.AddAutoMapper(typeof(Startup));

            // configure strongly typed settings objects
            var appSettingsSection = Configuration.GetSection("AppSettings");
            services.Configure<AppSettings>(appSettingsSection);

            // configure JWT authentication
            var appSettings = appSettingsSection.Get<AppSettings>();
            var key = Encoding.ASCII.GetBytes(appSettings.Secret);
            services.AddAuthentication(x =>
            {
                //  AuthenticateAsync() will use this scheme, and also the AuthenticationMiddleware added by
                // UseAuthentication() will use this scheme to set context.User automatically.
                // (Corresponds to AutomaticAuthentication)
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                // ChallengeAsync() will use this scheme, [Authorize] with policies that
                // don't specify schemes will also use this
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(x =>
            {
                x.RequireHttpsMetadata = false;
                x.SaveToken = true;
                x.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidateAudience = false
                };
            });

            // configure DI for application services
            services.AddScoped<IUserService, UserService>();

            // Register the Swagger generator, defining 1 or more Swagger documents
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Info
                {
                    Version = "v1",
                    Title = "My API",
                    Description = "A simple example ASP.NET Core Web API"
                });

                // Set the comments path for the Swagger JSON and UI.
                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                c.IncludeXmlComments(xmlPath);
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            // Enable middleware to serve generated Swagger as a JSON endpoint.
            app.UseSwagger();

            // Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.), 
            // specifying the Swagger JSON endpoint.
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
            });

            if (env.IsDevelopment())
            {
                // When the app runs in the Development environment:
                //   Use the Developer Exception Page to report app runtime errors.
                //   Use the Database Error Page to report database runtime errors.
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
            }
            else
            {
                // When the app doesn't run in the Development environment:
                //   Enable the Exception Handler Middleware to catch exceptions
                //     thrown in the following middlewares.
                //   Use the HTTP Strict Transport Security Protocol (HSTS)
                //     Middleware.
                app.UseExceptionHandler("/Error");
                app.UseHsts();
            }

            // Use HTTPS Redirection Middleware to redirect HTTP requests to HTTPS.
            app.UseHttpsRedirection();

            // Use Cookie Policy Middleware to conform to EU General Data 
            // Protection Regulation (GDPR) regulations.
            // app.UseCookiePolicy();

            // Authenticate before the user accesses secure resources.
            app.UseAuthentication();

            // Runs custom Middleware
            //app.UseCustomMiddleware(new CustomMiddlewareOptions { DisplayBefore = true });
            //app.UseCustomMiddleware(new CustomMiddlewareOptions { DisplayAfter = false });

            // Compresses app responses
            app.UseResponseCompression();

            // Add MVC to the request pipeline.
            app.UseMvc();
        }
    }
}
