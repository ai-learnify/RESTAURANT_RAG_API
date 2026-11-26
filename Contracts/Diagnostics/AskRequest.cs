namespace RestaurantApi.Contracts.Diagnostics;

public sealed record AskRequest(
    string Message,
    float? Temperature = null,
    int? MaxOutputTokens = null
);