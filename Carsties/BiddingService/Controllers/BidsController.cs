using AutoMapper;
using BiddingService.DTOs;
using BiddingService.Models;
using BiddingService.Services;
using Contracts.AuctionEvents;
using Contracts.ServiceBus;
using MassTransit;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Entities;

namespace BiddingService.Controllers;

[ApiController]
[Route("api/[controller]")]
public class BidsController : ControllerBase
{
    private readonly IMapper _mapper;
    private readonly IPublishEndpoint _publishEndpoint;
    private readonly GrpcAuctionClient _grpcClient;
    private readonly IServiceBusHelper _serviceBusHelper;

    public BidsController(IMapper mapper, IPublishEndpoint publishEndpoint, GrpcAuctionClient grpcClient, IServiceBusHelper serviceBusHelper)
    {
        _mapper = mapper;
        _publishEndpoint = publishEndpoint;
        _grpcClient = grpcClient;
        _serviceBusHelper = serviceBusHelper;
    }

    [Authorize]
    [HttpPost]
    public async Task<ActionResult<BidDto>> PlaceBid(string auctionId, int amount)
    {
        Auction auction = await DB.Find<Auction>().OneAsync(auctionId);

        if (auction == null)
        {
            auction = _grpcClient.GetAuction(auctionId);

            if (auction == null) return BadRequest("Cannot accept bids on this auction at this time");
        }

        if (auction.Seller == User.Identity.Name)
        {
            return BadRequest("You cannot bid on your own auction");
        }

        Bid bid = new Bid
        {
            Amount = amount,
            AuctionId = auctionId,
            Bidder = User.Identity.Name
        };

        bool auctionIsExpired = auction.AuctionEnd < DateTime.UtcNow;
        if (auctionIsExpired)
        {
            bid.BidStatus = BidStatus.Finished;
        }
        else
        {
            Bid highBid = await DB.Find<Bid>()
                        .Match(a => a.AuctionId == auctionId)
                        .Sort(b => b.Descending(x => x.Amount))
                        .ExecuteFirstAsync();

            bool newBidHasHigherAmount = highBid != null && amount > highBid.Amount;
            if (newBidHasHigherAmount || highBid == null)
            {
                bid.BidStatus = amount > auction.ReservePrice
                    ? BidStatus.Accepted
                    : BidStatus.AcceptedBelowReserve;
            }

            bool amountIsTooLow = highBid != null && bid.Amount <= highBid.Amount;
            if (amountIsTooLow)
            {
                bid.BidStatus = BidStatus.TooLow;
            }
        }

        await DB.SaveAsync(bid);

        await _publishEndpoint.Publish(_mapper.Map<BidPlaced>(bid));

        //await _serviceBusHelper.SendEventToServiceBus<BidPlaced>(bid);

        return Ok(_mapper.Map<BidDto>(bid));
    }

    [HttpGet("{auctionId}")]
    public async Task<ActionResult<List<BidDto>>> GetBidsForAuction(string auctionId)
    {
        List<Bid> bids = await GetBids(auctionId);

        return bids.Select(_mapper.Map<BidDto>).ToList();
    }

    private static async Task<List<Bid>> GetBids(string auctionId)
    {
        return await DB.Find<Bid>()
            .Match(a => a.AuctionId == auctionId)
            .Sort(b => b.Descending(a => a.BidTime))
            .ExecuteAsync();
    }
}
