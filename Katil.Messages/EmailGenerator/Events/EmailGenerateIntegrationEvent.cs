using System;
using Katil.Common.Utilities;

namespace Katil.Messages.EmailGenerator.Events
{
    public class EmailGenerateIntegrationEvent : BaseMessage
    {
        public Guid DisputeGuid { get; set; }

        public EmailMessageType MessageType { get; set; }
    }
}
