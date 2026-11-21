namespace RestaurantApi.Entities;

public sealed class Booking
{
    public int Id { get; set; }

    public string Name { get; set; } = string.Empty;
    public DateTime At { get; set; }
    public int Guests { get; set; }

    public int TableId { get; set; }
    public Table Table { get; set; } = null!;
}