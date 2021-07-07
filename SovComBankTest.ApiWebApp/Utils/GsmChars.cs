using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text.RegularExpressions;

namespace SovComBankTest.ApiWebApp.Utils
{
    internal class GsmChars
    {
        private static readonly Regex SmsLegalCharactersRegex = new(
            @"^[@£$¥èéùìòÇ\nØø\rÅå\fΔ_ΦΓΛΩΠΨΣΘΞÆæßÉ !""#¤%&'()*+,\-.\/0-9:;<=>?¡A-ZÄÖÑÜ§¿a-zäöñüà^{}\[~\]\|€А-Яа-я]*$"
        , RegexOptions.Compiled);

        public static bool Check(ReadOnlySpan<char> message) => SmsLegalCharactersRegex.IsMatch(message.ToString());
    }

    internal class Validator
    {
        public static bool TryValidateMessage(ReadOnlySpan<char> message, [NotNullWhen(false)] out string? error)
        {
            if (message.IsEmpty)
            {
                error = "Invite message is missing";
                return false;
            }

            if (!GsmChars.Check(message))
            {
                error =
                    "Invite message should contain only characters in 7-bit GSM encoding or Cyrillic letters as well.";
                return false;
            }

            if (message.Length > 160 || message.Length > 128 && HasAnyCyrillicChar(message))
            {
                error =
                    "Invite message too long, should be less or equal to 128 characters of 7-bit GSM charset.";
                return false;
            }

            error = null;
            return true;
        }

        private static bool HasAnyCyrillicChar(ReadOnlySpan<char> message)
        {
            foreach (var ch in message)
                if (ch switch
                {
                    (> 'А' and < 'Я') or (> 'a' and < 'я') => true,
                    _ => false
                })
                    return true;

            return false;
        }

        
        private static readonly Regex PhoneRegex = new(@"^7\d{10}$", RegexOptions.Compiled);
        public static bool TryValidatePhones(string[]? phones, out string? error)
        {
            if (!(phones?.Length > 0))
            {
                error = "Phone numbers are missing.";
                return false;
            }

            if (phones.Length > 16)
            {
                error = "Too much phone numbers, should be less or equal to 16 per request.";
                return false;
            }

            var phonesSet = phones.ToHashSet();

            if (phonesSet.Count != phones.Length)
            {
                error = "Duplicate numbers detected.";
                return false;
            }

            if (phones.Any(phone => !PhoneRegex.IsMatch(phone)))
            {
                error = "One or several phone numbers do not match with international format.";
                return false;
            }

            error = null;
            return true;
        }
    }
}