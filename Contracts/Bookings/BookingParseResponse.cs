using System.ComponentModel;

namespace RestaurantApi.Contracts.Bookings;

public sealed record BookingParseResponse(
    [property: Description("Booking date and time")] DateTime? At,
    [property: Description("Number of guests")] int? Guests
);