namespace Bank.Services.Models;

/// <summary>
/// Состояние отправки
/// </summary>
public enum SendStatus
{
    /// <summary>
    /// Успешно
    /// </summary>
    Ok,

    /// <summary>
    /// Превышен лимит
    /// </summary>
    TooMany,

    /// <summary>
    /// Запрещено
    /// </summary>
    Forbidden
}
