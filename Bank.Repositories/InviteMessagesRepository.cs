namespace Bank.Repositories;

/// <inheritdoc />
public sealed class InviteMessagesRepository : IInviteMessagesRepository
{
    private readonly IDbConnection _dbConnection;

    /// <summary>
    /// Создать экземпляр класса <see cref="InviteMessagesRepository"/>
    /// </summary>
    /// <param name="dbConnection">Соединение с БД</param>
    public InviteMessagesRepository(IDbConnection dbConnection) => _dbConnection = dbConnection;

    /// <inheritdoc />
    public Task<int> GetMessagesCountAsync(int apiId, DateTimeOffset date)
    {
        const string sql = $"""
            select count(log.{nameof(InviteMessagesLogEntity.Id)}) from {InviteMessagesLogEntity.TableName} log
            join {InviteMessageEntity.TableName} m on m.{nameof(InviteMessageEntity.Id)} = log.{nameof(InviteMessagesLogEntity.InviteMessageId)}
            where m.{nameof(InviteMessageEntity.ApiId)} = @{nameof(apiId)} and log.{nameof(InviteMessagesLogEntity.SendDateTime)} >= @{nameof(date)}
            """;

        return _dbConnection.QuerySingleAsync<int>(sql, new { apiId, date });
    }

    /// <inheritdoc />
    public Task<int> AddMessageAsync(InviteMessageEntity inviteMessage) => _dbConnection.InsertAsync(inviteMessage);

    /// <inheritdoc />
    public Task<int> AddMessageLogEntryAsync(InviteMessagesLogEntity[] log) => _dbConnection.InsertAsync(log);
}
