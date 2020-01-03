using NServiceBus;

namespace AwesomeBus.MessageContracts
{
    public interface ICreateCustomerCommand: ICommand
    {
         string FirstName { get; set; }
         string LastName { get; set; }
    }
}
