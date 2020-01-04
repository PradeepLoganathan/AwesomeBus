using Amazon.SQS;
using AwesomeBus.MessageContracts;
using Microsoft.Extensions.Configuration;
using NServiceBus;
using NServiceBus.Persistence.Sql;
using System;
using System.Data.SqlClient;
using System.IO;
using System.Threading.Tasks;

namespace AwesomeBus.OrderWorker
{
    class Program
    {
        static async Task Main(string[] args)
        {
            Console.Title = "Awesome.Bus.Second.OrderWorker";

            #region configuration
            IConfiguration Configuration = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddEnvironmentVariables()
                .AddCommandLine(args)
                .Build();
            #endregion

            #region NServiceBusIntegration
            var connection = @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=master;Database=NsbpubsubSqlOutbox;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
            var endpointConfiguration = new EndpointConfiguration("AwesomeBus.OrderCommandQueue");
            endpointConfiguration.EnableInstallers();
            var transport = endpointConfiguration.UseTransport<SqsTransport>();

            

            transport.ClientFactory(() => Configuration.GetAWSOptions().CreateServiceClient<IAmazonSQS>());

            
            endpointConfiguration.AuditProcessedMessagesTo("AwesomeBus-audit");
            endpointConfiguration.SendFailedMessagesTo("AwesomeBus-error");

            var persistence = endpointConfiguration.UsePersistence<SqlPersistence>();
            persistence.ConnectionBuilder(
                connectionBuilder: () =>
                {
                    return new SqlConnection(connection);
                });
            var dialect = persistence.SqlDialect<SqlDialect.MsSqlServer>();
            dialect.Schema("orderworker");
            persistence.TablePrefix("");
            var subscriptions = persistence.SubscriptionSettings();
            subscriptions.DisableCache();

            endpointConfiguration.EnableOutbox();

           // SqlHelper.CreateSchema(connection, "orderworker");

            //SqlHelper.ExecuteSql(connection, File.ReadAllText("Startup.sql"));

            var endpointInstance = await Endpoint.Start(endpointConfiguration);

            // Console.WriteLine("second subscriber Endpoint started ..... Press any key to exit");
            Console.ReadKey();
            await endpointInstance.Stop();
            #endregion
        }
    }
}
