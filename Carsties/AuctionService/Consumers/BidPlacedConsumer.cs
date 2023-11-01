using AuctionService.Data;
using Contracts.AuctionEvents;
using MassTransit;

namespace AuctionService.Consumers;

public class BidPlacedConsumer : IConsumer<BidPlaced>
{
    private readonly AuctionDbContext _dbContext;

    public BidPlacedConsumer(AuctionDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task Consume(ConsumeContext<BidPlaced> context)
    {
        Console.WriteLine("--> Consuming bid placed");

        var auction = await _dbContext.Auctions.FindAsync(Guid.Parse(context.Message.AuctionId));

        bool bidStatusAccepted = context.Message.BidStatus.Contains("Accepted");
        bool bidHasBiggerOffer = context.Message.Amount > auction.CurrentHighBid;
        bool noCurrentBid = auction.CurrentHighBid == null;

        if ( noCurrentBid || (bidStatusAccepted && bidHasBiggerOffer) )
        {
            auction.CurrentHighBid = context.Message.Amount;
            await _dbContext.SaveChangesAsync();
        }
    }
}
