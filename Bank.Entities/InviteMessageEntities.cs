namespace Bank.Entities;

/// <summary>
/// Запись сообщения в БД
/// </summary>
/// <param name="ApiId">Идентификатор приложения</param>
/// <param name="Message">Сообщение</param>
/// <param name="Id">Идентификатор записи</param>
[Table(TableName)]
public sealed record InviteMessageEntity(int ApiId, string Message, [property: Key] int Id = 0)
{
    /// <summary>
    /// Название таблицы сущности
    /// </summary>
    public const string TableName = "InviteMessagesLog";

    /// <inheritdoc />
    public override int GetHashCode() => Id;

    /// <inheritdoc />
    public bool Equals(InviteMessageEntity? other) => other != null && EqualityContract == other.EqualityContract && Id == other.Id;
}
