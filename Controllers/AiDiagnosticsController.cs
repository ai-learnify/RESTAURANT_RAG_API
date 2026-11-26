using RestaurantApi.Contracts.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.AI;

namespace RestaurantApi.Controllers;

[ApiController, Route("api/diagnostics/ai")]
public sealed class AiDiagnosticsController(IChatClient chat) : ControllerBase
{
    [HttpPost("ask")]
    public async Task<IActionResult> Ask(AskRequest req)
    {
        var options = new ChatOptions { MaxOutputTokens = req.MaxOutputTokens, Temperature = req.Temperature };

        var response = await chat.GetResponseAsync(req.Message, options);

        return Ok(AskResponse.From(response));
    }

    [HttpPost("chat")]
    public async Task<IActionResult> Chat(ChatRequest req)
    {
        var messages = new List<ChatMessage>
        {
            new(ChatRole.System, SystemPrompt),
            new(ChatRole.User, req.Message)
        };

        var response = await chat.GetResponseAsync(messages);

        return Ok(new ChatReply(response.Text));
    }

    private const string SystemPrompt = """
                                        You are the assistant of the restaurant "Supra".
                                        You answer only about the menu, bookings and opening hours (12:00–23:00).
                                        Answer in at most 3 sentences, in the language the guest writes in.
                                        On any other topic, politely decline and say what you can help with.
                                        Never invent prices or ingredients — say they are on the menu.
                                        """;
}