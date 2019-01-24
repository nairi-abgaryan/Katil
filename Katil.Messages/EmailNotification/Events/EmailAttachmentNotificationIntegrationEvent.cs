using System;
using System.IO;
using Katil.Common.Utilities;

namespace Katil.Messages.EmailNotification.Events
{
    public class EmailAttachmentNotificationIntegrationEvent : BaseMessage
    {
        public AttachmentType Type { get; set; }

        public string FileName { get; set; }

        public string CommonFileName { get; set; }
    }
}
