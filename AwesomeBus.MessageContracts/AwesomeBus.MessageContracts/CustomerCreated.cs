using NServiceBus;
using System;

namespace AwesomeBus.MessageContracts
{
    public class CustomerCreated:IEvent
    {
        public Guid CustomerId { get; set; }
    }
}
