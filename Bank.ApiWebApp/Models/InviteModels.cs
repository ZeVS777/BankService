﻿using System.ComponentModel.DataAnnotations;
using Bank.Services.Abstractions;
using Bank.Services.Models;

namespace Bank.ApiWebApp.Models;

/// <summary>
/// Запрос с номерами телефонов и сообщением указанным адресатам
/// </summary>
public sealed class InviteMessage
{
    /// <summary>
    /// Список уникальных номеров
    /// </summary>
    /// <example>["79998887766", "75554443322"]</example>
    [Required(ErrorMessage = PhoneValidationErrorMessages.PhoneNumbersAbsent)]
    public string[] Phones { get; init; } = default!;

    /// <summary>
    /// Сообщение в формате GSM для отправки указанным адресатам
    /// </summary>
    /// <example>Hello</example>
    [Required(ErrorMessage = InviteMessageValidationErrorMessages.MessageAbsent, AllowEmptyStrings = false)]
    public string Message { get; init; } = default!;

    /// <summary>
    /// Идентификатор АПИ
    /// </summary>
    /// <example>4</example>
    [Required(ErrorMessage = "Provide ApiId")]
    public int? ApiId { get; set; }
}

/// <summary>
/// Валидация модели данных отправки сообщения
/// </summary>
internal sealed class InviteMessageValidationAttribute : ValidationAttribute
{
    /// <inheritdoc />
    public override bool IsValid(object? value)
    {
        if (value is not InviteMessage inviteMessage)
            return false;

        var phones = inviteMessage.Phones;

        if (!ISmsService.TryValidatePhones(phones, out var error))
        {
            ErrorMessage = error;

            return false;
        }

        if (!ISmsService.TryValidateMessage(inviteMessage.Message, out error))
        {
            ErrorMessage = error;

            return false;
        }

        return true;
    }
}
