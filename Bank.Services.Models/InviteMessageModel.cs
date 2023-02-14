namespace Bank.Services.Models
{
    public sealed record InviteMessageModel(string[] Phones, string Message, int ApiId);

    public sealed record SingleInviteMessageModel(string Phone, string Message);
}
