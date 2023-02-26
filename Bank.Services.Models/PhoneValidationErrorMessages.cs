namespace Bank.Services.Models;

/// <summary>
/// Сообщения валидации запроса на отправку
/// </summary>
public static class PhoneValidationErrorMessages
{
    /// <summary>
    /// Превышен лимит отправки
    /// </summary>
    public const string TooMuchPhoneNumbersPerDay =
        "Too much phone numbers, should be less or equal to 128 per day";

    /// <summary>
    /// Превышен лимит контактов для отправки
    /// </summary>
    public const string TooMuchPhoneNumbers = "Too much phone numbers, should be less or equal to 16 per request.";

    /// <summary>
    /// Имеются повторяющиеся номера
    /// </summary>
    public const string DuplicatePhoneNumbers = "Duplicate numbers detected.";

    /// <summary>
    /// Неверный формат номера
    /// </summary>
    public const string NotValidPhoneNumberFormat =
        "One or several phone numbers do not match with international format.";

    /// <summary>
    /// Отсутствует номер
    /// </summary>
    public const string PhoneNumbersAbsent = "Phone numbers are missing.";
}
