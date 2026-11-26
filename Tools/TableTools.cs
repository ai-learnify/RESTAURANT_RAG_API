using Microsoft.EntityFrameworkCore;
using System.ComponentModel;
using RestaurantApi.Data;

namespace RestaurantApi.Tools;

public record AvailabilityInfo(bool Available, int FreeTables);

public sealed class TableTools(DataContext db)
{
    [Description("Checks whether a table is free at the given time")]
    public async Task<AvailabilityInfo> CheckAvailability(
        [Description("Booking date and time")] DateTimeOffset at,
        [Description("Number of guests")] int guests
    )
    {
        var utc = at.UtcDateTime;
        var from = utc.AddHours(-2);
        var to = utc.AddHours(2);

        var free = await db.Tables.CountAsync(t =>
            t.Seats >= guests &&
            !db.Bookings.Any(b => b.TableId == t.Id && b.At > from && b.At < to)
        );

        return new AvailabilityInfo(free > 0, free);
    }
}