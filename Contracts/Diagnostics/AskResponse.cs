using Microsoft.Extensions.AI;

namespace RestaurantApi.Contracts.Diagnostics;

public sealed record AskResponse(string Text, long? InputTokens, long? OutputTokens, long? ReasoningTokens, ChatFinishReason? Reason)
{
    public static AskResponse From(ChatResponse res) => new AskResponse(
        res.Text,
        res.Usage?.InputTokenCount,
        res.Usage?.OutputTokenCount,
        res.Usage?.ReasoningTokenCount,
        res.FinishReason
    );
}