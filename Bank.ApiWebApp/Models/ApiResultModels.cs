﻿using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Bank.ApiWebApp.Models;

/// <summary>
/// Результат запроса API.
/// </summary>
internal class ApiResult
{
    /// <summary>
    /// Создать экземпляр класса <see cref="ApiResult"/>
    /// </summary>
    /// <param name="message">Сообщение</param>
    public ApiResult(string? message) => Message = message;

    /// <summary>
    /// Сообщение, сопровождаемое с результатом запроса. Может отсутствовать
    /// </summary>
    /// <example>Возможное сообщение</example>
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? Message { get; }
}

/// <summary>
/// Результат запроса API с данными.
/// </summary>
/// <typeparam name="T">Тип данных результата.</typeparam>
internal sealed class ApiResult<T> : ApiResult
{
    /// <summary>
    /// Создать экземпляр класса <see cref="ApiResult{T}"/>
    /// </summary>
    /// <param name="data">Данные результата</param>
    /// <param name="message">Сообщение</param>
    public ApiResult(T data, string? message = null) : base(message) => Data = data;

    /// <summary>
    /// Данные ответа
    /// </summary>
    [Required]
    public T Data { get; }
}

/// <summary>
/// Описание ошибки.
/// </summary>
internal sealed class ErrorFeatures
{
    /// <summary>
    /// Создать экземпляр класса <see cref="ErrorFeatures"/>
    /// </summary>
    /// <param name="exception">Возникшее исключение</param>
    /// <param name="path">Запрос, при котором возникла ошибка</param>
    public ErrorFeatures(Exception exception, string path) => (ExceptionMessage, Path) = (exception.ToString(), path);

    /// <summary>
    /// Ошибка.
    /// </summary>
    [Required]
    public string ExceptionMessage { get; }

    /// <summary>
    /// Путь запроса, который привёл к ошибке.
    /// </summary>
    [Required]
    public string Path { get; }
}

/// <summary>
/// Ошибки валидации.
/// </summary>
internal sealed class ValidationError
{
    /// <summary>
    /// Создать экземпляр класса <see cref="ValidationError"/>
    /// </summary>
    /// <param name="field">Поле, в котором возникла ошибка</param>
    /// <param name="message">Сообщение о выявленных нарушениях запроса</param>
    private ValidationError(string field, string message) =>
        (Field, Message) = (field != string.Empty ? field : null, message);

    /// <summary>
    /// Поле модели, в котором присутствует ошибка. Может быть null, если нет привязки к конкретному невалидному полю.
    /// </summary>
    /// <example>someField</example>
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? Field { get; }

    /// <summary>
    /// Текст ошибки.
    /// </summary>
    /// <example>Требуется поле</example>
    [Required]
    public string Message { get; }

    /// <summary>
    /// Получить список ошибок валидации из контекста запроса
    /// </summary>
    /// <param name="context">Контекст запроса</param>
    /// <returns>Список ошибок валидации</returns>
    public static IEnumerable<ValidationError> GetValidationErrors(ActionContext context)
    {
        if (context.ModelState.ErrorCount <= 0)
            return Enumerable.Empty<ValidationError>();

        var modelType = context.ActionDescriptor.Parameters
            .FirstOrDefault(p =>
                p.BindingInfo?.BindingSource?.Id.Equals("Body", StringComparison.InvariantCultureIgnoreCase) ?? false)
            ?.ParameterType;

        var result = context.ModelState
            .Where(v => v.Value?.ValidationState == ModelValidationState.Invalid)
            .ToDictionary(
                k => GetPropertyDisplayName(modelType, k.Key),
                v => v.Value!.Errors.Select(e => e.ErrorMessage))
            .SelectMany(kv => kv.Value
                .Select(v => new ValidationError(
                    kv.Key,
                    string.IsNullOrEmpty(v)
                        ? "Not valid format"
                        : v)));

        return result;
    }

    private static string GetPropertyDisplayName(Type? modelType, string modelStateKey)
    {
        static string ToCamelCase(ReadOnlySpan<char> value)
        {
            Span<char> returnValue = new char[value.Length];
            value.CopyTo(returnValue);
            returnValue[0] = char.ToLower(returnValue[0]);

            return returnValue.ToString();
        }

        if (string.IsNullOrWhiteSpace(modelStateKey))
            return modelStateKey;

        if (modelType == null)
            return ToCamelCase(modelStateKey);

        var model = modelType;

        var parametersHierarchy = modelStateKey.Split(".");
        var parametersHierarchyLevels = parametersHierarchy.Length;

        var currentHierarchyLevel = 1;
        var displayNames = new List<string>(parametersHierarchyLevels);
        foreach (var parameter in parametersHierarchy)
        {
            var parameterIsEnumerable = parameter.EndsWith(']');
            var parameterToCheck = parameterIsEnumerable ? parameter.Split('[', 2)[0] : parameter;

            var property = model.GetProperties().FirstOrDefault(p =>
                p.Name.Equals(parameterToCheck, StringComparison.InvariantCultureIgnoreCase));

            if (property == null)
                return ToCamelCase(modelStateKey);

            var name = property.GetCustomAttributes(typeof(JsonPropertyNameAttribute), true)
                .Cast<JsonPropertyNameAttribute>().SingleOrDefault()?.Name;

            if (name == null)
                return ToCamelCase(modelStateKey);

            if (parameterIsEnumerable)
            {
                var enumerator = parameter[parameter.IndexOf('[')..];
                name += enumerator;
            }

            displayNames.Add(name);
            if (currentHierarchyLevel == parametersHierarchyLevels)
                return string.Join('.', displayNames);

            model = parameterIsEnumerable
                ? property.PropertyType.IsGenericType
                    ? property.PropertyType.GenericTypeArguments[0]
                    : property.PropertyType.GetElementType()
                : property.PropertyType;

            if (model == null)
                return ToCamelCase(modelStateKey);

            currentHierarchyLevel++;
        }

        return ToCamelCase(modelStateKey);
    }
}
