using AwesomeBus.MessageContracts;
using NServiceBus;
using System;
using System.Threading.Tasks;

using static AwesomeBus.Helper.HelperClass;

namespace AwesomeBus.OrderWorker
{
    class CreateOrderCommandHandler : IHandleMessages<CreateOrderCommand>
    {
        public async Task Handle(CreateOrderCommand message, IMessageHandlerContext context)
        {   
            WriteToConsole($"Handler fired for {nameof(CreateOrderCommand)} {message.OrderID}", ConsoleColor.DarkBlue);

            OrderCreatedEvent orderCreatedEvent = new OrderCreatedEvent
            {
                OrderID = Guid.NewGuid()
            };
            
            await context.Publish(orderCreatedEvent);

            WriteToConsole($"Event {nameof(OrderCreatedEvent)} published", ConsoleColor.DarkBlue);
        }
    }
}
