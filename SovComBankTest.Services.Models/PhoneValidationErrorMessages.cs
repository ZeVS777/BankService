namespace SovComBankTest.Services.Models
{
    public static class PhoneValidationErrorMessages
    {
        public const string TooMuchPhoneNumbersPerDay =
            "Too much phone numbers, should be less or equal to 128 per day";
        public const string TooMuchPhoneNumbers = "Too much phone numbers, should be less or equal to 16 per request.";
        public const string DuplicatePhoneNumbers = "Duplicate numbers detected.";
        public const string NotValidPhoneNumberFormat =
            "One or several phone numbers do not match with international format.";
        public const string PhoneNumbersAbsent = "Phone numbers are missing.";
    }
}