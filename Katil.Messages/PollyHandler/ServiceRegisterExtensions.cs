using System;
using EasyNetQ.Consumer;
using EasyNetQ.DI;
using Polly;

namespace Katil.Messages.PollyHandler
{
    public static class ServiceRegisterExtensions
    {
        public static IServiceRegister UseMessageHandlerPolicy(this IServiceRegister registrar, Policy policy)
        {
            if (registrar == null)
            {
                throw new ArgumentNullException(nameof(registrar));
            }

            if (policy == null)
            {
                throw new ArgumentNullException(nameof(policy));
            }

            registrar.Register<IHandlerRunner>(services => new PollyHandlerRunner(services.Resolve<IConsumerErrorStrategy>(), policy));

            return registrar;
        }

        public static IServiceRegister UseMessageWaitAndRetryHandlerPolicy(this IServiceRegister registrar)
        {
            var policy = Policy
                .Handle<Exception>()
                .WaitAndRetryAsync(3, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)));

            return UseMessageHandlerPolicy(registrar, policy);
        }
    }
}
