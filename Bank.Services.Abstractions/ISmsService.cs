using System.Diagnostics.CodeAnalysis;
using System.Text.RegularExpressions;
using Bank.Utils;
using static Bank.Services.Models.InviteMessageValidationErrorMessages;
using static Bank.Services.Models.PhoneValidationErrorMessages;

namespace Bank.Services.Abstractions;

/// <summary>
/// Сервис отправки сообщений
/// </summary>
public partial interface ISmsService
{
    /// <summary>
    /// Максимальная длина сообщения
    /// </summary>
    public const int Threshold = 128;

    /// <summary>
    /// Отправить сообщение
    /// </summary>
    /// <param name="inviteMessage">Данные сообщения</param>
    /// <returns>Результат отправки</returns>
    public Task<SendResult> SendAsync(InviteMessageModel inviteMessage);

    private static readonly Regex SmsLegalCharactersRegex = new(
        @"^[@£$¥èéùìòÇ\nØø\rÅå\fΔ_ΦΓΛΩΠΨΣΘΞÆæßÉ !""#¤%&'()*+,\-.\/0-9:;<=>?¡A-ZÄÖÑÜ§¿a-zäöñüà^{}\[~\]\|€А-Яа-яЁё]*$"
        , RegexOptions.Compiled);

    private static bool CheckGsmChars(ReadOnlySpan<char> message) => SmsLegalCharactersRegex.IsMatch(message.ToString());

    /// <summary>
    /// Провести валидацию сообщения
    /// </summary>
    /// <param name="message"></param>
    /// <param name="error"></param>
    /// <returns><see langword="true"/>, если ошибок не обнаружено, иначе <see langword="false"/></returns>
    public static bool TryValidateMessage(string message, [NotNullWhen(false)] out string? error) =>
        (error = message switch
        {
            null or "" => MessageAbsent,
            { Length: > 160 } => TooLong,
            { Length: > 128 } when HasAnyCyrillicChar(message) => TooLong,
            _ when !CheckGsmChars(message) => NotValidMessageFormat,
            _ => null
        }) == null;

    private static bool HasAnyCyrillicChar(string message) => message.Any(ch => ch.IsCyrillicChar());

    private static readonly Regex PhoneRegex = GetPhoneRegex();

    /// <summary>
    /// Произвести валидацию номеров
    /// </summary>
    /// <param name="phones">Номера</param>
    /// <param name="error">Ошибка валидации</param>
    /// <returns></returns>
    public static bool TryValidatePhones(string[]? phones, [NotNullWhen(false)] out string? error) =>
        (error = phones switch
        {
            null or { Length: < 1 } => PhoneNumbersAbsent,
            { Length: > 16 } => TooMuchPhoneNumbers,
            _ when phones.Distinct().Count() != phones.Length => DuplicatePhoneNumbers,
            _ when phones.Any(phone => !PhoneRegex.IsMatch(phone)) => NotValidPhoneNumberFormat,
            _ => null
        }) == null;

    [GeneratedRegex("^7\\d{10}$", RegexOptions.Compiled)]
    private static partial Regex GetPhoneRegex();
}
