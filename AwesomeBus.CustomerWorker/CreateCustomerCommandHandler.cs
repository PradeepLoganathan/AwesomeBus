using AwesomeBus.MessageContracts;
using NServiceBus;
using System;
using System.Threading.Tasks;

using static AwesomeBus.Helper.HelperClass;

namespace AwesomeBus.CustomerWorker
{
    class CreateCustomerCommandHandler : IHandleMessages<ICreateCustomerCommand>
    {
        public async Task Handle(ICreateCustomerCommand message, IMessageHandlerContext context)
        {
            WriteToConsole($"Handler fired for {nameof(ICreateCustomerCommand)} - {message.FirstName}", ConsoleColor.Green);

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
