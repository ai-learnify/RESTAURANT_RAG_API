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
}