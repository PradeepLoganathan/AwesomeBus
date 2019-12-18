using Amazon.SQS;
using AwesomeBus.MessageContracts;
using Microsoft.Extensions.Configuration;
using NServiceBus;
using System;
using System.Threading.Tasks;

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

            endpointConfiguration.UsePersistence<InMemoryPersistence>();
            endpointConfiguration.AuditProcessedMessagesTo("AwesomeBus-audit");
            endpointConfiguration.SendFailedMessagesTo("AwesomeBus-error");
            
            var routing = transport.Routing();
            routing.RouteToEndpoint(typeof(CreateCustomerCommand), "AwesomeBus.Subscriber");
            
            var endpointInstance = await Endpoint.Start(endpointConfiguration);
            #endregion

            #region sendcommand

            var createCustomerCommand = new CreateCustomerCommand
            {
                FirstName = "pradeep",
                LastName = "loganathan"
            };

            await endpointInstance.Send(createCustomerCommand);
            #endregion

            Console.WriteLine("Publisher Endpoint started ..... Press any key to exit");
            Console.ReadKey();
            await endpointInstance.Stop().ConfigureAwait(false);
            

        }
    }
}
