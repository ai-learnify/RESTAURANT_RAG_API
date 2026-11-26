using Microsoft.EntityFrameworkCore;
using RestaurantApi.Extensions;
using RestaurantApi.Entities;

namespace RestaurantApi.Data;

public static class DataSeed
{
    // Products and tables go into the migration via HasData — stable Ids for every lecturer.
    public static Product[] Products =>
    [
        new() { Id = 1, Name = "Khachapuri Imeruli", Price = 18, Description = "Round bread baked with a thick layer of melted Imeretian cheese inside, served hot from the oven." },
        new() { Id = 2, Name = "Adjarian Khachapuri", Price = 22, Description = "Boat-shaped bread filled with sulguni cheese, topped with a raw egg yolk and a piece of butter — stir it all together and tear off the crust." },
        new() { Id = 3, Name = "Khinkali (5 pcs)", Price = 12, Description = "Hand-pleated dumplings filled with minced beef and pork, onion, coriander and hot broth, sprinkled with black pepper." },
        new() { Id = 4, Name = "Mushroom Khinkali (5 pcs)", Price = 11, Description = "Dumplings filled with chopped mushrooms, onion and herbs, cooked in their own juices." },
        new() { Id = 5, Name = "Lobio in a clay pot", Price = 14, Description = "Red beans stewed with onion, coriander, garlic and blue fenugreek, served bubbling in a clay pot with hot mchadi." },
        new() { Id = 6, Name = "Mchadi", Price = 4, Description = "Crispy fried cornbread from western Georgia, made from maize flour, water and salt." },
        new() { Id = 7, Name = "Pkhali", Price = 13, Description = "Three balls of finely chopped spinach, beetroot and leek mixed with walnut paste, garlic and herbs, decorated with pomegranate seeds." },
        new() { Id = 8, Name = "Aubergine with walnuts", Price = 15, Description = "Fried aubergine slices rolled around a garlicky walnut paste, topped with pomegranate seeds." },
        new() { Id = 9, Name = "Tomato salad", Price = 12, Description = "Ripe tomatoes and cucumbers with red onion, coriander, basil and a dressing of ground walnuts and Kakhetian sunflower oil." },
        new() { Id = 10, Name = "Ajapsandali", Price = 14, Description = "Summer stew of aubergine, peppers, tomatoes, potatoes and onion with garlic and plenty of fresh herbs." },
        new() { Id = 11, Name = "Elarji", Price = 10, Description = "Thick cornmeal porridge stretched with lots of melted sulguni cheese, a Megrelian classic." },
        new() { Id = 12, Name = "Mtsvadi", Price = 25, Description = "Pork on a skewer grilled over vine branches, served with raw onion rings and tkemali plum sauce." },
        new() { Id = 13, Name = "Chicken Shkmeruli", Price = 28, Description = "Roasted young chicken baked in a clay pan in a creamy garlic and milk sauce." },
        new() { Id = 14, Name = "Chakapuli", Price = 30, Description = "Lamb stewed with tarragon, green plums, white wine and spring onion." },
        new() { Id = 15, Name = "Ojakhuri", Price = 24, Description = "Fried pork with potatoes and onion, served sizzling in a ketsi clay dish." },
        new() { Id = 16, Name = "Kharcho", Price = 16, Description = "Thick beef soup with rice, ground walnuts, tkemali plum sauce and khmeli-suneli spices." },
        new() { Id = 17, Name = "Churchkhela", Price = 6, Description = "Walnuts threaded on a string and dipped in thickened grape juice, then dried — the Georgian sweet." },
        new() { Id = 18, Name = "Pelamushi", Price = 7, Description = "Grape pudding made from grape juice and corn flour, served chilled with crushed walnuts." }
    ];

    public static Table[] Tables =>
    [
        new() { Id = 1, Number = 1, Seats = 2 },
        new() { Id = 2, Number = 2, Seats = 2 },
        new() { Id = 3, Number = 3, Seats = 2 },
        new() { Id = 4, Number = 4, Seats = 4 },
        new() { Id = 5, Number = 5, Seats = 4 },
        new() { Id = 6, Number = 6, Seats = 4 },
        new() { Id = 7, Number = 7, Seats = 6 },
        new() { Id = 8, Number = 8, Seats = 6 }
    ];

    // Bookings are relative to "tomorrow", so they can't live in HasData (fixed values in the migration).
    // Registered with UseSeeding in Program.cs; runs on `dotnet ef database update` when there are no bookings.
    public static void SeedBookings(DbContext context)
    {
        var bookings = context.Set<Booking>();

        if (bookings.Any()) return;

        var time = TimeProvider.System;
        var tomorrow = time.GetRestaurantNow().Date.AddDays(1);

        DateTime At(int hour, int minute = 0) => time.ToUtc(tomorrow.AddHours(hour).AddMinutes(minute));

        bookings.AddRange(
            new Booking { TableId = 4, At = At(19), Guests = 4, Name = "Nino" },
            new Booking { TableId = 5, At = At(19, 30), Guests = 3, Name = "Luka" },
            new Booking { TableId = 7, At = At(20), Guests = 6, Name = "Giorgi" },
            new Booking { TableId = 1, At = At(20, 30), Guests = 2, Name = "Ana" }
        );

        context.SaveChanges();
    }
}