namespace SovComBankTest.Services.Models
{
    public sealed record SendResult(SendStatus Status, int MessagesRemains, string? Message);
}