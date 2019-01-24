using System;
using System.Threading.Tasks;
using EasyNetQ.AutoSubscribe;
using Microsoft.Extensions.DependencyInjection;

namespace Katil.Messages
{
    public class DependencyInjectionMessageDispatcher : IAutoSubscriberMessageDispatcher
    {
        private readonly IServiceProvider resolver;

        public DependencyInjectionMessageDispatcher(IServiceProvider resolver)
        {
            this.resolver = resolver;
        }

        public void Dispatch<TMessage, TConsumer>(TMessage message)
            where TMessage : class
            where TConsumer : class, IConsume<TMessage>
        {
            using (var scope = resolver.CreateScope())
            {
                var consumer = resolver.GetService<TConsumer>();
                consumer.Consume(message);
            }
        }

        public async Task DispatchAsync<TMessage, TAsyncConsumer>(TMessage message)
            where TMessage : class
            where TAsyncConsumer : class, IConsumeAsync<TMessage>
        {
            using (var scope = resolver.CreateScope())
            {
                var asyncConsumer = resolver.GetService<TAsyncConsumer>();
                await asyncConsumer.ConsumeAsync(message).ConfigureAwait(false);
            }
        }
    }
}
