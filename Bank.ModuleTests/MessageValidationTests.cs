using Bank.Services.Abstractions;
using Bank.Services.Models;
using Xunit;

namespace Bank.ModuleTests;

/// <summary>
/// Тесты валидации сообщений
/// </summary>
public class MessageValidationTests
{
    /// <summary>
    /// Проверки поддерживаемых сообщений
    /// </summary>
    /// <param name="message">Проверяемое сообщение</param>
    [Theory]
    [InlineData("@£$¥èéùìòÇ\nØø\rÅå\fΔ_ΦΓΛΩΠΨΣΘΞÆæßÉ !\"#¤%&'()*+,-./0123456789:;<=>?¡ABCDEFGHIJKLMNOPQRSTUVWXYZÄÖÑÜ§¿abcdefghijjklmnopqrstuvwxyzzäöñüà^{}[~]|€______1234567890______")]
    [InlineData("АБВГДЕЁЖИКЛМНОПРСТУФХЦЧШЩЬЫЪЭЮЯабвгдеёжзиклмнопрстуфхцчшщьыъэюяАБВГДЕЁЖИКЛМНОПРСТУФХЦЧШЩЬЫЪЭЮЯабвгдеёжзиклмнопрстуфхцчшщьыъэюя12")]
    public void GivenTheRightMessage_WhenChecking_ValidationMustBeSuccessful(string message)
    {
        var result = ISmsService.TryValidateMessage(message, out _);
        Assert.True(result);
    }

    /// <summary>
    /// Проверки слишком длинных сообщений
    /// </summary>
    /// <param name="message">Проверяемое сообщение</param>
    [Theory]
    [InlineData("@£$¥èéùìòÇ\nØø\rÅå\fΔ_ΦΓΛΩΠΨΣΘΞÆæßÉ !\"#¤%&'()*+,-./0123456789:;<=>?¡ABCDEFGHIJKLMNOPQRSTUVWXYZÄÖÑÜ§¿abcdefghijjklmnopqrstuvwxyzzäöñüà^{}[~]|€______1234567890______!")]
    [InlineData("АБВГДЕЁЖИКЛМНОПРСТУФХЦЧШЩЬЫЪЭЮЯабвгдеёжзиклмнопрстуфхцчшщьыъэюяАБВГДЕЁЖИКЛМНОПРСТУФХЦЧШЩЬЫЪЭЮЯабвгдеёжзиклмнопрстуфхцчшщьыъэюя12!")]
    public void GivenTooLongMessage_WhenChecking_ValidationMustBeFailed(string message)
    {
        var result = ISmsService.TryValidateMessage(message, out var error);
        Assert.True(!result && string.Equals(error, InviteMessageValidationErrorMessages.TooLong));
    }

    /// <summary>
    /// Проверки пустых сообщений
    /// </summary>
    /// <param name="message">Проверяемое сообщение</param>
    [Theory]
    [InlineData(null)]
    [InlineData("")]
    public void GivenEmptyMessage_WhenChecking_ValidationMustBeFailed(string? message)
    {
        var result = ISmsService.TryValidateMessage(message, out var error);
        Assert.True(!result && string.Equals(error, InviteMessageValidationErrorMessages.MessageAbsent));
    }
}
