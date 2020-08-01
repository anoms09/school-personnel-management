using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using DbUp;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using School.Personnel.Management.Interfaces;
using School.Personnel.Management.Repositories.AppAdmin;
using School.Personnel.Management.Repositories.Miscellaneous;
using School.Personnel.Management.Repositories.Staff;
using School.Personnel.Management.Util;
using Serilog;
using Swashbuckle.AspNetCore.SwaggerUI;

namespace school_personnel_management
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
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "School Personnel Management Service",
                    Version = "v1",
                    Description = "Manage Personnel for school",

                });
            });

            services.Configure<ApiBehaviorOptions>(options =>
            {
                options.SuppressModelStateInvalidFilter = true;
            });

            SetupDependencies(services);
        }

        private void SetupDependencies(IServiceCollection services)
        {
            services.AddTransient<PermissionRepository>();      
            services.AddTransient<FacultyRepository>();            
            services.AddSingleton<IAppConfiguration, AppConfiguration>();            
            PerformScriptUpdate();
        }


        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            app.UseForwardedHeaders(new ForwardedHeadersOptions
            {
                ForwardedHeaders = ForwardedHeaders.All,
                RequireHeaderSymmetry = false,
                ForwardLimit = null
            });
            app.UseCors(options => options.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod());
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
                app.UseHsts();

            app.UseSwagger();

            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "School Personnel Management v1.0");
                c.RoutePrefix = "api-docs";
                c.DocExpansion(DocExpansion.None);
            });

            loggerFactory.AddSerilog();
            app.UseHttpsRedirection();
            app.UseMvc();
        }

        private void PerformScriptUpdate()
        {
            var connString = Configuration["ConnectionStrings:PersonnelDbConnectionString"];
            var upgraderTran = DeployChanges.To
                .SqlDatabase(connString)
                .WithScriptsEmbeddedInAssembly(Assembly.GetExecutingAssembly(),
                    name => name.StartsWith("School.Personnel.Management.Scripts"))
                .LogToConsole()
                .Build();

            var resultEnt = upgraderTran.PerformUpgrade();
            if (!resultEnt.Successful)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(resultEnt.Error);
                Console.ResetColor();
            }
        }
    }
}
