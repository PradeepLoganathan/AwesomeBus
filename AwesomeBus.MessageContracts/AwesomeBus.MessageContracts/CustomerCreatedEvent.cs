using NServiceBus;
using System;

namespace AwesomeBus.MessageContracts
{
    public class CustomerCreatedEvent:IEvent
    {
        public Guid CustomerId { get; set; }
    }
}
