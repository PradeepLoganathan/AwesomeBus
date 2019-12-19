using AwesomeBus.MessageContracts;
using NServiceBus;
using System;
using System.Threading.Tasks;

using static AwesomeBus.Helper.HelperClass;

namespace AwesomeBus.CustomerWorker
{
    class CreateCustomerCommandHandler : IHandleMessages<CreateCustomerCommand>
    {
        public async Task Handle(CreateCustomerCommand message, IMessageHandlerContext context)
        {
            WriteToConsole($"Handler fired for {nameof(CreateCustomerCommand)} - {message.FirstName}", ConsoleColor.Green);

            var customerCreated = new CustomerCreatedEvent
            {
                CustomerId = Guid.NewGuid()
            };

            await Task.Delay(0);

            await context.Publish(customerCreated);

            WriteToConsole($"Event {nameof(CustomerCreatedEvent)} published", ConsoleColor.Green);
        }
    }
}
