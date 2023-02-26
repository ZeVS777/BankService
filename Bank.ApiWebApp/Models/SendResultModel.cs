using System.ComponentModel.DataAnnotations;

namespace Bank.ApiWebApp.Models;

/// <summary>
/// Результат отправки
/// </summary>
internal sealed class SendResultModel
{
    /// <summary>
    /// Создать экземпляр класса <see cref="SendResultModel"/>
    /// </summary>
    /// <param name="remains">Количество оставшихся возможных отправок сообщений</param>
    /// <param name="sentMessage">Сообщение, которое было отправлено</param>
    public SendResultModel(int remains, string sentMessage) => (Remains, SentMessage) = (remains, sentMessage);

    /// <summary>
    /// Количество оставшихся возможных отправок сообщений
    /// </summary>
    public int Remains { get; }

    /// <summary>
    /// Сообщение, которое было отправлено
    /// </summary>
    [Required]
    public string SentMessage { get; }
}
