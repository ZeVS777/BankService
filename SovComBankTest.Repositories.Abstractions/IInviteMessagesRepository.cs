using System;
using System.Threading.Tasks;
using SovComBankTest.Entities;

namespace SovComBankTest.Repositories.Abstractions
{
    public interface IInviteMessagesRepository
    {
        Task<int> GetMessagesCountAsync(int apiId, DateTimeOffset date);
        Task<int> AddMessageAsync(InviteMessageEntity inviteMessage);
        Task<int> AddMessageLogEntryAsync(InviteMessagesLogEntity[] log);
    }
}
