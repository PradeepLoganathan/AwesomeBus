using AwesomeBus.MessageContracts;
using NServiceBus;
using System;
using System.Threading.Tasks;

namespace AwesomeBus.Subscriber
{
    class CustomerCreatedHandler : IHandleMessages<CustomerCreated>
    {
        public Task Handle(CustomerCreated message, IMessageHandlerContext context)
        {
            Console.WriteLine($"recieved Customer created message with customerid {message.CustomerId}");
            return Task.CompletedTask;
        }
    }
}
