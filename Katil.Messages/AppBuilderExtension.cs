using System;
using System.Reflection;
using EasyNetQ;
using EasyNetQ.AutoSubscribe;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;

namespace Katil.Messages
{
    public static class AppBuilderExtension
    {
        public static IBus GetBus(this IApplicationBuilder appBuilder)
        {
            var services = appBuilder.ApplicationServices.CreateScope().ServiceProvider;

            var lifeTime = services.GetService<IApplicationLifetime>();
            return services.GetService<IBus>();
        }

        public static IApplicationBuilder UseRespond(this IApplicationBuilder appBuilder)
        {
            var services = appBuilder.ApplicationServices.CreateScope().ServiceProvider;

            var lifeTime = services.GetService<IApplicationLifetime>();
            var bus = services.GetService<IBus>();
            return appBuilder;
        }

        public static IApplicationBuilder UseSubscribe(this IApplicationBuilder appBuilder, string subscriptionIdPrefix, Assembly assembly)
        {
            var services = appBuilder.ApplicationServices.CreateScope().ServiceProvider;

            var lifeTime = services.GetService<IApplicationLifetime>();
            var bus = services.GetService<IBus>();
            lifeTime.ApplicationStarted.Register(() =>
            {
                var subscriber = new AutoSubscriber(bus, subscriptionIdPrefix);
                subscriber.Subscribe(assembly);
                subscriber.SubscribeAsync(assembly);
            });

            lifeTime.ApplicationStopped.Register(() => bus.Dispose());

            return appBuilder;
        }

        public static IApplicationBuilder UseAutoSubscribe(this IApplicationBuilder appBuilder, string subscriptionIdPrefix, Assembly assembly)
        {
            var services = appBuilder.ApplicationServices.CreateScope().ServiceProvider;

            var lifeTime = services.GetService<IApplicationLifetime>();
            var bus = services.GetService<IBus>();
            lifeTime.ApplicationStarted.Register(() =>
            {
                var subscriber = new AutoSubscriber(bus, subscriptionIdPrefix)
                {
                    AutoSubscriberMessageDispatcher = new DependencyInjectionMessageDispatcher(services)
                };

                subscriber.Subscribe(assembly);
                subscriber.SubscribeAsync(assembly);
            });

            lifeTime.ApplicationStopped.Register(() => bus.Dispose());

            return appBuilder;
        }
    }
}
