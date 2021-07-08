namespace SovComBankTest.Services.Models
{
    public class PhoneValidationErrorMessages
    {
        public const string TooMuchPhoneNumbersPerDay =
            "Too much phone numbers, should be less or equal to 128 per day";
        public const string TooMuchPhoneNumbers = "Too much phone numbers, should be less or equal to 16 per request.";
        public const string DuplicatePhoneNumbers = "Duplicate numbers detected.";
        public const string NotValidPhoneNumberFormat =
            "One or several phone numbers do not match with international format.";
        public const string PhoneNumbersAbsent = "Phone numbers are missing.";
    }

    
    public class InviteMessageValidationErrorMessages
    {
        public const string MessageAbsent = "Invite message is missing";
        public const string NotValidMessageFormat =
            "Invite message should contain only characters in 7-bit GSM encoding or Cyrillic letters as well.";
        public const string TooLong =
            "Invite message too long, should be less or equal to 128 characters of 7-bit GSM charset.";
    }
}