using System.ComponentModel;

namespace RestaurantApi.Contracts.Bookings;

public sealed record BookingParseResponse(
    [property: Description("Booking date and time in restaurant time with UTC offset, e.g. 2026-11-28T20:00:00+04:00")] DateTimeOffset? At,
    [property: Description("Number of guests")] int? Guests
);