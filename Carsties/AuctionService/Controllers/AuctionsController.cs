using AuctionService.Data;
using AuctionService.DTOs;
using AuctionService.Entities;
using AutoMapper;
using Contracts.AuctionEvents;
using MassTransit;
using Microsoft.AspNetCore.Mvc;

namespace AuctionService.Controllers;

[ApiController]
[Route("api/auctions")]
public class AuctionsController : ControllerBase
{
    private readonly IAuctionRepository _repo;
    private readonly IMapper _mapper;
    private readonly IPublishEndpoint _publishEndpoint;

    public AuctionsController(IAuctionRepository repo, IMapper mapper, IPublishEndpoint publishEndpoint)
    {
        _repo = repo;
        _mapper = mapper;
        _publishEndpoint = publishEndpoint;
    }

    [HttpGet]
    public async Task<ActionResult<List<AuctionDto>>> GetAllAuctions(string date)
    {
        return await _repo.GetAuctionsAsync(date);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<AuctionDto>> GetAuctionById(Guid id)
    {
        var auction = await _repo.GetAuctionByIdAsync(id);

        if (auction == null) return NotFound();

        return auction;
    }

    //[Authorize]
    [HttpPost]
    public async Task<ActionResult<AuctionDto>> CreateAuction(CreateAuctionDto auctionDto)
    {
        Auction auction = _mapper.Map<Auction>(auctionDto);

        auction.Seller = User.Identity.Name;

        _repo.AddAuction(auction);

        AuctionDto newAuction = _mapper.Map<AuctionDto>(auction);

        await SendEventToServiceBus<AuctionCreated>(newAuction);

        bool result = await _repo.SaveChangesAsync();

        if (!result) return BadRequest("Could not save changes to the DB");

        return CreatedAtAction(nameof(GetAuctionById), new { auction.Id }, newAuction);
    }

    //[Authorize]
    [HttpPut("{id}")]
    public async Task<ActionResult> UpdateAuction(Guid id, UpdateAuctionDto updateAuctionDto)
    {
        Auction auction = await _repo.GetAuctionEntityById(id);

        if (auction == null) return NotFound();

        if (auction.Seller != User.Identity.Name) return Forbid();

        auction.UpdateBasedOn(updateAuctionDto);
        AuctionDto updatedAuction = _mapper.Map<AuctionDto>(auction);
        await SendEventToServiceBus<AuctionUpdated>(updatedAuction);

        var result = await _repo.SaveChangesAsync();

        if (!result) return BadRequest("Problem saving changes");

        return Ok();

    }

    //[Authorize]
    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteAuction(Guid id)
    {
        var auction = await _repo.GetAuctionEntityById(id);

        if (auction == null) return NotFound();

        if (auction.Seller != User.Identity.Name) return Forbid();

        _repo.RemoveAuction(auction);

        await _publishEndpoint.Publish<AuctionDeleted>(new { Id = auction.Id.ToString() });

        var result = await _repo.SaveChangesAsync();

        if (!result) return BadRequest("Could not update DB");

        return Ok();
    }

    private async Task SendEventToServiceBus<T>(Dto newAuction)
    {
        await _publishEndpoint.Publish(_mapper.Map<T>(newAuction));
    }
}
