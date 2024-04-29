using Bookify.Application.Abstractions.Data;
using Bookify.Application.Abstractions.Messaging;
using Bookify.Domain.Abstractions;
using Dapper;
using System.Data;

namespace Bookify.Application.Bookings.GetBooking;

internal sealed class GetBookingQueryHandler : IQueryHandler<GetBookingQuery, BookingResponse>
{
    private readonly ISqlConnectionFactory _sqlConnectionFactory;

    public GetBookingQueryHandler(ISqlConnectionFactory sqlConnectionFactory)
    {
        _sqlConnectionFactory = sqlConnectionFactory;
    }

    public async Task<Result<BookingResponse>> Handle(GetBookingQuery request, CancellationToken cancellationToken)
    {
        using IDbConnection connection = await _sqlConnectionFactory.CreateConnectionAsync();

        var booking = await connection.QueryFirstOrDefaultAsync<BookingResponse>(
                       "SELECT Id, ApartmentId, UserId, Duration, PriceForPeriod, CleaningFee, AmenitiesUpCharge, TotalPrice, Status, CreatedOnUtc, ConfirmedUtc, RejectedOnUtc, CompletedOnUtc, CanceledOnUtc FROM Bookings WHERE Id = @BookingId", new { request.BookingId });
        return booking;
    }
}
