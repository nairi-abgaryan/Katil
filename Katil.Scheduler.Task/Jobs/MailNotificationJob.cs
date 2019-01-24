using System;
using Katil.Scheduler.Task.Infrastructure;
using Microsoft.Extensions.DependencyInjection;
using Quartz;
using Serilog;

namespace Katil.Scheduler.Task.Jobs
{
    [DisallowConcurrentExecution]
    public class MailNotificationJob : JobBase
    {
        public const string Identity = "mail-notification-job";

        public MailNotificationJob(IServiceScopeFactory serviceScopeFactory)
        {
            ServiceScopeFactory = serviceScopeFactory;
        }

        private IServiceScopeFactory ServiceScopeFactory { get; set; }
    }
}
