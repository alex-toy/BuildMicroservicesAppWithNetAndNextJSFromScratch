using Microsoft.AspNetCore.Mvc;
using MongoDB.Entities;
using SearchService.RequestHelpers;

namespace SearchService;

[ApiController]
[Route("api/search")]
public class SearchController : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<List<Item>>> SearchItems([FromQuery] SearchParams searchParams)
    {
        PagedSearch<Item, Item> query = DB.PagedSearch<Item, Item>();

        query.SetAllFilters(searchParams);

        query.PageNumber(searchParams.PageNumber);

        query.PageSize(searchParams.PageSize);

        (IReadOnlyList<Item> Results, long TotalCount, int PageCount) result = await query.ExecuteAsync();

        return Ok(new
        {
            results = result.Results,
            pageCount = result.PageCount,
            totalCount = result.TotalCount
        });
    }
}
