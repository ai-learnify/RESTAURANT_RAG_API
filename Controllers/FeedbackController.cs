using RestaurantApi.Contracts.Feedback;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.AI;
using RestaurantApi.Enums;

namespace RestaurantApi.Controllers;

[ApiController, Route("api/feedback/ai")]
public sealed class FeedbackController(IChatClient chat) : ControllerBase
{
    [HttpPost("sentiment")]
    public async Task<IActionResult> AnalyzeSentiment(SentimentRequest req)
    {
        var messages = new List<ChatMessage>
        {
            new(ChatRole.System, "Classify the guest's review of the restaurant"),
            new(ChatRole.User, req.Message)
        };

        var response = await chat.GetResponseAsync<Sentiment>(messages);

        if (response.Result == Sentiment.Negative) Console.WriteLine("SMS: notify the manager");

        return Ok(new SentimentResponse(response.Result.ToString()));
    }
}