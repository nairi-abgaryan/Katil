using System;
using EasyNetQ;
using EasyNetQ.AutoSubscribe;
using Katil.Data.Repositories.UnitOfWork;
using Katil.Messages.EmailGenerator.Events;
using Katil.Messages.EmailNotification.Events;

namespace Katil.Services.EmailGenerator.EmailGeneratorService.IntegrationEvents.EventHandling
{
    public class EmailGenerateIntegrationEventHandler : IConsumeAsync<EmailGenerateIntegrationEvent>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IBus _bus;

        public EmailGenerateIntegrationEventHandler(IBus bus, IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            _bus = bus;
        }

        [AutoSubscriberConsumer(SubscriptionId = "EmailGenerate")]
        public async System.Threading.Tasks.Task ConsumeAsync(EmailGenerateIntegrationEvent message)
        {
            Console.WriteLine("Email Generate Integration Event Received: {0}, {1}", message.DisputeGuid, message.MessageType);
            await _unitOfWork.UserRepository.GetAllAsync();
        }

        private void Publish(EmailNotificationIntegrationEvent message)
        {
            _bus.PublishAsync(message)
                .ContinueWith(task =>
                {
                    if (task.IsCompleted)
                    {
                        Console.WriteLine("Publish email generation event: {0} {1}", message.DisputeGuid, message.MessageType);
                    }
                    if (task.IsFaulted)
                    {
                        Console.Out.WriteLine("\n\n");
                        Console.Out.WriteLine(task.Exception);
                        Console.Out.WriteLine("\n\n");
                    }
                });
        }
    }
}
