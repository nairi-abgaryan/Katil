using System;
using System.Threading.Tasks;
using EasyNetQ.Consumer;
using EasyNetQ.Events;
using EasyNetQ.Logging;
using Polly;

namespace Katil.Messages.PollyHandler
{
    public class PollyHandlerRunner : HandlerRunner
    {
        private readonly ILog logger = LogProvider.For<PollyHandlerRunner>();
        private readonly Policy _policy;

        public PollyHandlerRunner(IConsumerErrorStrategy consumerErrorStrategy, Policy policy)
            : base(consumerErrorStrategy)
        {
            _policy = policy ?? throw new ArgumentNullException(nameof(policy));
        }

        public async override Task<AckStrategy> InvokeUserMessageHandlerAsync(ConsumerExecutionContext context)
        {
            if (logger.IsDebugEnabled())
            {
                logger.DebugFormat("Received message with receivedInfo={receivedInfo}", context.Info);
            }

            var ackStrategy = await InvokeUserMessageHandlerInternalAsync(context).ConfigureAwait(false);

            return (model, tag) =>
            {
                try
                {
                    return ackStrategy(model, tag);
                }
                catch (Exception exception)
                {
                    logger.Error(
                        exception,
                        "Unexpected exception when attempting to ACK or NACK, receivedInfo={receivedInfo}",
                        context.Info);
                }

                return AckResult.Exception;
            };
        }

        private async Task<AckStrategy> InvokeUserMessageHandlerInternalAsync(ConsumerExecutionContext context)
        {
            try
            {
                await _policy.ExecuteAsync(async () =>
                {
                    await context.UserHandler(context.Body, context.Properties, context.Info).ConfigureAwait(false);
                });
            }
            catch (Exception exception)
            {
                logger.Error(exception, "Consumer error strategy has failed");
                return AckStrategies.NackWithoutRequeue;
            }

            return AckStrategies.Ack;
        }
    }
}
