namespace Bank.Entities;

/// <summary>
/// Запись логирования
/// </summary>
/// <param name="SendDateTime">Дата отправки сообщения</param>
/// <param name="Phone">Номер, на который было отправлено сообщение</param>
/// <param name="InviteMessageId">Идентификатор отправленного сообщения</param>
/// <param name="Id">Идентификатор записи</param>
[Table(TableName)]
public sealed record InviteMessagesLogEntity(DateTimeOffset SendDateTime, string Phone, int InviteMessageId,
    [property: Key] int Id = 0)
{
    /// <summary>
    /// Название таблицы сущности
    /// </summary>
    public const string TableName = "InviteMessagesLog";

    /// <inheritdoc />
    public bool Equals(InviteMessagesLogEntity? other) =>
        other != null && EqualityContract == other.EqualityContract && Id == other.Id;

    /// <inheritdoc />
    public override int GetHashCode() => Id;
}
