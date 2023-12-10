using AutoMapper;
using MassTransit;

namespace Contracts.ServiceBus
{
    public class ServiceBusHelper : IServiceBusHelper
    {
        private readonly IPublishEndpoint _publishEndpoint;
        private readonly IMapper _mapper;

        public ServiceBusHelper(IPublishEndpoint publishEndpoint, IMapper mapper)
        {
            _publishEndpoint = publishEndpoint;
            _mapper = mapper;
        }

        public async Task SendEventToServiceBus<T>(Dto newAuction)
        {
            T message = _mapper.Map<T>(newAuction);
            await _publishEndpoint.Publish(message);
        }

        public async Task SendEventToServiceBus<T>(ContractEntity auction)
        {
            T message = _mapper.Map<T>(auction);
            await _publishEndpoint.Publish(message);
        }

        //public async Task SendEventToServiceBus<T>(IContractEntity auction)
        //{
        //    T message = _mapper.Map<T>(auction);
        //    await _publishEndpoint.Publish(message);
        //}
    }
}
