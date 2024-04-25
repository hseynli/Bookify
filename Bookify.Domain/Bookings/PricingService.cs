﻿using Bookify.Domain.Apartments;
using Bookify.Domain.Shared;

namespace Bookify.Domain.Bookings;

public class PricingService
{
    public PricingDetails CalculatePrice(Apartment apartment, DateRange period)
    {
        var curreny = apartment.Price.Currency;

        var priceForPeriod = new Money(apartment.Price.Amount * period.LengthInDays, curreny);

        decimal percentageUpCharge = 0.0m;
        foreach (var amenity in apartment.Amenities)
        {
            percentageUpCharge += amenity switch
            {
                Amenity.GardenView or Amenity.MountainView => 0.05m,
                Amenity.AirConditioning => 0.1m,
                Amenity.Parking => 0.01m,
                _ => 0.0m
            };
        }

        var amenitiesUpCharge = Money.Zero(curreny);

        if (percentageUpCharge > 0)
            amenitiesUpCharge = new Money(priceForPeriod.Amount * percentageUpCharge, curreny);

        var totalPrice = Money.Zero();

        totalPrice += priceForPeriod;

        if(!apartment.CleaningFee.IsZero())
            totalPrice += apartment.CleaningFee;

        totalPrice += amenitiesUpCharge;

        return new PricingDetails(priceForPeriod, apartment.CleaningFee, amenitiesUpCharge, totalPrice);
    }
}
