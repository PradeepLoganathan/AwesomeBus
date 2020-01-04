using Amazon.SQS;
using AwesomeBus.MessageContracts;
using Microsoft.Extensions.Configuration;
using NServiceBus;
using NServiceBus.Persistence.Sql;
using System;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace AwesomeBus.CustomerWorker
{
    class Program
    {
        static async Task Main(string[] args)
        {
            Console.Title = "Awesome.Bus.CustomerWorker";

            #region configuration
            IConfiguration Configuration = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddEnvironmentVariables()
                .AddCommandLine(args)
                .Build();
            #endregion



            #region NServiceBusIntegration
            var connection = @"Data Source=.\SqlExpress;Database=NsbpubsubSqlOutbox;Integrated Security=True;Max Pool Size=100";
            var endpointConfiguration = new EndpointConfiguration("AwesomeBus.CustomerCommandQueue");
            endpointConfiguration.EnableInstallers();
            var transport = endpointConfiguration.UseTransport<SqsTransport>();

            

            transport.ClientFactory(() => Configuration.GetAWSOptions().CreateServiceClient<IAmazonSQS>());

            endpointConfiguration.UsePersistence<InMemoryPersistence>();
            endpointConfiguration.AuditProcessedMessagesTo("AwesomeBus-audit");
            endpointConfiguration.SendFailedMessagesTo("AwesomeBus-error");
            endpointConfiguration.UseSerialization<NewtonsoftSerializer>();

            var persistence = endpointConfiguration.UsePersistence<SqlPersistence>();
            persistence.ConnectionBuilder(
                connectionBuilder: () =>
                {
                    return new SqlConnection(connection);
                });
            var dialect = persistence.SqlDialect<SqlDialect.MsSqlServer>();
            dialect.Schema("customerworker");
            persistence.TablePrefix("");
            endpointConfiguration.EnableOutbox();


            var endpointInstance = await Endpoint.Start(endpointConfiguration);
            
            // Console.WriteLine("CustomerWorker Endpoint started ..... Press any key to exit");
            Console.ReadKey();
            await endpointInstance.Stop();
            #endregion

        }
    }
}
