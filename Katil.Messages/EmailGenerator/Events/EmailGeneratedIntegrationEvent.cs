using System;

namespace Katil.Messages.EmailGenerator.Events
{
    public class EmailGeneratedIntegrationEvent : BaseMessage
    {
        public Guid DisputeGuid { get; set; }

        public string FileContentBase64 { get; set; }
    }
}
