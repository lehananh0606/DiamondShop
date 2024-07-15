namespace DiamondShopSystem.Helper
{
    public sealed record ErrorResponse(int StatusCode, string? Message, bool isError, dynamic Errors, DateTime Timestamp);
}
