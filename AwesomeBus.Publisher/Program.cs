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
            string AssemblyName = System.Reflection.Assembly.GetExecutingAssembly().GetName().Name;
            Console.Title = AssemblyName;

            #region configuration
            IConfiguration Configuration = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddEnvironmentVariables()
                .AddCommandLine(args)
                .Build();
            #endregion          
           
            #region NServiceBus-Configuration
            var endpointConfiguration = new EndpointConfiguration("AwesomeBus.Publisher");
            endpointConfiguration.EnableInstallers();
            var transport = endpointConfiguration.UseTransport<SqsTransport>();
            
            
            transport.ClientFactory(() => Configuration.GetAWSOptions().CreateServiceClient<IAmazonSQS>());

            //endpointConfiguration.UsePersistence<InMemoryPersistence>();
            endpointConfiguration.AuditProcessedMessagesTo("AwesomeBus-audit");
            endpointConfiguration.SendFailedMessagesTo("AwesomeBus-error");
            endpointConfiguration.SendOnly();
            endpointConfiguration.UseSerialization<NewtonsoftSerializer>();

            //endpointConfiguration.SendHeartbeatTo(

            //        serviceControlQueue: "ServiceControl_Queue",
            //        frequency: TimeSpan.FromSeconds(15),
            //        timeToLive: TimeSpan.FromSeconds(30));

            var connection = @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=master;Database=NsbpubsubSqlOutbox;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
            var persistence = endpointConfiguration.UsePersistence<SqlPersistence>();
            persistence.ConnectionBuilder(
                connectionBuilder: () =>
                {
                    return new SqlConnection(connection);
                });
            var dialect = persistence.SqlDialect<SqlDialect.MsSqlServer>();
            dialect.Schema("senderpublisher");
            persistence.TablePrefix("");
            var subscriptions = persistence.SubscriptionSettings();
            subscriptions.DisableCache();

            var routing = transport.Routing();
            routing.RouteToEndpoint(typeof(CreateCustomerCommand), "AwesomeBus.CustomerEndpoint");
            routing.RouteToEndpoint(typeof(CreateOrderCommand), "AwesomeBus.OrderEndpoint");

            endpointConfiguration.EnableOutbox();
            var endpointInstance = await Endpoint.Start(endpointConfiguration).ConfigureAwait(false);
            #endregion

            #region CreateCustomerCommand


            //var createCustomerCommand = new CreateCustomerCommand
            //{
            //    FirstName = "test",
            //    LastName = "lastnametest"
            //};

            //await endpointInstance.Send(createCustomerCommand);            
            //WriteToConsole($"Fired {nameof(ICreateCustomerCommand)}", ConsoleColor.Yellow);
            #endregion

            #region CreateorderCommand
            var createOrderCommand = new CreateOrderCommand
            {
                OrderID = Guid.NewGuid(),
                OrderData = new OrderData() 
                { 
                    OrderDateTime = DateTime.UtcNow.ToLongDateString(), 
                    OrderItem = "Dell XPS", 
                    Quantity = 1, 
                    TotalPrice = 2356.18
                }
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
