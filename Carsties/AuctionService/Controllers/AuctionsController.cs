using AuctionService.Data;
using AuctionService.DTOs;
using AuctionService.Entities;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;

namespace AuctionService.Controllers;

[ApiController]
[Route("api/auctions")]
public class AuctionsController : ControllerBase
{
    private readonly IAuctionRepository _repo;
    private readonly IMapper _mapper;
    //private readonly IPublishEndpoint _publishEndpoint;

    public AuctionsController(IAuctionRepository repo, IMapper mapper)
    {
        _repo = repo;
        _mapper = mapper;
        //_publishEndpoint = publishEndpoint;
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
        var auction = _mapper.Map<Auction>(auctionDto);

        auction.Seller = User.Identity.Name;

        _repo.AddAuction(auction);

        AuctionDto newAuction = _mapper.Map<AuctionDto>(auction);

        //await _publishEndpoint.Publish(_mapper.Map<AuctionCreated>(newAuction));

        bool result = await _repo.SaveChangesAsync();

        if (!result) return BadRequest("Could not save changes to the DB");

        return CreatedAtAction(nameof(GetAuctionById), new { auction.Id }, newAuction);
    }

    //[Authorize]
    [HttpPut("{id}")]
    public async Task<ActionResult> UpdateAuction(Guid id, UpdateAuctionDto updateAuctionDto)
    {
        var auction = await _repo.GetAuctionEntityById(id);

        if (auction == null) return NotFound();

        if (auction.Seller != User.Identity.Name) return Forbid();

        auction.UpdateBasedOn(updateAuctionDto);

        //await _publishEndpoint.Publish(_mapper.Map<AuctionUpdated>(auction));

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

        //await _publishEndpoint.Publish<AuctionDeleted>(new { Id = auction.Id.ToString() });

        var result = await _repo.SaveChangesAsync();

        if (!result) return BadRequest("Could not update DB");

        return Ok();
    }
}
