namespace Bank.Services.Models;

/// <summary>
/// Результат отправки сообщения
/// </summary>
/// <param name="Status">Статус отправки</param>
/// <param name="MessagesRemains">Оставшееся количество сообщений</param>
/// <param name="Message">Сообщение</param>
public sealed record SendResult(SendStatus Status, int MessagesRemains, string? Message);
