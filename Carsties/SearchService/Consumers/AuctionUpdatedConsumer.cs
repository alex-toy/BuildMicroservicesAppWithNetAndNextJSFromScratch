﻿using AutoMapper;
using Contracts.AuctionEvents;
using MassTransit;
using MongoDB.Driver;
using MongoDB.Entities;

namespace SearchService;

public class AuctionUpdatedConsumer : IConsumer<AuctionUpdated>
{
    private readonly IMapper _mapper;

    public AuctionUpdatedConsumer(IMapper mapper)
    {
        _mapper = mapper;
    }
    public async Task Consume(ConsumeContext<AuctionUpdated> context)
    {
        Console.WriteLine("--> Consuming auction updated: " + context.Message.Id);

        Item item = _mapper.Map<Item>(context.Message);

        UpdateResult result = await DB.Update<Item>()
            .Match(a => a.ID == context.Message.Id)
            .ModifyOnly(x => new { x.Color, x.Make, x.Model, x.Year, x.Mileage }, item)
            .ExecuteAsync();

        if (!result.IsAcknowledged) throw new MessageException(typeof(AuctionUpdated), "Problem updating mongodb");
    }
}
