using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Katil.Messages;
using Katil.Messages.Pdf.Events;
using Katil.Services.PdfConvertor.PdfService.IntegrationEvents.EventHandling;

namespace Katil.Services.PdfConvertor.PdfService
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

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services
            .AddCustomMvc()
            .AddCustomSwagger(Configuration)
            .AddEventBus(Configuration);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Pdf Generator API V1");
            });

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHsts();
            }

            app.UseMvc();
            ConfigureResponds(app);
        }

        private void ConfigureResponds(IApplicationBuilder app)
        {
            var bus = app.GetBus();

            bus.Respond<PdfDocumentGenerateIntegrationEvent, PdfFileGeneratedIntegrationEvent>(request =>
            {
                return PdfFileGenerateIntegrationEventHandler.ConsumeAsync(request);
            });
        }
    }
}
