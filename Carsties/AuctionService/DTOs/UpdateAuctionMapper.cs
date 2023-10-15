using AuctionService.Entities;

namespace AuctionService.DTOs
{
    public static class UpdateAuctionMapper
    {
        public static void UpdateBasedOn(this Auction auction, UpdateAuctionDto updateAuctionDto)
        {
            auction.Item.Make = updateAuctionDto.Make ?? auction.Item.Make;
            auction.Item.Model = updateAuctionDto.Model ?? auction.Item.Model;
            auction.Item.Color = updateAuctionDto.Color ?? auction.Item.Color;
            auction.Item.Mileage = updateAuctionDto.Mileage ?? auction.Item.Mileage;
            auction.Item.Year = updateAuctionDto.Year ?? auction.Item.Year;
        }
    }
}
