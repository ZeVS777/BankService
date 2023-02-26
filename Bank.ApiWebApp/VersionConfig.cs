using Microsoft.AspNetCore.Mvc;

namespace Bank.ApiWebApp;

/// <summary>
/// Конфигурация версионности API
/// </summary>
internal static class VersionConfig
{
    /// <summary>
    /// Параметр в запросе или заголовке, ответственный за назначение версии запрашиваемого API
    /// </summary>
    public const string ApiVersionParameterName = "api-version";

    /// <summary>
    /// Версия API по умолчанию
    /// </summary>
    public static readonly ApiVersion DefaultApiVersion = new(1, 0);

    /// <summary>
    /// Активные версии АПИ
    /// </summary>
    public static readonly string[] ExistingVersions = { "1.0" };
}
