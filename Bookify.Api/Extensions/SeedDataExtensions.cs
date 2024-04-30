using Bogus;
using Bookify.Application.Abstractions.Data;
using Bookify.Domain.Apartments;
using Dapper;
using System.Data;

namespace Bookify.Api.Extensions;

public static class SeedDataExtensions
{
    public static async void SeedData(this IApplicationBuilder app)
    {
        using var scope = app.ApplicationServices.CreateScope();

        var sqlConnectionFactory = scope.ServiceProvider.GetRequiredService<ISqlConnectionFactory>();
        using IDbConnection connection = await sqlConnectionFactory.CreateConnectionAsync();

        Faker faker = new Faker();

        List<object> apartments = new();
        for (var i = 0; i < 100; i++)
        {
            apartments.Add(new
            {
                Id = Guid.NewGuid(),
                Name = faker.Company.CompanyName(),
                Description = "Amazing view",
                Country = faker.Address.Country(),
                State = faker.Address.State(),
                ZipCode = faker.Address.ZipCode(),
                City = faker.Address.City(),
                Street = faker.Address.StreetAddress(),
                PriceAmount = faker.Random.Decimal(50, 1000),
                PriceCurrency = "USD",
                CleaningFeeAmount = faker.Random.Decimal(25, 200),
                CleaningFeeCurrency = "USD",
                Amenities = new List<int> { (int)Amenity.Parking, (int)Amenity.MountainView },
                LastBookedOn = DateTime.MinValue
            });
        }

        const string sql = """
            INSERT INTO public.apartments
            (id, "name", description, "address_Country", "address_State", "address_ZipCode", "address_City", "address_Street", "price_Amount", "price_Currency", "cleaningFee_Amount", "cleaningFee_Currency", "amenities", "lastBookedOnUtc")
            VALUES(@Id, @Name, @Description, @Country, @State, @ZipCode, @City, @Street, @PriceAmount, @PriceCurrency, @CleaningFeeAmount, @CleaningFeeCurrency, @Amenities, @LastBookedOn);
            """;

        connection.Execute(sql, apartments);
    }
}
