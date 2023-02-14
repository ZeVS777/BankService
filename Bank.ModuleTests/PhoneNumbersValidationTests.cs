using System;
using System.Collections.Generic;
using Bank.Services.Abstractions;
using Bank.Services.Models;
using Xunit;

namespace Bank.ModuleTests
{
    public class PhoneNumbersValidationTests
    {
        [Fact]
        public void GivenTheRightPhoneNumber_WhenChecking_ValidationMustBeSuccessful()
        {
            var testData = new[] {"71234567890"};
            var result = ISmsService.TryValidatePhones(testData, out _);
            Assert.True(result);
        }
        
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

        public static IEnumerable<object?[]> GetEmptyPhoneNumbers()
        {
            yield return new object?[] { null };
            yield return new object[] { Array.Empty<string>() };
        }

        [Theory]
        [MemberData(nameof(GetEmptyPhoneNumbers))]
        public void GivenNoPhoneNumbers_WhenChecking_ValidationMustBeFailed(string[] phones)
        {
            var result = ISmsService.TryValidatePhones(phones, out var error);
            Assert.True(!result && string.Equals(error, PhoneValidationErrorMessages.PhoneNumbersAbsent));
        }

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

        public static IEnumerable<object?[]> GetNotValidPhoneNumbers()
        {
            yield return new object[] { "7" };
            yield return new object[] { "723456789012" };
            yield return new object[] { "89998887766" };
            yield return new object[] { "7(999)888-77-66" };
        }

        [Theory]
        [MemberData(nameof(GetNotValidPhoneNumbers))]
        public void GivenNotValidPhoneNumbers_WhenChecking_ValidationMustBeFailed(string phone)
        {
            var result = ISmsService.TryValidatePhones(new[] {phone}, out var error);
            Assert.True(!result && string.Equals(error, PhoneValidationErrorMessages.NotValidPhoneNumberFormat));
        }
    }
}
