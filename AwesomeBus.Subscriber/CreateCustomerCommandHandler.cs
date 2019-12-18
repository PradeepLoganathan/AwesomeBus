using AwesomeBus.MessageContracts;
using NServiceBus;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AwesomeBus.Subscriber
{
    class CreateCustomerCommandHandler : IHandleMessages<CreateCustomerCommand>
    {
        public async Task Handle(CreateCustomerCommand message, IMessageHandlerContext context)
        {
            Console.WriteLine($"Create customer command handler {message.FirstName}");

            var customerCreated = new CustomerCreatedEvent
            {
                CustomerId = Guid.NewGuid()
            };

            await context.Publish(customerCreated);            
        }
    }
}
