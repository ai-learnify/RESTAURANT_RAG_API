namespace RestaurantApi.Extensions;

public static class TimeProviderExtensions
{
    private static readonly TimeZoneInfo RestaurantZone = TimeZoneInfo.FindSystemTimeZoneById("Asia/Tbilisi");

    extension(TimeProvider time)
    {
        public DateTimeOffset GetRestaurantNow() =>
            TimeZoneInfo.ConvertTime(time.GetUtcNow(), RestaurantZone);

        public DateTime ToUtc(DateTime restaurantTime) =>
            TimeZoneInfo.ConvertTimeToUtc(DateTime.SpecifyKind(restaurantTime, DateTimeKind.Unspecified), RestaurantZone);
    }
}