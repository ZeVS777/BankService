using Bank.Services.Abstractions;
using Bank.Services.Models;
using Xunit;

namespace Bank.ModuleTests;

/// <summary>
/// Тесты валидацции номеров
/// </summary>
public class PhoneNumbersValidationTests
{
    /// <summary>
    /// Проверка поддерживаемого номера
    /// </summary>
    [Fact]
    public void GivenTheRightPhoneNumber_WhenChecking_ValidationMustBeSuccessful()
    {
        var testData = new[] { "71234567890" };
        var result = ISmsService.TryValidatePhones(testData, out _);
        Assert.True(result);
    }

    /// <summary>
    /// Проверка поддерживаемого количества номеров
    /// </summary>
    [Fact]
    public void GivenTheMaxAmountOfPhoneNumbers_WhenChecking_ValidationMustBeSuccessful()
    {
        var testData = new[] {
            "70000000001",
            "70000000002",
            "70000000003",
            "70000000004",
            "70000000005",
            "70000000006",
            "70000000007",
            "70000000008",
            "70000000009",
            "70000000010",
            "70000000011",
            "70000000012",
            "70000000013",
            "70000000014",
            "70000000015",
            "70000000016"
        };
        var result = ISmsService.TryValidatePhones(testData, out _);
        Assert.True(result);
    }

    /// <summary>
    /// Проверка неподдерживаемого количества номеров
    /// </summary>
    [Fact]
    public void GivenTooMuchPhoneNumbers_WhenChecking_ValidationMustBeFailed()
    {
        var testData = new[] {
            "70000000001",
            "70000000002",
            "70000000003",
            "70000000004",
            "70000000005",
            "70000000006",
            "70000000007",
            "70000000008",
            "70000000009",
            "70000000010",
            "70000000011",
            "70000000012",
            "70000000013",
            "70000000014",
            "70000000015",
            "70000000016",
            "70000000017"
        };

        var result = ISmsService.TryValidatePhones(testData, out var error);
        Assert.True(!result && string.Equals(error, PhoneValidationErrorMessages.TooMuchPhoneNumbers));
    }

    /// <summary>
    /// Получить списки пустых телефонов
    /// </summary>
    /// <returns>Списки пустых телефонов</returns>
    public static IEnumerable<object?[]> GetEmptyPhoneNumbers()
    {
        yield return new object?[] { null };
        yield return new object[] { Array.Empty<string>() };
    }

    /// <summary>
    /// Проверка незаданных номеров
    /// </summary>
    /// <param name="phones">Список номеров</param>
    [Theory]
    [MemberData(nameof(GetEmptyPhoneNumbers))]
    public void GivenNoPhoneNumbers_WhenChecking_ValidationMustBeFailed(string[] phones)
    {
        var result = ISmsService.TryValidatePhones(phones, out var error);
        Assert.True(!result && string.Equals(error, PhoneValidationErrorMessages.PhoneNumbersAbsent));
    }

    /// <summary>
    /// Проверка совпадающих номеров
    /// </summary>
    [Fact]
    public void GivenDuplicatePhoneNumbers_WhenChecking_ValidationMustBeFailed()
    {
        var testData = new[] {
            "70000000001",
            "70000000001"
        };

        var result = ISmsService.TryValidatePhones(testData, out var error);
        Assert.True(!result && string.Equals(error, PhoneValidationErrorMessages.DuplicatePhoneNumbers));
    }

    /// <summary>
    /// Получить списки невалидных номеров
    /// </summary>
    /// <returns>Списки невалидных номеров</returns>
    public static IEnumerable<object?[]> GetNotValidPhoneNumbers()
    {
        yield return new object[] { "7" };
        yield return new object[] { "723456789012" };
        yield return new object[] { "89998887766" };
        yield return new object[] { "7(999)888-77-66" };
    }

    /// <summary>
    /// Проверка невалидных номеров
    /// </summary>
    /// <param name="phone">Номер для проверки</param>
    [Theory]
    [MemberData(nameof(GetNotValidPhoneNumbers))]
    public void GivenNotValidPhoneNumbers_WhenChecking_ValidationMustBeFailed(string phone)
    {
        var result = ISmsService.TryValidatePhones(new[] { phone }, out var error);
        Assert.True(!result && string.Equals(error, PhoneValidationErrorMessages.NotValidPhoneNumberFormat));
    }
}
