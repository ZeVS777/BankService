using System;
using System.Data;
using System.Threading.Tasks;
using Dapper;
using Dapper.Contrib.Extensions;
using SovComBankTest.Entities;
using SovComBankTest.Repositories.Abstractions;

namespace SovComBankTest.Repositories
{
    public sealed class InviteMessagesRepository: IInviteMessagesRepository
    {
        private readonly IDbConnection _dbConnection;

        public InviteMessagesRepository(IDbConnection dbConnection) => _dbConnection = dbConnection;

        public Task<int> GetMessagesCountAsync(int apiId, DateTimeOffset date)
        {
            var sql = string.Concat(
                "select count(log.Id) from InviteMessagesLog log ",
                $"join InviteMessage m on m.{nameof(InviteMessageEntity.Id)} = log.{nameof(InviteMessagesLogEntity.InviteMessageId)} ",
                $"where m.{nameof(InviteMessageEntity.ApiId)} = @{nameof(apiId)} and log.{nameof(InviteMessagesLogEntity.SendDateTime)} >= @{nameof(date)}"
            );

            return _dbConnection.QuerySingleAsync<int>(sql, new {apiId, date});
        }

        public Task<int> AddMessageAsync(InviteMessageEntity inviteMessage) => _dbConnection.InsertAsync(inviteMessage);

        public Task<int> AddMessageLogEntryAsync(InviteMessagesLogEntity[] log) => _dbConnection.InsertAsync(log);
    }
}
