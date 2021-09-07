using System;
using AutoMapper;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using net_core_backend.Context;
using net_core_backend.Models;
using net_core_backend.Services;
using net_core_backend.Services.Interfaces;
using net_core_backend.Profiles;
using Microsoft.OpenApi.Models;
using net_core_backend.Helpers;
using WebApi.Helpers;
using AutoWrapper;

namespace net_core_backend
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
            services.AddCors(options => options.AddPolicy("AllowAll", p => p.AllowAnyOrigin()
                                                            .AllowAnyMethod()
                                                             .AllowAnyHeader()));
            services.AddAutoMapper(c => c.AddProfile<AutoMapping>(), typeof(Startup));

            services.Configure<AppSettings>(Configuration.GetSection("AppSettings"));


            services.AddDbContext<OneBlinqDBContext>(options =>
            {
                options.UseSqlServer(Configuration.GetConnectionString("SQLCONNSTR_Database"));
            });


            //Comment this when you've setup a database connection string
            services.AddSingleton<IContextFactory>(new ContextFactoryTesting(Configuration.GetConnectionString("Testing_SQLCONNSTR_Database")));
            
            //Uncomment this when you've setup a database connection string
            //services.AddSingleton<IContextFactory>(new ContextFactory(Configuration.GetConnectionString("SQLCONNSTR_Database")));

            services.AddSingleton<IExampleService, ExampleService>();
            
            services.AddSingleton<IAccountService, AccountService>();

            services.AddHttpContextAccessor();

            services.AddHttpClient();

            services.AddControllers().AddNewtonsoftJson(options => options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore);

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "My API", Version = "v1" });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseCors("AllowAll");
            app.UseApiResponseAndExceptionWrapper();
            app.UseSwagger();

            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
            });

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            //app.UseAuthentication();

            //app.UseAuthorization();

            app.UseMiddleware<JwtMiddleware>();


            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
