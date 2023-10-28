using Contracts.AuctionEvents;
using MassTransit;
using MongoDB.Driver;
using MongoDB.Entities;

namespace SearchService;

public class AuctionDeletedConsumer : IConsumer<AuctionDeleted>
{
    public async Task Consume(ConsumeContext<AuctionDeleted> context)
    {
        Console.WriteLine("--> Consuming AuctionDeleted: " + context.Message.Id);

        DeleteResult result = await Delete(context);

        if (!result.IsAcknowledged) throw new MessageException(typeof(AuctionDeleted), "Problem deleting auction");
    }

    private static async Task<DeleteResult> Delete(ConsumeContext<AuctionDeleted> context)
    {
        return await DB.DeleteAsync<Item>(context.Message.Id);
    }
}
