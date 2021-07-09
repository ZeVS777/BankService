using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using SovComBankTest.Services.Models;

namespace SovComBankTest.Services.Abstractions
{
    public interface ISmsService
    {
        public static readonly int Threshold = 128;

        public Task<SendResult> SendAsync(InviteMessageModel inviteMessage);

        private static readonly Regex SmsLegalCharactersRegex = new(
            @"^[@£$¥èéùìòÇ\nØø\rÅå\fΔ_ΦΓΛΩΠΨΣΘΞÆæßÉ !""#¤%&'()*+,\-.\/0-9:;<=>?¡A-ZÄÖÑÜ§¿a-zäöñüà^{}\[~\]\|€А-Яа-яЁё]*$"
            , RegexOptions.Compiled);

        private static bool CheckGsmChars(ReadOnlySpan<char> message) => SmsLegalCharactersRegex.IsMatch(message.ToString());

        public static bool TryValidateMessage(ReadOnlySpan<char> message, [NotNullWhen(false)] out string? error)
        {
            error = message switch
            {
                {IsEmpty: true} => InviteMessageValidationErrorMessages.MessageAbsent,
                {Length: > 160} => InviteMessageValidationErrorMessages.TooLong,
                {Length: > 128} when HasAnyCyrillicChar(message) => InviteMessageValidationErrorMessages.TooLong,
                _ when !CheckGsmChars(message) => InviteMessageValidationErrorMessages.NotValidMessageFormat,
                _ => null
            };

            return error == null;
        }

        private static bool HasAnyCyrillicChar(ReadOnlySpan<char> message)
        {
            //Можно и regex, но решил показать возможности языка
            foreach (var ch in message)
                if (ch switch
                {
                    'Ё' or 'ё' => true,
                    (>= 'А' and <= 'Я') or (>= 'а' and <= 'я') => true,
                    _ => false
                })
                    return true;

            return false;
        }

        
        private static readonly Regex PhoneRegex = new(@"^7\d{10}$", RegexOptions.Compiled);
        public static bool TryValidatePhones(string[]? phones, out string? error)
        {
            error = phones switch
            {
                null or {Length: < 1} => PhoneValidationErrorMessages.PhoneNumbersAbsent,
                {Length: > 16} => PhoneValidationErrorMessages.TooMuchPhoneNumbers,
                _ when phones.ToHashSet().Count != phones.Length => PhoneValidationErrorMessages.DuplicatePhoneNumbers,
                _ when phones.Any(phone => !PhoneRegex.IsMatch(phone)) => PhoneValidationErrorMessages.NotValidPhoneNumberFormat,
                _ => null
            };

            return error == null;
        }
    }
}
