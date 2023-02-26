namespace Bank.Services.Models;

/// <summary>
/// Модель сообщения
/// </summary>
/// <param name="Phones">Номер телефона, для которого предназначено сообщение</param>
/// <param name="Message">Текст сообщения</param>
/// <param name="ApiId">Идентификатор сообщения</param>
public sealed record InviteMessageModel(string[] Phones, string Message, int ApiId);
