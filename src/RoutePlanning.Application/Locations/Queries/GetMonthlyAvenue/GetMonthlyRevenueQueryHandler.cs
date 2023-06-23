using Netcompany.Net.Cqs.Queries;
using CancellationToken = System.Threading.CancellationToken;

using Microsoft.EntityFrameworkCore;
using RoutePlanning.Domain.BookingRequest;


namespace RoutePlanning.Application.Locations.Queries.GetMonthlyAvenue
{
    public sealed class GetMonthlyRevenueQueryHandler : IQueryHandler<GetMonthlyRevenueQuery, decimal>
    {
        private readonly IQueryable<BookingRequest> _bookingRequests;

        public GetMonthlyRevenueQueryHandler(IQueryable<BookingRequest> bookingRequests)
        {
            _bookingRequests = bookingRequests;
        }

        public async Task<decimal> Handle(GetMonthlyRevenueQuery query, CancellationToken cancellationToken)
        {
            var now = DateTime.Now;
            var totalRevenue = await _bookingRequests
                .Where(br => br.DateTime.Month == now.Month && br.DateTime.Year == now.Year)
                .SumAsync(br => br.Price ?? 0, cancellationToken);
        
            return Convert.ToDecimal(totalRevenue);
        }
    }
}
