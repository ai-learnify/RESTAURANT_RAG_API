using RestaurantApi.Contracts.Assistant;
using RestaurantApi.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.AI;
using RestaurantApi.Tools;

namespace RestaurantApi.Controllers;

[ApiController, Route("api/assistant")]
public sealed class AssistantController(IChatClient chat, MenuTools menu, TableTools table, TimeProvider time) : ControllerBase
{
    [HttpPost("chat")]
    public async Task<IActionResult> Chat(ChatRequest req)
    {
        var messages = new List<ChatMessage>
        {
            new(ChatRole.System, SystemPrompt()),
            new(ChatRole.User, req.Message)
        };

        var options = new ChatOptions
        {
            Tools =
            [
                AIFunctionFactory.Create(menu.GetMenu, name: "get_menu"),
                AIFunctionFactory.Create(table.CheckAvailability, name: "check_availability")
            ]
        };

        var response = await chat.GetResponseAsync(messages, options);

        return Ok(new ChatReply(response.Text));
    }

    private string SystemPrompt() => $"""
                                             You are the assistant of the restaurant "Supra".
                                             You answer only about the menu, bookings and opening hours (12:00–23:00).
                                             Answer in at most 3 sentences, in the language the guest writes in.
                                             On any other topic, politely decline and say what you can help with.
                                             Now in the restaurant: {time.GetRestaurantNow():yyyy-MM-dd'T'HH:mm:sszzz, dddd}.
                                             """;
}