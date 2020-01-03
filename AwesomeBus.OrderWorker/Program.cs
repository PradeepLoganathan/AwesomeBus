using Amazon.SQS;
using AwesomeBus.MessageContracts;
using Microsoft.Extensions.Configuration;
using NServiceBus;
using System;
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
            var endpointConfiguration = new EndpointConfiguration("AwesomeBus.OrderCommandQueue");
            endpointConfiguration.EnableInstallers();
            var transport = endpointConfiguration.UseTransport<SqsTransport>();

            

            transport.ClientFactory(() => Configuration.GetAWSOptions().CreateServiceClient<IAmazonSQS>());

            //endpointConfiguration.UsePersistence<InMemoryPersistence>();
            transport.DisablePublishing();
            endpointConfiguration.AuditProcessedMessagesTo("AwesomeBus-audit");
            endpointConfiguration.SendFailedMessagesTo("AwesomeBus-error");

            var endpointInstance = await Endpoint.Start(endpointConfiguration);

            // Console.WriteLine("second subscriber Endpoint started ..... Press any key to exit");
            Console.ReadKey();
            await endpointInstance.Stop();
            #endregion
        }
    }
}
