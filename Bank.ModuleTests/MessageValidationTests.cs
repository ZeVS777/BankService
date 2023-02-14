using Bank.Services.Abstractions;
using Bank.Services.Models;
using Xunit;

namespace Bank.ModuleTests
{
    public class MessageValidationTests
    {
        [Theory]
        [InlineData("@£$¥èéùìòÇ\nØø\rÅå\fΔ_ΦΓΛΩΠΨΣΘΞÆæßÉ !\"#¤%&'()*+,-./0123456789:;<=>?¡ABCDEFGHIJKLMNOPQRSTUVWXYZÄÖÑÜ§¿abcdefghijjklmnopqrstuvwxyzzäöñüà^{}[~]|€______1234567890______")]
        [InlineData("АБВГДЕЁЖИКЛМНОПРСТУФХЦЧШЩЬЫЪЭЮЯабвгдеёжзиклмнопрстуфхцчшщьыъэюяАБВГДЕЁЖИКЛМНОПРСТУФХЦЧШЩЬЫЪЭЮЯабвгдеёжзиклмнопрстуфхцчшщьыъэюя12")]
        public void GivenTheRightMessage_WhenChecking_ValidationMustBeSuccessful(string message)
        {
            var result = ISmsService.TryValidateMessage(message, out _);
            Assert.True(result);
        }
        
        [Theory]
        [InlineData("@£$¥èéùìòÇ\nØø\rÅå\fΔ_ΦΓΛΩΠΨΣΘΞÆæßÉ !\"#¤%&'()*+,-./0123456789:;<=>?¡ABCDEFGHIJKLMNOPQRSTUVWXYZÄÖÑÜ§¿abcdefghijjklmnopqrstuvwxyzzäöñüà^{}[~]|€______1234567890______!")]
        [InlineData("АБВГДЕЁЖИКЛМНОПРСТУФХЦЧШЩЬЫЪЭЮЯабвгдеёжзиклмнопрстуфхцчшщьыъэюяАБВГДЕЁЖИКЛМНОПРСТУФХЦЧШЩЬЫЪЭЮЯабвгдеёжзиклмнопрстуфхцчшщьыъэюя12!")]
        public void GivenTooLongMessage_WhenChecking_ValidationMustBeFailed(string message)
        {
            var result = ISmsService.TryValidateMessage(message, out var error);
            Assert.True(!result && string.Equals(error, InviteMessageValidationErrorMessages.TooLong));
        }
        
        [Theory]
        [InlineData(null)]
        [InlineData("")]
        public void GivenEmptyMessage_WhenChecking_ValidationMustBeFailed(string? message)
        {
            var result = ISmsService.TryValidateMessage(message, out var error);
            Assert.True(!result && string.Equals(error, InviteMessageValidationErrorMessages.MessageAbsent));
        }
    }
}