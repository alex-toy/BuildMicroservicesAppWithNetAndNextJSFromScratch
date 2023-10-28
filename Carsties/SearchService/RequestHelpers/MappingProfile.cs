using AutoMapper;
using Contracts.AuctionEvents;

namespace SearchService.RequestHelpers
{

    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<AuctionCreated, Item>();
            CreateMap<AuctionUpdated, Item>();
        }
    }
}
