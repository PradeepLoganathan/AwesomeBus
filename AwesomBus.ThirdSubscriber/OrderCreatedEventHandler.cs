//using AwesomeBus.MessageContracts;
//using NServiceBus;
//using System;
//using System.Threading.Tasks;

//namespace AwesomBus.ThirdSubscriber
//{
//    class OrderCreatedEventHandler : IHandleMessages<OrderCreatedEvent>
//    {
//        public Task Handle(OrderCreatedEvent message, IMessageHandlerContext context)
//        {
//            // Console.WriteLine($"recieved Order created event with Order ID {message.OrderID}");
//            return Task.CompletedTask;
//        }
//    }
//}
