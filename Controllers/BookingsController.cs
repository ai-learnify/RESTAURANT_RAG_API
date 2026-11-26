using RestaurantApi.Contracts.Bookings;
using RestaurantApi.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.AI;
using RestaurantApi.Entities;
using RestaurantApi.Data;

namespace RestaurantApi.Controllers;

[ApiController, Route("api/bookings")]
public sealed class BookingsController(DataContext db, IChatClient chat, TimeProvider time) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<List<Booking>>> GetAll()
    {
        var bookings = await db.Bookings.OrderBy(b => b.At).ToListAsync();

        return bookings;
    }


    [HttpPost("parse")]
    public async Task<IActionResult> Parse(BookingParseRequest req)
    {
        var messages = new List<ChatMessage>
        {
            new(ChatRole.System, $"""
                                  Extract the booking details from the guest's message.
                                  Now in the restaurant: {time.GetRestaurantNow():yyyy-MM-dd'T'HH:mm:sszzz, dddd}.
                                  Leave null whatever is not in the message.
                                  """),
            new(ChatRole.User, req.Message)
        };

        var response = await chat.GetResponseAsync<BookingParseResponse>(messages);

        if (response.Result.At is null)
            return BadRequest("Which day and what time?");

        if (response.Result.At.Value.Hour is < 12 or >= 23)
            return BadRequest("We're open 12:00–23:00");

        if (response.Result.Guests is null)
            return BadRequest("How many of you?");

        return Ok(response.Result);
    }
}