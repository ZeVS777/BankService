namespace SovComBankTest.Services.Models
{
    public record InviteMessageModel(string[] Phones, string Message, int ApiId);

    public record SingleInviteMessageModel(string Phone, string Message);
}
