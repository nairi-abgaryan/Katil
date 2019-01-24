using System;

namespace Katil.Messages
{
    public class BaseMessage
    {
        public Guid MessageGuid { get; set; }

        public DateTime CreatedDateTime { get; set; }
    }
}
