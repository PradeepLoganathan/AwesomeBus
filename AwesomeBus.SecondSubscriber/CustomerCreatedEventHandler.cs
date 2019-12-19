using NServiceBus;
using System;
using System.Threading.Tasks;
using AwesomeBus.MessageContracts;

namespace AwesomeBus.SecondSubscriber
{
    class CustomerCreatedEventHandler:IHandleMessages<CustomerCreatedEvent>
    {
        public Task Handle(CustomerCreatedEvent message, IMessageHandlerContext context)
        {
            Console.WriteLine($"recieved Customer created event with customerid {message.CustomerId}");
            return Task.CompletedTask;
        }
    }
}
