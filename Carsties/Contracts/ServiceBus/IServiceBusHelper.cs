namespace Contracts.ServiceBus
{
    public interface IServiceBusHelper
    {
        Task SendEventToServiceBus<T>(Dto newAuction);
        Task SendEventToServiceBus<T>(ContractEntity auction);
    }
}