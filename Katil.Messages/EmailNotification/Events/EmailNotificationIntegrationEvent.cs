using System;
using System.Collections.Generic;

namespace Katil.Messages.EmailNotification.Events
{
    public class EmailNotificationIntegrationEvent : BaseMessage
    {
        public Guid DisputeGuid { get; set; }

        public byte MessageType { get; set; }

        public string EmailTo { get; set; }

        public string EmailFrom { get; set; }

        public string Title { get; set; }

        public string Subject { get; set; }

        public string Body { get; set; }

        public DateTime? PreferedSendDate { get; set; }

        public byte Retries { get; set; }

        public virtual ICollection<EmailAttachmentNotificationIntegrationEvent> EmailAttachments { get; set; }
    }
}
