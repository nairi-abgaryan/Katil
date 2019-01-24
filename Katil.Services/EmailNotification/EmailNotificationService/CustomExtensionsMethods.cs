using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Katil.Data.Model;
using Katil.Data.Repositories.AppUser;
using Katil.Data.Repositories.Token;
using Katil.Data.Repositories.UnitOfWork;
using Katil.Messages.PollyHandler;
using Katil.Services.EmailNotification.EmailNotificationService.IntegrationEvents.EventHandling;
using Katil.UserResolverService;
using Swashbuckle.AspNetCore.Swagger;

namespace Katil.Services.EmailNotification.EmailNotificationService
{
    public static class CustomExtensionsMethods
    {
        public static IServiceCollection AddRepositories(this IServiceCollection services)
        {
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<ITokenRepository, TokenRepository>();
            services.AddScoped<IUserRepository, UserRepository>();

            return services;
        }

        public static IServiceCollection AddCustomMvc(this IServiceCollection services)
        {
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
            return services;
        }

        public static IServiceCollection AddHealthChecks(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddHealthChecks(checks =>
            {
                var minutes = 1;
                if (int.TryParse(configuration["HealthCheck:Timeout"], out var minutesParsed))
                {
                    minutes = minutesParsed;
                }
            });

            return services;
        }

        public static IServiceCollection AddCustomDbContext(this IServiceCollection services, IConfiguration configuration)
        {
            // Postgres
            var connectionString = configuration.GetConnectionString("DbConnection");
            services.AddEntityFrameworkNpgsql()
                .AddDbContext<KatilContext>(c => c.UseNpgsql(connectionString, b => b.MigrationsAssembly("Katil.Data.Model")), ServiceLifetime.Scoped);

            return services;
        }

        public static IServiceCollection AddCustomSwagger(this IServiceCollection services, IConfiguration configuration)
        {
            // Register the Swagger generator, defining 1 or more Swagger documents
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Info { Title = "Email Generator API", Version = "v1" });
            });

            return services;
        }

        public static IServiceCollection AddEventBus(this IServiceCollection services, IConfiguration configuration)
        {
            services.RegisterEasyNetQ(
                configuration["MQ:Cluster"],
                registerServices => registerServices.UseMessageWaitAndRetryHandlerPolicy());

            return services;
        }

        public static IServiceCollection AddCustomIntegrations(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddSingleton<IUserResolver, UserResolver>();

            services.AddScoped<EmailNotificationIntegrationEventHandler, EmailNotificationIntegrationEventHandler>();

            return services;
        }
    }
}
