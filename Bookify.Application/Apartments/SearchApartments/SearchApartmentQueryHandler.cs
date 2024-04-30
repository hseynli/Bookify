using Bookify.Application.Abstractions.Data;
using Bookify.Application.Abstractions.Messaging;
using Bookify.Domain.Abstractions;
using Bookify.Domain.Bookings;
using Dapper;

namespace Bookify.Application.Apartments.SearchApartments;

internal sealed class SearchApartmentQueryHandler : IQueryHandler<SearchApartmentsQuery, IReadOnlyList<ApartmentResponse>>
{
    private readonly ISqlConnectionFactory _sqlConnectionFactory;

    private static readonly int[] ActiveBookingStatuses =
    {
        (int)BookingStatus.Reserved,
        (int)BookingStatus.Confirmed,
        (int)BookingStatus.Completed
    };

    public SearchApartmentQueryHandler(ISqlConnectionFactory sqlConnectionFactory)
    {
        _sqlConnectionFactory = sqlConnectionFactory;
    }

    public async Task<Result<IReadOnlyList<ApartmentResponse>>> Handle(SearchApartmentsQuery request, CancellationToken cancellationToken)
    {
        if (request.StartDate >= request.EndDate)
            return new List<ApartmentResponse>();

        using var connection = await _sqlConnectionFactory.CreateConnectionAsync();

        const string sql = """
            SELECT
                a."id" AS Id,
                a."name" AS Name,
                a."description" AS Description,
                a."price_Amount" AS Price,
                a."price_Currency" AS Currency,
                a."address_Country" AS Country,
                a."address_State" AS State,
                a."address_ZipCode" AS ZipCode,
                a."address_City" AS City,
                a."address_Street" AS Street
            FROM apartments AS a
            WHERE NOT EXISTS
            (
                SELECT 1
                FROM bookings AS b
                WHERE
                    b."apartmentId" = a."id" AND
                    b."duration_Start" <= @EndDate AND
                    b."duration_End" >= @StartDate AND
                    b."status" = ANY(@ActiveBookingStatuses)
            )
            """;

        var apartments = await connection
            .QueryAsync<ApartmentResponse, AddressResponse, ApartmentResponse>(
                sql,
                (apartment, address) =>
                {
                    apartment.Address = address;

                    return apartment;
                },
                new
                {
                    request.StartDate,
                    request.EndDate,
                    ActiveBookingStatuses
                },
                splitOn: "Country");

        return apartments.ToList();
    }
}
