namespace Bank.Repositories.Abstractions;

/// <summary>
/// Репозиторий управления сообщениями
/// </summary>
public interface IInviteMessagesRepository
{
    /// <summary>
    /// Получить количество сообщений
    /// </summary>
    /// <param name="apiId">Идентификатор приложения</param>
    /// <param name="date">Дата с которой осуществляется поиск</param>
    /// <returns>Количество сообщений</returns>
    /// <exception cref="DbException">Ошибка обращения к БД</exception>
    Task<int> GetMessagesCountAsync(int apiId, DateTimeOffset date);

    /// <summary>
    /// Добавить новое сообщение
    /// </summary>
    /// <param name="inviteMessage">Данные сообщения</param>
    /// <returns>Количество затронутых записей</returns>
    /// <exception cref="DbException">Ошибка обращения к БД</exception>
    Task<int> AddMessageAsync(InviteMessageEntity inviteMessage);

    /// <summary>
    /// Добавить записи логирования управления сообщениями
    /// </summary>
    /// <param name="log">Записи логирования</param>
    /// <returns>Количество затронутых записей</returns>
    /// <exception cref="DbException">Ошибка обращения к БД</exception>
    Task<int> AddMessageLogEntryAsync(InviteMessagesLogEntity[] log);
}
