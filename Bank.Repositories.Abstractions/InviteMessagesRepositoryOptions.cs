namespace Bank.Repositories.Abstractions;

/// <summary>
/// Настройка репозитория сообщений
/// </summary>
public class InviteMessagesRepositoryOptions
{
    /// <summary>
    /// Строка соединения с БД
    /// </summary>
    public string? ConnectionString { get; init; }
}
