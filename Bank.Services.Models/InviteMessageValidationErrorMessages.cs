namespace Bank.Services.Models
{
    public static class InviteMessageValidationErrorMessages
    {
        public const string MessageAbsent = "Invite message is missing";
        public const string NotValidMessageFormat =
            "Invite message should contain only characters in 7-bit GSM encoding or Cyrillic letters as well.";
        public const string TooLong =
            "Invite message too long, should be less or equal to 128 characters of 7-bit GSM charset.";
    }
}