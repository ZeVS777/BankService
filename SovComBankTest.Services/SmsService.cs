using SovComBankTest.Services.Abstractions;
using SovComBankTest.Services.Models;

namespace SovComBankTest.Services
{
    public class SmsService: ISmsService
    {
        public SendStatus Send(InviteMessageModel inviteMessage)
        {
            if (inviteMessage.ApiId != 4) return SendStatus.Forbidden;

            //Check TooMany in transaction and send if ok

            return SendStatus.Ok;
        }
    }
}
