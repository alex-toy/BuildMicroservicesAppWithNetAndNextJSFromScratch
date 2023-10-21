using MongoDB.Driver;
using MongoDB.Entities;
using System.Linq.Expressions;

namespace SearchService.RequestHelpers
{
    public static class PagedSearchHelper
    {
        private static Func<SortDefinitionBuilder<Item>, SortDefinition<Item>> makeAscending = x => x.Ascending(a => a.Make);
        private static Func<SortDefinitionBuilder<Item>, SortDefinition<Item>> makeDescending = x => x.Descending(a => a.Make);
             
        private static Expression<Func<Item, bool>> auctionIsFinished = x => x.AuctionEnd < DateTime.UtcNow;
        private static Expression<Func<Item, bool>> auctionIsEndingSoon = x => x.AuctionEnd < DateTime.UtcNow.AddHours(6) && x.AuctionEnd > DateTime.UtcNow;
        private static Expression<Func<Item, bool>> auctionIsNotFinished = x => x.AuctionEnd > DateTime.UtcNow;

        public static void SetAllFilters(this PagedSearch<Item, Item> query, SearchParams searchParams)
        {
            query.SetMatch(searchParams);

            query.SetOrdering(searchParams);

            query.SetFiltering(searchParams);

            query.SetSellerMatching(searchParams);

            query.SetWinnerMatching(searchParams);
        }

        public static void SetMatch(this PagedSearch<Item, Item> query, SearchParams searchParams)
        {
            if (!string.IsNullOrEmpty(searchParams.SearchTerm))
            {
                query.Match(Search.Full, searchParams.SearchTerm).SortByTextScore();
            }
        }

        public static void SetWinnerMatching(this PagedSearch<Item, Item> query, SearchParams searchParams)
        {
            if (!string.IsNullOrEmpty(searchParams.Winner))
            {
                query.Match(x => x.Winner == searchParams.Winner);
            }
        }

        public static void SetSellerMatching(this PagedSearch<Item, Item> query, SearchParams searchParams)
        {
            if (!string.IsNullOrEmpty(searchParams.Seller))
            {
                query.Match(x => x.Seller == searchParams.Seller);
            }
        }

        public static void SetFiltering(this PagedSearch<Item, Item> query, SearchParams searchParams)
        {
            query = searchParams.FilterBy switch
            {
                "finished" => query.Match(auctionIsFinished),
                "endingSoon" => query.Match(auctionIsEndingSoon),
                _ => query.Match(auctionIsNotFinished)
            };
        }

        public static void SetOrdering(this PagedSearch<Item, Item> query, SearchParams searchParams)
        {
            query = searchParams.OrderBy switch
            {
                "make" => query.Sort(searchParams.MakeAscending ? makeAscending : makeDescending).Sort(x => x.Ascending(a => a.Model)),
                "new" => query.Sort(x => x.Descending(a => a.CreatedAt)),
                _ => query.Sort(x => x.Ascending(a => a.AuctionEnd))
            };
        }
    }
}
