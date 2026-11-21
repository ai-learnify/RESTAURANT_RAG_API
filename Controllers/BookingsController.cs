using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using RestaurantApi.Entities;
using RestaurantApi.Data;

namespace RestaurantApi.Controllers;

[ApiController, Route("api/bookings")]
public sealed class BookingsController(DataContext db) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<List<Booking>>> GetAll()
    {
        var bookings = await db.Bookings.OrderBy(b => b.At).ToListAsync();

        return bookings;
    }
}
