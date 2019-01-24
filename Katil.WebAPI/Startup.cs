using System;
using Katil.Data.Model;
using Katil.Scheduler.Task.Infrastructure;
using Katil.Scheduler.Task.Jobs;
using Katil.WebAPI.WebApiHelpers;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Quartz;
using Serilog;

namespace Katil.WebAPI
{
    public class Startup
    {
        public Startup(IConfiguration configuration, IHostingEnvironment hostingEnvironment)
        {
            Configuration = configuration;

            var builder = new ConfigurationBuilder()
                .SetBasePath(hostingEnvironment.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile("secrets/appsettings.json", optional: true)
                .AddEnvironmentVariables();

            Configuration = builder.Build();
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services
                .AddRepositories()
                .AddCustomIntegrations(Configuration)
                .AddCustomMvc()
                .AddCustomCors(Configuration)
                .AddMapper()
                .AddCustomDbContext(Configuration)
                .AddCustomSwagger(Configuration)
                .AddScheduler(Configuration)
                .AddEventBus(Configuration);

            services.AddAuthentication(options =>
                {
                    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                })
                .AddJwtBearer(options =>
                {
                    options.Audience = "https://localhost:5001/";
                    options.Authority = "https://localhost:5000/";
                    options.RequireHttpsMetadata = false;
                });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddSerilog();
            using (var serviceScope = app.ApplicationServices.GetService<IServiceScopeFactory>().CreateScope())
            {
                var logger = loggerFactory.CreateLogger("DB Infos");
                logger.LogInformation("DB connection string {0}, {1}", Configuration.GetConnectionString("DbConnection"), env);

                var context = serviceScope.ServiceProvider.GetRequiredService<KatilContext>();

                context.Database.Migrate();
            }

            app.UseErrorWrappingMiddleware();

            ////ConfigureScheduler(app);

            app.UseCors("AllowCors");

            app.UseMvc();
            app.UseMvcWithDefaultRoute();

            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.DocExpansion(DocExpansion.None);
                c.SwaggerEndpoint("../swagger/v1/swagger.json", "V1");
            });
        }

        private void ConfigureScheduler(IApplicationBuilder app)
        {
            var scheduler = app.ApplicationServices.GetService<IScheduler>();
            scheduler.StartScheduledJob<MailNotificationJob>(Configuration["Scheduler:MailNotificationCronSchedule"]);
        }
    }
}
