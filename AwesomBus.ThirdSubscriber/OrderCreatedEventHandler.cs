using AwesomeBus.MessageContracts;
using NServiceBus;
using System;
using System.Threading.Tasks;
using static AwesomeBus.Helper.HelperClass;

namespace AwesomBus.ThirdSubscriber
{
    class OrderCreatedEventHandler : IHandleMessages<OrderCreatedEvent>
    {
        public Task Handle(OrderCreatedEvent message, IMessageHandlerContext context)
        {
            WriteToConsole($"recieved Order created event with Order ID {message.OrderID}", ConsoleColor.DarkCyan);
            return Task.CompletedTask;
        }
    }
}
