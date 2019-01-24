using AutoMapper;
using Katil.Business.Services.Files;
using Katil.Business.Services.TokenServices;
using Katil.Business.Services.UserServices;
using Katil.Data.Model;
using Katil.Data.Repositories.AppUser;
using Katil.Data.Repositories.Token;
using Katil.Data.Repositories.UnitOfWork;
using Katil.Messages.PollyHandler;
using Katil.Scheduler.Task.Infrastructure;
using Katil.Scheduler.Task.Jobs;
using Katil.UserResolverService;
using Katil.WebAPI.Filters;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Swashbuckle.AspNetCore.Swagger;

namespace Katil.WebAPI
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
            services.AddMvc();

            return services;
        }

        public static IServiceCollection AddMapper(this IServiceCollection services)
        {
            Mapper.Reset();
            services.AddAutoMapper();

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
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Info { Title = "Katil", Version = "v1" });
                c.DescribeAllEnumsAsStrings();
            });

            services.ConfigureSwaggerGen(options =>
            {
                options.OperationFilter<AuthenticationHeaderParametersOperationFilter>();
                options.OperationFilter<ConcurrencyCheckHeaderParameter>();
                options.OperationFilter<FileOperationFilter>();
            });
            return services;
        }

        public static IServiceCollection AddCustomCors(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddCors(
                options => options.AddPolicy(
                    "AllowCors",
                    builder =>
                    {
                        builder
                            .AllowAnyOrigin()
                            .AllowCredentials()
                            .AllowAnyMethod()
                            .AllowAnyHeader()
                            .WithExposedHeaders("Token", "Content-Type", "Authorization");
                    }));

            return services;
        }

        public static IServiceCollection AddScheduler(this IServiceCollection services, IConfiguration configuration)
        {
            services.UseQuartz(new[] { typeof(MailNotificationJob) });

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

            services.AddScoped<ITokenService, TokenService>();
            services.AddScoped<IAuthenticateService, AuthenticateService>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IFileService, FileService>();

            return services;
        }
    }
}
