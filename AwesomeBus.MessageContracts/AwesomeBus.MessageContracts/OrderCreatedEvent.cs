using NServiceBus;
using System;

namespace AwesomeBus.MessageContracts
{
    public class OrderCreatedEvent:IEvent
    {
        public Guid OrderID { get; set; }
    }
}
