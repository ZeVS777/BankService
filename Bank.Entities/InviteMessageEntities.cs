using System;
using Dapper.Contrib.Extensions;

namespace Bank.Entities
{
    [Table("InviteMessage")]
    public sealed record InviteMessageEntity(int ApiId, string Message, [property: Key] int Id = 0)
    {
        public override int GetHashCode() => Id;

        public bool Equals(InviteMessageEntity? other) => other != null && EqualityContract == other.EqualityContract && Id == other.Id;
    }

    [Table("InviteMessagesLog")]
    public sealed record InviteMessagesLogEntity(DateTimeOffset SendDateTime, string Phone, int InviteMessageId, [property: Key] int Id = 0)
    {
        public override int GetHashCode() => Id;

        public bool Equals(InviteMessagesLogEntity? other) => other != null && EqualityContract == other.EqualityContract && Id == other.Id;
    }
}
