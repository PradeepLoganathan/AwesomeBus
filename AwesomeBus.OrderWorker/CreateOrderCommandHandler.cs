using AwesomeBus.MessageContracts;
using Newtonsoft.Json;
using NServiceBus;
using System;
using System.Data.SqlClient;
using System.Threading.Tasks;

using static AwesomeBus.Helper.HelperClass;

namespace AwesomeBus.OrderWorker
{
    class CreateOrderCommandHandler : IHandleMessages<CreateOrderCommand>
    {
        public async Task Handle(CreateOrderCommand message, IMessageHandlerContext context)
        {   
            WriteToConsole($"Handler fired for {nameof(CreateOrderCommand)} {message.OrderID}", ConsoleColor.DarkBlue);

            #region database-updates
            //write the order to the database
            try
            {
                var session = context.SynchronizedStorageSession.SqlPersistenceSession();
                var OrderData = JsonConvert.SerializeObject(message.OrderData);
                var sql = @"insert into orderworker.SubmittedOrder
                                    (Id, Orders)
                        values      (@Id, @OrderDate)";
                using (var command = new SqlCommand(
                    cmdText: sql,
                    connection: (SqlConnection)session.Connection,
                    transaction: (SqlTransaction)session.Transaction))
                {
                    var parameters = command.Parameters;
                    parameters.AddWithValue("Id", message.OrderID.ToString());
                    parameters.AddWithValue("OrderDate", OrderData);
                    await command.ExecuteNonQueryAsync()
                        .ConfigureAwait(false);
                }
            }
            catch (Exception e)
            {
                
            }

            //throw new Exception();
            #endregion

            #region fireevent
            //respond back with the order accepted message

            //fire the order created event

            OrderCreatedEvent orderCreatedEvent = new OrderCreatedEvent
            {
                OrderID = Guid.NewGuid()
            };
            
            await context.Publish(orderCreatedEvent);

            WriteToConsole($"Event {nameof(OrderCreatedEvent)} published", ConsoleColor.DarkBlue);
            #endregion fireevent

            #region reply-orderaccepted
            #endregion
        }
    }
}
