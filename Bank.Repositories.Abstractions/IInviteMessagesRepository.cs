using System;
using System.Threading.Tasks;
using Bank.Entities;

namespace Bank.Repositories.Abstractions
{
    public interface IInviteMessagesRepository
    {
        Task<int> GetMessagesCountAsync(int apiId, DateTimeOffset date);
        Task<int> AddMessageAsync(InviteMessageEntity inviteMessage);
        Task<int> AddMessageLogEntryAsync(InviteMessagesLogEntity[] log);
    }

    public class InviteMessagesRepositoryOptions
    {
        public string? ConnectionString { get; set; }
    }
}
