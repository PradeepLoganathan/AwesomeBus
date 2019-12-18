using Amazon;
using Amazon.Runtime;
using Amazon.Runtime.CredentialManagement;
using Amazon.SQS;
using Amazon.SQS.Model;
using Microsoft.Extensions.Configuration;
using NServiceBus;
using System;
using System.Threading.Tasks;

namespace AwesomeBus
{
    class Program
    {
        static async Task Main(string[] args)
        {
            Console.Title = "Awesome.Bus";

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
            var endpointInstance = await Endpoint.Start(endpointConfiguration);
            Console.WriteLine("Endpoint started ..... Press any key to exit");
            Console.ReadKey();
            await endpointInstance.Stop().ConfigureAwait(false);
            #endregion

        }
    }
}
