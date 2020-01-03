using AwesomeBus.MessageContracts;

namespace AwesomeBus.Publisher
{
    public class CreateCustomerCommand : ICreateCustomerCommand
    {
        public string FirstName { get ; set ; }
        public string LastName { get ; set ; }
    }
}