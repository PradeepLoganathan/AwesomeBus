using Amazon.SQS;
using AwesomeBus.MessageContracts;
using Microsoft.Extensions.Configuration;
using NServiceBus;
using NServiceBus.Persistence.Sql;
using System;
using System.Data.SqlClient;
using System.Threading.Tasks;

using static AwesomeBus.Helper.HelperClass;

namespace AwesomeBus.Publisher
{
    class Program
    {
        static async Task Main(string[] args)
        {
            Console.Title = "Awesome.Bus.Publisher";

            #region configuration
            IConfiguration Configuration = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddEnvironmentVariables()
                .AddCommandLine(args)
                .Build();
            #endregion

            
           

            #region NServiceBusIntegration
            var endpointConfiguration = new EndpointConfiguration("AwesomeBus.Publisher");
            endpointConfiguration.EnableInstallers();
            var transport = endpointConfiguration.UseTransport<SqsTransport>();
            
            
            transport.ClientFactory(() => Configuration.GetAWSOptions().CreateServiceClient<IAmazonSQS>());

            //endpointConfiguration.UsePersistence<InMemoryPersistence>();
            endpointConfiguration.AuditProcessedMessagesTo("AwesomeBus-audit");
            endpointConfiguration.SendFailedMessagesTo("AwesomeBus-error");
            endpointConfiguration.SendOnly();
            endpointConfiguration.UseSerialization<NewtonsoftSerializer>();

            var connection = @"Data Source=.\SqlExpress;Database=NsbpubsubSqlOutbox;Integrated Security=True;Max Pool Size=100";
            var persistence = endpointConfiguration.UsePersistence<SqlPersistence>();
            persistence.ConnectionBuilder(
                connectionBuilder: () =>
                {
                    return new SqlConnection(connection);
                });
            var dialect = persistence.SqlDialect<SqlDialect.MsSqlServer>();
            dialect.Schema("sender");
            persistence.TablePrefix("");

            var routing = transport.Routing();
            routing.RouteToEndpoint(typeof(CreateCustomerCommand), "AwesomeBus.CustomerCommandQueue");
            routing.RouteToEndpoint(typeof(CreateOrderCommand), "AwesomeBus.OrderCommandQueue");

            endpointConfiguration.EnableOutbox();
            var endpointInstance = await Endpoint.Start(endpointConfiguration);
            #endregion

            #region sendcommand

            await Task.Delay(2000);

            var createCustomerCommand = new CreateCustomerCommand
            {
                FirstName = "test",
                LastName = "lastnametest"
            };

            await endpointInstance.Send(createCustomerCommand);            
            WriteToConsole($"Fired {nameof(ICreateCustomerCommand)}", ConsoleColor.Yellow);

            var createOrderCommand = new CreateOrderCommand
            {
                OrderID = Guid.NewGuid(),
                OrderDate = DateTime.UtcNow
            };

            await endpointInstance.Send(createOrderCommand);
            WriteToConsole($"Fired {nameof(CreateOrderCommand)}", ConsoleColor.Yellow);
            #endregion

            WriteToConsole("Publisher Endpoint started ..... Press any key to exit", ConsoleColor.Yellow);
            Console.ReadKey();


            await endpointInstance.Stop().ConfigureAwait(false);
        }
    }
}
