using NServiceBus;

namespace AwesomeBus.MessageContracts
{
    public class CreateCustomerCommand: ICommand
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }
}
