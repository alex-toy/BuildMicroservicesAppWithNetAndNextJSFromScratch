using Contracts.AuctionEvents;
using MassTransit;
using MongoDB.Entities;

namespace SearchService;

public class BidPlacedConsumer : IConsumer<BidPlaced>
{
    public async Task Consume(ConsumeContext<BidPlaced> context)
    {
        Console.WriteLine("--> Consuming bid placed");

        var auction = await DB.Find<Item>().OneAsync(context.Message.AuctionId);

        bool bidStatusAccepted = context.Message.BidStatus.Contains("Accepted");
        bool bidHasHigherOffer = context.Message.Amount > auction.CurrentHighBid;

        if (bidStatusAccepted && bidHasHigherOffer)
        {
            auction.CurrentHighBid = context.Message.Amount;
            await auction.SaveAsync();
        }
    }
}
