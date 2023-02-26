namespace Bank.Services.Models;

/// <summary>
/// Сообщения, сопровождающие процесс отправки сообщений
/// </summary>
public static class InviteMessageValidationErrorMessages
{
    /// <summary>
    /// Сообщение не найдено
    /// </summary>
    public const string MessageAbsent = "Invite message is missing";

    /// <summary>
    /// Формат сообщения не верный: содержит неподдерживаемые символы
    /// </summary>
    public const string NotValidMessageFormat =
        "Invite message should contain only characters in 7-bit GSM encoding or Cyrillic letters as well.";

    /// <summary>
    /// Формат сообщения не верный: слишком длинное сообщение
    /// </summary>
    public const string TooLong =
        "Invite message too long, should be less or equal to 128 characters of 7-bit GSM charset.";
}
