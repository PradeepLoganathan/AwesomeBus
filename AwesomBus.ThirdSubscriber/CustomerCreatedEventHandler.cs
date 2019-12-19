using AwesomeBus.MessageContracts;
using NServiceBus;
using System;
using System.Threading.Tasks;

using static AwesomeBus.Helper.HelperClass;

namespace AwesomBus.ThirdSubscriber
{
    class CustomerCreatedEventHandler : IHandleMessages<CustomerCreatedEvent>
    {
        public Task Handle(CustomerCreatedEvent message, IMessageHandlerContext context)
        {
            WriteToConsole($"Recieved ${nameof(CustomerCreatedEvent)} with customerid {message.CustomerId}", ConsoleColor.DarkMagenta);
            
            return Task.CompletedTask;
        }
    }
}
